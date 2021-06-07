using MediaValet.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using SupervisorAPI.Model;
using SupervisorAPI.Service.BusinessLogic;
using SupervisorAPI.Service.Contract;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SupervisorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class OrderController : ControllerBase
    {

        private  int orderCounter;       
        private readonly IOrderQueue _orderQueue;
        private readonly IConfirmationTable _confirmationTable;

        public OrderController(IOrderQueue orderQueue, IConfirmationTable confirmationTable)
        {
            _orderQueue = orderQueue;
            _confirmationTable = confirmationTable;
        }

        [HttpPost]
        [Route("orderqueue")]
        public  async Task<ConfirmationResponse> SendMessageToQueue (OrderMessage msg)
        {
            ConfirmationResponse confirmationResponse=null;
            try
            {
                orderCounter = Utility.GenerateOrderId(_confirmationTable);

                Random random = new Random();
                int randomNumber = random.Next(1, 10);

                Order orderEntity = new Order
                {
                    OrderId = orderCounter,
                    RandomNumber = randomNumber,
                    OrderText = msg.OrderText

                };
                string orderString = System.Text.Json.JsonSerializer.Serialize(orderEntity);

                var orderListQueue = _orderQueue.GetQueue(StorageEntity.OrderStorageQueue);

                orderListQueue.AddMessage(new CloudQueueMessage(Utility.Base64Encode(orderString)));

                CloudTable cloudTable = _confirmationTable.GetTable(StorageEntity.ConfirmationStorageTable);

                TableOperation tableOperation = TableOperation.Retrieve<Confirmation>(orderEntity.OrderId.ToString(), orderEntity.RandomNumber.ToString());
                System.Threading.Thread.Sleep(1000);

                TableResult tableResult = await cloudTable.ExecuteAsync(tableOperation);
                var confirmationResult = tableResult.Result as Confirmation;
                
                if (confirmationResult!=null)
                {
                    if (!string.IsNullOrEmpty(confirmationResult.OrderStatus))
                    {
                        confirmationResponse = new ConfirmationResponse
                        {
                            OrderID = confirmationResult.PartitionKey,
                            AgentId = confirmationResult.AgentId,
                            OrderStatus = confirmationResult.OrderStatus,
                            StatusCode = (int)HttpStatusCode.OK,
                            FaultMessage = ""
                        };
                    }
                }
                else
                {
                    confirmationResponse = new ConfirmationResponse
                    {
                        OrderID = Convert.ToString(orderEntity.OrderId),
                        AgentId = "",
                        OrderStatus = "",
                        StatusCode = (int)HttpStatusCode.Created,
                        FaultMessage = "Order has been added into queue. Agent will process it later."
                    };
                    this.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                }
            }            
            catch (Exception ex)
            {
                throw ex;
            }
            return confirmationResponse;

        }
    }
}