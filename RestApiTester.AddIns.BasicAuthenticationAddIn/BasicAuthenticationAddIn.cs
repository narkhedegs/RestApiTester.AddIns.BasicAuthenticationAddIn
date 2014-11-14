using System;
using RestApiTester.Common;
using Environment = RestApiTester.Common.Environment;

namespace RestApiTester.AddIns
{
    public class BasicAuthenticationAddIn : IBeforeRequestRunAddIn
    {
        public void Execute(string configurationJson, IRestRequest request, Environment environment, IRestClient restClient)
        {
            throw new NotImplementedException();
        }
    }
}
