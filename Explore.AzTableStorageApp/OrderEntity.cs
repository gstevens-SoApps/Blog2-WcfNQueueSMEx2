/*
GS.Explore.AzTableStorageApp.OrderEntity
  
Copyright 2015 George Stevens

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace GS.Explore.AzTableStorageApp
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
