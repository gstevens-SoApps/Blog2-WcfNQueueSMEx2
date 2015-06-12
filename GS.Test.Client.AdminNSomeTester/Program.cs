/*
GS.Test.Client.AdminNSomeTester.Program
  
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

using GS.Contract.Admin;
using GS.Contract.Something;
using GS.iFX.Common;
using GS.iFX.TestUI;
using GS.Proxy.Admin;
using System;
using GS.Proxy.Something;

namespace GS.Test.Client.AdminNSomeTester
{
    class Program
    {
        private static string m_ThisName = "AdminNSomeTester";

        static void Main(string[] args)
        {
            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Press <ENTER> to continue.", m_ThisName);

            while (true)
            {
                string userInput = PauseTillUserPressesEnter_OrAComand_OrExitOnX().ToLower();
                if (userInput == "x")
                {
                    break;
                }
                if (userInput == "qs")
                {
                    DisplayQueueStatistics(ConstantsNEnums.IngestionQueueName);
                }
                else if (userInput == "ql")
                {
                    DisplayQueueLength(ConstantsNEnums.IngestionQueueName);
                }
                else if (userInput == "an")
                {
                    DisplayDataAnalysisResult();
                }
                else
                {
                    // Assume the queue exists, having been previously provisioned in Azure Portal.
                    Console.WriteLine("{0}.Main(): Oops, unrecognized command.  Try again...", m_ThisName);
                }
            }
        }

        private static void DisplayDataAnalysisResult()
        {
            Console.WriteLine("{0}.Main(): Analyzing ingested data..", m_ThisName);
            IngestedDataAnalysisResult result = AnalyzeIngestedData();
            string analysisMsg;
            if (result != null)
            {
                analysisMsg = string.Format("{0}.Main():\nData Analysis Result Part 1 =\n  {1}", m_ThisName, result.ResultPart1);
                analysisMsg += string.Format("\nData Analysis Result Part 2 =\n  {0}", result.ResultPart2);
            }
            else
            {
                analysisMsg = "Server return null.";
            }
            Console.WriteLine(analysisMsg);
        }

        private static void DisplayQueueLength(string queueName)
        {
            Console.WriteLine("{0}.Main(): Getting queue length..", m_ThisName);
            DataFeedStatistics stats = PresentQueueStatistics(queueName);
            string lengthMsg;
            if (stats != null)
            {
                lengthMsg = string.Format("{0}.Main(): Queue length = {1}", m_ThisName, stats.QueueLength);
            }
            else
            {
                lengthMsg = "Server return null.";
            }
            Console.WriteLine(lengthMsg);
        }

        private static void DisplayQueueStatistics(string queueName)
        {
            Console.WriteLine("{0}.Main(): Getting queue statistics..", m_ThisName);
            DataFeedStatistics stats = PresentQueueStatistics(queueName);
            string statMsg = string.Format("{0}.Main(): Queue statistics..", m_ThisName);
            Console.WriteLine(statMsg);
            if (stats != null)
            {
                statMsg =
                    string.Format("   queueName={0}, queueLength={1},\n   deadLetterQueueLength={2}, statsTime={3}",
                        stats.FeedComponentName, stats.QueueLength,
                        stats.DeadLetterQueueLength, stats.StatsCollectionDateTime);
            }
            else
            {
                statMsg = "Server return null.";
            }
            Console.WriteLine(statMsg);
        }

        private static string PauseTillUserPressesEnter_OrAComand_OrExitOnX()
        {
            Console.WriteLine("\nEnter commands as follows:\n    enter 'qs'<ENTER> for Queue Stats,\n OR enter 'ql'<ENTER> for Queue Length,\n OR enter 'an'<ENTER> for Data Analysis,\n OR enter 'x'<ENTER> to EXIT....");
            string userInput = Console.ReadLine();
            return userInput;
        }

        private static DataFeedStatistics PresentQueueStatistics(string queueName)
        {
            DataFeedStatistics stats = null;
            FeedAdminClient proxy = new FeedAdminClient("feedAdmin");
            try
            {
                stats = proxy.PresentFeedComponentInfo(queueName);
                proxy.Close();
            }
            catch (Exception ex)
            {
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".PresentQueueStatistics(): Exception!! ", ex);
                proxy.Abort();
            }
            return stats;
        }

        private static IngestedDataAnalysisResult AnalyzeIngestedData()
        {
            IngestedDataAnalysisResult result = null;
            SomeDataAnalysisClient proxy = new SomeDataAnalysisClient("someDataAnalysis");
            try
            {
                result = proxy.AnalyzeIngestedData();
                proxy.Close();
            }
            catch (Exception ex)
            {
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".AnalyzeIngestedData(): Exception!! ", ex);
                proxy.Abort();
            }
            return result;
        }
    }
}
