using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class QueueCreator
    {
        public static async void CreateAzureQueues(string azureConnectionString, string queueName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConnectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                await queueClient.GetQueueReference(queueName).CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
