using Explore.AzStorage.TableStorageApp;
using GS.Ifx.UI;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
/*
Explore.AzTableStorageApp.Program
  
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
using System;
using System.Configuration;

namespace Explore.AzTbleStorageApp
{
    class Program
    {
        private static CloudStorageAccount m_StorageAcct = null;
                
        static void Main(string[] args)
        {
            try
            {
                m_StorageAcct = GetCloudStorageAccount();
                //CloudTable table = ObtainTableCreatIfNotExist("customers");
                CloudTable table = ObtainTableCreatIfNotExist("orders");

                OrderEntity newOrder = new OrderEntity("George", "20150502");
                newOrder.OrderNumber = "1";
                newOrder.ShippedDate = Convert.ToDateTime("5/2/2015");
                newOrder.RequiredDate = Convert.ToDateTime("5/10/2015");
                newOrder.Status = "shipped";
                TableOperation insertOperation = TableOperation.Insert(newOrder);
                table.Execute(insertOperation);
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("409"))
                {
                    Console.WriteLine("Caught exception:\n   Likely RowKey already exists on insert.\n  ex = {0}", ex);
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine("Caught exception:  ex = {0}", ex);
            }
            ConsoleNTraceHelpers.PauseTillUserPressesEnterExitOnX();
        }

        private static CloudTable ObtainTableCreatIfNotExist(string tableName)
        {
            CloudTableClient tableClient = m_StorageAcct.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        private static CloudStorageAccount GetCloudStorageAccount()
        {
            CloudStorageAccount storageAccount = 
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            return storageAccount;
        }
    }
}
