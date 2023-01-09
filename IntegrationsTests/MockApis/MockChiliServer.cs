using System;
using System.Net;
using WireMock.Logging;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace IntegrationsTests.MockApis
{
    public class MockChiliServer : IDisposable
    {
        private WireMockServer _wireMockServer;
        public string Url => _wireMockServer.Url!;

        //Initialize the Mock server with some setting.
        //The port to listen on.
        //Logger will print in console when the wiremock server receives a request
        //AllowPartialMapping Defines if the matching should be done with exact matching or partial matching. Partial matching means that the best matching mapping is used for a input request. In case this setting is set to null or false, only Exact matching is done. This means that only when an exact 100% match is found for an input request, the response is handled. Else you get a error (404).
        public void StartServer()
        {
            var logger = new WireMockConsoleLogger();
            _wireMockServer = WireMockServer.Start( new WireMockServerSettings
                    {
                        Port = 49291,
                        Logger = logger,
                        AllowPartialMapping = true
                    }
            );
        }

        //Real http call : https://chiliapi.com/rest-api/v1/system/apikey?environmentName=test
        //requestBuilder represents the request that will be mapped for the wiremock server
        //if the wiremock server is able to map the request then it will return the responseBuilder
        public void SetUpGetChiliApiKey()
        {
            var requestBuilder = Request.Create()
                .WithPath("/rest-api/v1/system/apikey")
                .WithParam("environmentName", "test")
                .UsingPost()
                .WithBody("{\r\n    \"username\" : \"myuser\",\r\n    \"password\" : \"mypass\"\r\n}")
                .WithHeader("Content-Type", "application/json");

            var responseBuilder = Response.Create()
                .WithHeader("Content-Type", "application/xml; charset=utf-8")
                .WithBody("<apiKey succeeded=\"true\" key=\"testKey\"/>")
                .WithStatusCode(HttpStatusCode.Created);

            _wireMockServer.Given(requestBuilder).RespondWith(responseBuilder);
        }

        public void Dispose()
        {
            _wireMockServer?.Dispose();
            _wireMockServer?.Stop();
        }
    }
}