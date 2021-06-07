using Microsoft.WindowsAzure.Storage.Table;

namespace SupervisorAPI.Service.Contract
{
    public interface IConfirmationTable
    {
        CloudTable GetTable(string tableName);
    }
}
