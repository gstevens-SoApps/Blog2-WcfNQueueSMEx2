﻿using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Explore.AzStorage.TableStorageApp
{
    public class OrderEntity : TableEntity
    {
        public OrderEntity(string customerName, string orderDate)
        {
            PartitionKey = customerName;
            RowKey = orderDate;
        }

        public OrderEntity()
        {}
        public string OrderNumber { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string Status { get; set; }
    }
}
