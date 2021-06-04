using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupervisorAPI.Service.Contract
{
   public interface IConfirmationTable
    {
        CloudTable GetTable(string tableName);
    }
}
