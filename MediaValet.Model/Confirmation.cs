using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MediaValet.Model
{
    public class Confirmation : TableEntity
    {
        public Confirmation(int OrderId, int ramdomNumber)
        {
            this.PartitionKey = Convert.ToString(OrderId); this.RowKey = Convert.ToString(ramdomNumber);
        }
        public Confirmation()
        { }
        public string AgentId { get; set; }
        public string OrderStatus { get; set; }
    }

}
