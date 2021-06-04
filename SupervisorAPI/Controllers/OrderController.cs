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

        private  int _orderCounter;       
        private readonly IOrderQueue _orderQueue;
        private readonly IConfirmationTable _confirmationTable;

        public OrderController(IOrderQueue orderQueue, IConfirmationTable confirmationTable)
        {
            _orderQueue = orderQueue;
            _confirmationTable = confirmationTable;
        }

        [HttpPost]
        [Route("enqueue")]
        public  async Task<ConfirmationResponse> SendMessageToQueue (OrderMessage msg)
        {
            ConfirmationResponse confirmationResponse=null;

            try
            {
                _orderCounter = Utility.GenerateOrderId(_confirmationTable);

                Random random = new Random();
                int randomNumber = random.Next(1, 10);

                Order orderEntity = new Order
                {
                    OrderId = _orderCounter,
                    RandomNumber = randomNumber,
                    OrderText = msg.OrderText

                };
                string orderString = System.Text.Json.JsonSerializer.Serialize(orderEntity);

                var orderListQueue = _orderQueue.GetQueue("orderqueue");

                orderListQueue.AddMessage(new CloudQueueMessage(Utility.Base64Encode(orderString)));

                CloudTable cloudTable = _confirmationTable.GetTable("confirmation");

                TableOperation tableOperation = TableOperation.Retrieve<Confirmation>(orderEntity.OrderId.ToString(), orderEntity.RandomNumber.ToString());
                System.Threading.Thread.Sleep(1000);

                TableResult tableResult = await cloudTable.ExecuteAsync(tableOperation);
                Confirmation confirmationResult = tableResult.Result as Confirmation;
                
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
                        OrderID = "",
                        AgentId = "",
                        OrderStatus = "",
                        StatusCode = (int)HttpStatusCode.OK,
                        FaultMessage = "No confirmation received from agent!"
                    };
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