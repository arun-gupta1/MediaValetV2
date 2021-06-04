using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class QueueCreator
    {
        public static void CreateAzureQueues(string azureConnectionString, string queueName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConnectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                queueClient.GetQueueReference(queueName).CreateIfNotExists();
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
    }
}
