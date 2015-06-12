/*
GS.Engine.Something.SomeDataAnalysisEngine
  
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

using GS.Contract.Something;
using GS.DataAccess.Common;
using GS.iFX.Service;
using ServiceModelEx;

namespace GS.Engine.Something
{
    public class SomeDataAnalysisEngine : WcfNQueueSMEx2Service, ISomeDataAnalysisEngine
    {
        IngestedDataAnalysisResult ISomeDataAnalysisEngine.DoIngestedDataAnalysis()
        {
            // Get the ingested data from the DB.
            IIngestedDataDA ingestDataDaProxy = InProcFactory.CreateInstance<IngestedDataDA, IIngestedDataDA>();
            string dummyDataForAnalysis = ingestDataDaProxy.GetDummyDataForAnalysis();

            // Do the analysis.
            IngestedDataAnalysisResult result = new IngestedDataAnalysisResult();
            InProcFactory.CloseProxy(ingestDataDaProxy);
            result.ResultPart1 = dummyDataForAnalysis;
            int analysisResult = dummyDataForAnalysis.Length;
            result.ResultPart2 = 
                string.Format("The analysis of the dummy data reveals it contains {0} characters", analysisResult);
            return result;
        }
    }
}
