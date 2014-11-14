using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NSpec;
using RestApiTester.Common;

namespace RestApiTester.AddIns.Specifications
{
    public class basic_authentication_addin_specifications : nspec
    {
        private RestApiTester.AddIns.BasicAuthenticationAddIn _basicAuthenticationAddIn;
        private string _configurationJson;
        private Mock<IRestRequest> _restRequest;

        public void when_executing_basic_authentication_addin()
        {
            before = () =>
            {
                _basicAuthenticationAddIn = new RestApiTester.AddIns.BasicAuthenticationAddIn();

                _restRequest = new Mock<IRestRequest>();
                _restRequest.Setup(request => request.Headers).Returns(new Dictionary<string, string>());
            };

            act = () => _basicAuthenticationAddIn.Execute(_configurationJson, _restRequest.Object, null, null);

            context["given null or empty configuration json"] = () =>
            {
                before = () => _configurationJson = null;

                it["should throw ArgumentNullException"] = expect<ArgumentNullException>();
            };

            context["given invalid configuration json"] = () =>
            {
                before = () =>
                {
                    _configurationJson =
                        "{" +
                        "\"invalidProperty1\":1," +
                        "\"invalidProperty2\":2";
                };

                it["should throw ArgumentException"] = expect<ArgumentException>();
            };

            context["given null username"] = () =>
            {
                before = () =>
                {
                    _configurationJson =
                        "{" +
                        "\"username\":null," +
                        "\"password\":\"some password\"" +
                        "}";
                };

                it["should throw ArgumentException"] = expect<ArgumentException>();
            };

            context["given null password"] = () =>
            {
                before = () =>
                {
                    _configurationJson =
                        "{" +
                        "\"username\":\"some username\"," +
                        "\"password\":null" +
                        "}";
                };

                it["should throw ArgumentException"] = expect<ArgumentException>();
            };

            context["given valid configuration json"] = () =>
            {
                const string username = "some username";
                const string password = "some password";
                var expectedAuthorizationHeaderValue = string.Format("Basic {0}",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));

                before = () =>
                {
                    _configurationJson =
                        "{" +
                        "\"username\":\"" + username + "\"," +
                        "\"password\":\"" + password + "\"," +
                        "}";
                };

                it["should populate Request Authorization Header"] =
                    () =>
                        _restRequest.Object.Headers.should_contain(
                            header => header.Key == "Authorization" && header.Value == expectedAuthorizationHeaderValue);
            };
        }
    }
}
