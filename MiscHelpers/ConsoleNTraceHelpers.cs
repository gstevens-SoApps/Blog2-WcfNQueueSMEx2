/*
MiscHelpers.ConsoleNTraceHelpers
  
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
using System.Text;
using Shared;
using System;
using System.Diagnostics;

namespace MiscHelpers
{
    public class ConsoleNTraceHelpers
    {
        public static string PauseTillUserPressesEnter()
        {
            Console.WriteLine("Press <ENTER> to continue....");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public static string PauseTillUserPressesEnterExitOnX()
        {
            Console.WriteLine("Press <ENTER> to continue  OR enter 'x' then press <ENTER> to EXIT....");
            string userInput = Console.ReadLine();
            return userInput;
        }

        private const string TestMessageStartDisplayString = "**TestMessage follows:\n";

        public static void DisplayTestMessage(TestMessage msg)
        {
            Console.WriteLine(FormatTestMessageDisplay(msg));
        }

        public static void TraceTestMessage(TestMessage msg)
        {
            Trace.TraceInformation("\n" + FormatTestMessageDisplay(msg));
        }

        private static string FormatSbMessageDisplay(SbMessage msg)
        {
            return string.Format("\n  MessageId={0},\n  MessageDateTime={1}",
                                 msg.MessageId, msg.MessageDateTime);
        }

        private static string FormatTestMessageDisplay(TestMessage msg)
        {
            StringBuilder sb = new StringBuilder(TestMessageStartDisplayString);
            
            sb.Append(string.Format("  SourceMsgSeqNum={0}, SourceId={1}, SourceGroupId={2}\n",
                                    msg.SourceMsgSeqNumber, msg.SourceId, msg.SourceGroupId));
            sb.Append(string.Format("  MsgBody={0}", msg.MsgBody));
            sb.Append(FormatSbMessageDisplay(msg));
            return sb.ToString();
        }
    }
}
