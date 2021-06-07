using MediaValet.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using NLog;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Agent
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static async Task Main(string[] args)
        {
            try
            {
                await ProcessQueueMessage();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine("Something went wrong. Agent Exit.");
            }
        }
        private static async Task ProcessQueueMessage()
        {
            var agentId = Guid.NewGuid();
            Random random = new Random();
            int randomNumber = random.Next(1, 10);

            Console.WriteLine(" I’m agent " + agentId + ", my magic number is " + randomNumber);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageConnection.ConnectionString);
            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(StorageEntity.OrderStorageQueue);
            await cloudQueue.CreateIfNotExistsAsync();

            for (int a = 0; a < 1; a--)
            {
                CloudQueueMessage retrievedMessage = await cloudQueue.GetMessageAsync();
                if (retrievedMessage != null)
                {
                    if (!string.IsNullOrEmpty(retrievedMessage.AsString))
                    {
                        Order orderEntity = JsonSerializer.Deserialize<Order>(Base64Decode(retrievedMessage.AsString));
                        Console.WriteLine("Received order: " + orderEntity.OrderId);

                        if (randomNumber == orderEntity.RandomNumber)
                        {
                            Console.WriteLine("Oh no, my magic number was found ");
                            break;
                        }
                        Console.WriteLine("Order message: " + orderEntity.OrderText);

                        CloudTableClient Tableclient = storageAccount.CreateCloudTableClient();
                        CloudTable table = Tableclient.GetTableReference(StorageEntity.ConfirmationStorageTable);
                        await table.CreateIfNotExistsAsync();

                        Confirmation obj = new Confirmation(orderEntity.OrderId, orderEntity.RandomNumber)
                        {
                            AgentId = agentId.ToString(),
                            OrderStatus = "Processed"
                        };

                        TableOperation insertOperation = TableOperation.Insert(obj);
                        await table.ExecuteAsync(insertOperation);
                        await cloudQueue.DeleteMessageAsync(retrievedMessage);
                    }
                }
            }
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
