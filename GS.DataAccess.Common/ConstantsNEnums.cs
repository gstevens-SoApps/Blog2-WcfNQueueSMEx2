/*
GS.DataAccess.DataFeed.ConstantsNEnums
  
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

namespace GS.DataAccess.Common
{
    public class ConstantsNEnums
    {
        // In the IDesign Method, use cases own the data.  Thus they know all
        // about the data, its storage, and its structure.  Hence table names
        // and entities and their structure stay in the DataAccess project.
        //
        // Other code that needs to use any of this, simply includes this assembly.
        // This makes for apps that are composed of well factored components, rather
        // than putting all data together in one place, etc.
        
        // Constants and Enums for this Data Accessor
        public const string TestMessageTableName = "TestMessages";
        //public const string TestMessageTableName = "TestMessagesNew100It";
        //public const string TestMessageTableName = "TestMessagesNew1000It";
        //public const string TestMessageTableName = "TestMessagesNew10It";
        //public const string TestMessageTableName = "TestMessagesJunk";
    }
}
