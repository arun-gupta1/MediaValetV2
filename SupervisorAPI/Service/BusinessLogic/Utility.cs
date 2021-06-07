using MediaValet.Model;
using Microsoft.WindowsAzure.Storage.Table;
using SupervisorAPI.Model;
using SupervisorAPI.Service.Contract;
using System;
using System.Collections.Generic;

namespace SupervisorAPI.Service.BusinessLogic
{
    public static class Utility
    {       
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static int GenerateOrderId(IConfirmationTable confirmationTable)
        {
            int orderId = 0;
            CloudTable cloudTable = confirmationTable.GetTable(StorageEntity.OrderCountStorageTable);

            TableContinuationToken tableToken = null;
            var orderEntities = new List<OrderCounter>();
            do
            {
                var queryResult = cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<OrderCounter>(), tableToken);
                orderEntities.AddRange(queryResult.Result);
            } while (tableToken != null);


            if (orderEntities.Count > 0)
            {
                var orderEntity = orderEntities[0] as OrderCounter;
                orderId = Convert.ToInt32(orderEntity.orderid) + 1;
                orderEntity.orderid = orderId;

                TableOperation updateOperation = TableOperation.Replace(orderEntity);
                cloudTable.ExecuteAsync(updateOperation);
            }
            else
            {
                OrderCounter obj = new OrderCounter("Order", 1);
                obj.orderid = orderId;
                TableOperation insertOperation = TableOperation.Insert(obj);
                cloudTable.ExecuteAsync(insertOperation);
            }

            return orderId;
        }
    }
}
