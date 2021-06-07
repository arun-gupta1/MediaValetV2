using System;
using System.Collections.Generic;
using System.Text;

namespace MediaValet.Model
{
    public static class StorageEntity
    {
        public static string OrderStorageQueue { get { return "orderqueuenew"; } }
        public static string ConfirmationStorageTable { get { return "confirmationnew"; } }
        public static string OrderCountStorageTable { get { return "ordercountnew"; } }

    }
}
