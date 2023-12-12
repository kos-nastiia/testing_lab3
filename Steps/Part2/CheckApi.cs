using Newtonsoft.Json;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;

namespace lab3_Testing.Steps.Part2
{
    [Binding]
    public class GeoLocationSteps
    {
        private readonly RestClient _client;
        private RestRequest _request;
        private RestResponse _response;

        public GeoLocationSteps()
        {
            _client = new RestClient("https://api.country.is/");
        }

        [Given(@"I have a valid IPv4 address '(.*)'")]
        public void GivenIHaveAValidIPvAddress(string ipv4Address)
        {
            _request = new RestRequest(ipv4Address);
        }

        [When(@"I query the GeoLocation API")]
        public void WhenIQueryTheGeoLocationAPI()
        {
            _response = _client.Execute(_request);
        }

        [Then(@"I should receive the country code '(.*)'")]
        public void ThenIShouldReceiveTheCountryCode(string expectedCountryCode)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(_response.Content);
            Assert.NotNull(result);
            Assert.AreEqual(expectedCountryCode, result["country"]);
        }

        [Then(@"I should receive an error response")]
        public void ThenIShouldReceiveAnErrorResponse()
        {
            Assert.IsFalse(_response.IsSuccessful, "The response was successful but should have been an error.");
            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)
                .Or.EqualTo(HttpStatusCode.NotFound)
                .Or.EqualTo(HttpStatusCode.InternalServerError), "The status code was not an expected error code.");
        }
    }
}