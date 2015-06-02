﻿/*
GS.Contract.DataFeed.IFeedAdmin
  
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

namespace GS.Contract.DataFeed
{
    [ServiceContract(Namespace = "GeorgeStevens/SoApps/6/15")]
    public interface IFeedAdmin
    {
        [OperationContract]
        DataFeedStatistics PresentFeedComponentInfo(string componentName);

        // George 6-1-15
        // There are plenty of potential admin operations.
        // * Add and delete data feed components -- queues, topics, event hubs, etc.
        // * Change the permissions and/or the Shared Access Signatures of feed components.
        // * Look at performance statistics beside queue length, like number of items
        //   passing through per second, day, hour, etc.
        // Etc.
        // The key idea is the admin "facet" is completely different from the
        // data feed "facet" of the IDataFeeds interface (concerned only with ingesting
        // the data".  Thus, keep these concerns separate via interfaces.  Then
        // one can separately choose what manager implements these interfaces.  This
        // allows for cost effective composibility and an agile code base.
    }
}
