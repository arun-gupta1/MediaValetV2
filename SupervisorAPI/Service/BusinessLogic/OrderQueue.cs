using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Options;
using SupervisorAPI.Infrastructure.AzureStorageSetting;
using SupervisorAPI.Service.Contract;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class OrderQueue: IOrderQueue
    {
        private readonly CloudQueueClient _queueClient;
        public OrderQueue(IOptions<AzureStorageConnection> settings)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(settings.Value.ConnectionString);
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
