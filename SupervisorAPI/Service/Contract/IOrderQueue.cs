using Microsoft.Azure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupervisorAPI.Service.Contract
{
    public interface IOrderQueue
    {
        CloudQueue GetQueue(string queueName);
    }
}
