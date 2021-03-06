﻿/*
GS.iFX.Azure.AzureStorageHelpers
  
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

using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace GS.iFX.Azure
{
    // George 5-31-15.  Please see the comments in AzureServiceBusHelpers for the
    // purpose of this class.

    public class AzureStorageHelpers
    {
        public static void AzTableStorageInsert<T>(string tableName, T msgEntity) 
            where T :TableEntity
        {
            CloudTable table = ObtainTableCreatIfNotExist(tableName);
            TableOperation insertOperation = TableOperation.Insert(msgEntity);
            table.Execute(insertOperation);
        }

        public static CloudTable ObtainTableCreatIfNotExist(string tableName)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        public static CloudStorageAccount GetCloudStorageAccount()
        {
            // TODO 6-12-15 George.  Make this string a constant.
            // The app.config file resides in the service host of the service calling this.
            string connString = ConfigurationManager.AppSettings["StorageConnectionString"];
            Debug.Assert(!string.IsNullOrEmpty(connString));
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            return storageAccount;
        }
    }
}
