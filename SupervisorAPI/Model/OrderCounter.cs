using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace SupervisorAPI.Model
{
    public class OrderCounter: TableEntity
    {
        public OrderCounter(string tablename, int tableid)
        {
            this.PartitionKey = tablename; this.RowKey = Convert.ToString(tableid);
        }
        public OrderCounter()
        { }

        public int orderid { get; set; }
    }
}
