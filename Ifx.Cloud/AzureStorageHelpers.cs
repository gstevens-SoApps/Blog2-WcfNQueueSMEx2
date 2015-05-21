/*
GS.Ifx.Cloud.AzureStorageHelpers
  
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

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace GS.Ifx.Cloud
{
    public class AzureStorageHelpers
    {
        // TODO Put this in Infrastructure somewhere.
        public static CloudTable ObtainTableCreatIfNotExist(string tableName)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        // TODO Put this in Infrastructure somewhere.  And use a constant.
        public static CloudStorageAccount GetCloudStorageAccount()
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            return storageAccount;
        }
    }
}
