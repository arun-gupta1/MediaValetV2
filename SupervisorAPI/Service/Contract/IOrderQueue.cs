using Microsoft.Azure.Storage.Queue;

namespace SupervisorAPI.Service.Contract
{
    public interface IOrderQueue
    {
        CloudQueue GetQueue(string queueName);
    }
}
