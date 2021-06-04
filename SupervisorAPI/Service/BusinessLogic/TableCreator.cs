using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace SupervisorAPI.Service.BusinessLogic
{
    public class TableCreator
    {
        public static void CreateAzureTables(string azureConnectionString, string tableName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConnectionString);
                CloudTableClient Tableclient = storageAccount.CreateCloudTableClient();
                CloudTable table = Tableclient.GetTableReference(tableName);
                table.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
