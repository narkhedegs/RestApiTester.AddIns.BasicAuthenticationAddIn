using System;
using System.Text;
using Newtonsoft.Json;
using RestApiTester.Common;
using Environment = RestApiTester.Common.Environment;

namespace RestApiTester.AddIns
{
    public class BasicAuthenticationAddIn : IBeforeRequestRunAddIn
    {
        public void Execute(
            string configurationJson, IRestRequest request, Environment environment, IRestClient restClient)
        {
            if (string.IsNullOrEmpty(configurationJson))
                throw new ArgumentNullException("configurationJson",
                    "Please provide configuration for " + GetType().Name + ".");

            try
            {
                var configuration =
                    JsonConvert.DeserializeObject<BasicAuthenticationAddInConfiguration>(configurationJson);

                if (configuration.Username == null)
                    throw new ArgumentException("username cannot be null.", "configurationJson");
                if (configuration.Password == null)
                    throw new ArgumentException("password cannot be null.", "configurationJson");

                var authorizationHeaderValue = string.Format("Basic {0}",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(configuration.Username + ":" + configuration.Password)));

                request.Headers.Add("Authorization",authorizationHeaderValue);
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                throw new ArgumentException(
                    "Invalid configuration json. Please provide a valid configuration for " + GetType().Name + ".",
                    "configurationJson", jsonSerializationException);
            }
        }
    }
}
