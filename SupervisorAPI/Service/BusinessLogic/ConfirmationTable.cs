using MediaValet.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SupervisorAPI.Service.Contract;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class ConfirmationTable : IConfirmationTable
    {
        private readonly CloudTableClient _tableClient;
        public ConfirmationTable()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureStorageConnection.ConnectionString);
                _tableClient = storageAccount.CreateCloudTableClient();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CloudTable GetTable(string tableName)
        {
            return _tableClient.GetTableReference(tableName);
        }
    }
}
