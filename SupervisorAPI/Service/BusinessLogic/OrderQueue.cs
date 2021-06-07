using MediaValet.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using SupervisorAPI.Service.Contract;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class OrderQueue: IOrderQueue
    {
        private readonly CloudQueueClient _queueClient;
        public OrderQueue()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageConnection.ConnectionString);
                _queueClient = storageAccount.CreateCloudQueueClient();
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
        public CloudQueue GetQueue(string queueName)
        {
            return _queueClient.GetQueueReference(queueName);
        }
    }
}
