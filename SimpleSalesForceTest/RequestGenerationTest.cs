using System.Collections.Generic;
using NUnit.Framework;


namespace SimpleSalesForceTest
{ 
    [TestFixture]
    public class RequestGenerationTests
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DefaultRequest()
        {

            string validationString = "https://mypkscommunity-developer-edition.na102.force.com/NTOCustomers/services/oauth2/authorize?response_type=token+id_token&client_id=YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY&redirect_uri=finsmaaa://sflogin&state=FINSM&nonce=1234&scope=openid%20refresh_token";

            List<SimpleSalesforce.Response_Type> responseType = new List<SimpleSalesforce.Response_Type>();
            responseType.Add(SimpleSalesforce.Response_Type.token);
            responseType.Add(SimpleSalesforce.Response_Type.id_token);

            string uri = "https://mypkscommunity-developer-edition.na102.force.com/NTOCustomers/services/oauth2/authorize";
            string clientId    = "YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY";
            string redirectUri = "finsmaaa://sflogin";

            SimpleSalesforce.AccessTokenRequestParameters request = new SimpleSalesforce.AccessTokenRequestParameters(uri,responseType, clientId, redirectUri);


            List<SimpleSalesforce.Scope> scope = new List<SimpleSalesforce.Scope>();
            scope.Add(SimpleSalesforce.Scope.openid);
            scope.Add(SimpleSalesforce.Scope.refresh_token);

            request.Scope = scope;

            request.Nonce = "1234";


            string requestString = request.ToString();


            Assert.AreEqual(validationString, request);
        }

    }
}
