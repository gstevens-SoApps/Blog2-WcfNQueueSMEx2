/*
GS.DataAccess.DataFeedAdmin.IFeedAdminDA
  
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

using System.ServiceModel;
using GS.Contract.DataFeed;

namespace GS.DataAccess.DataFeedAdmin
{
    [ServiceContract(Namespace = "GeorgeStevens/SoApps/5/15")]
    public interface IFeedAdminDA
    {
        [OperationContract]
        long GetQueueLength(string queueName);
        [OperationContract]
        DataFeedStatistics GetFeedStatistics(string queueName);
    }
}
