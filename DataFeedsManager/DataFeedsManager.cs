/*
DataFeedsManager.DataFeedsManager
  
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
using Shared;
using System;
using System.Diagnostics;
using MiscHelpers;
using ServiceModelEx;

namespace DataFeedsManager
{
    [GenericResolverBehavior] 
    public class DataFeedsManager : IDataFeeds
    {
        private string m_ThisName = "DataFeedsManager";

        void IDataFeeds.IngestTestData(TestMessage msg)
        {
            string greeting = String.Format("\n{0}.IngestTestData(): Entered.", m_ThisName);
            Console.WriteLine(greeting);
            Trace.TraceInformation("**" + greeting);
            ConsoleNTraceHelpers.DisplayTestMessage(msg);
            ConsoleNTraceHelpers.TraceTestMessage(msg);

            // Below is where the code goes that does the work of this service operation.
            // That code will be developed in subsequent iterations.
        }
    }
}
