using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using SupervisorAPI.Infrastructure.AzureStorageSetting;
using SupervisorAPI.Service.Contract;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class ConfirmationTable: IConfirmationTable
    {
        private readonly CloudTableClient _tableClient;
        public ConfirmationTable(IOptions<AzureStorageConnection> settings)
        {
            try
            {
                Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(settings.Value.ConnectionString);
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
