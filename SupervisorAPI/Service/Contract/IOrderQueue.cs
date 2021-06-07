using Microsoft.WindowsAzure.Storage.Queue;

namespace SupervisorAPI.Service.Contract
{
    public interface IOrderQueue
    {
        CloudQueue GetQueue(string queueName);
    }
}
