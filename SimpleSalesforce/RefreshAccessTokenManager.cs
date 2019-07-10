using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSalesforce
{

    /*

        OAuth 2.0 Refresh Token Flow
        https://help.salesforce.com/articleView?id=remoteaccess_oauth_refresh_token_flow.htm&type=5
        https://help.salesforce.com/articleView?id=remoteaccess_oauth_jwt_flow.htm&type=5

        Since we are following a flow where the client secret can't be stored on individual devices, we will
        not have to provide a private key encryption to get a new Access Token.
        This flow is greatly simplified with a simple post request to Salesforce.

    */


    /// <summary>
    ///     Using the current refresh token, we need to use the Refresh Token, and get a new Access Token.
    /// </summary>
    public class RefreshAccessTokenManager
    {
        /// <summary>
        /// The required default constructor
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="clientId"></param>
        /// <param name="refreshToken"></param>
        public RefreshAccessTokenManager(string uri, string clientId, string refreshToken)
        {
            //HMACSHA256.Create()

            _client_id     = clientId;
            _refresh_token = refreshToken;
            _uri = uri;

        }

            string _grant_type;
            string _refresh_token;
            string _client_id;
            string _client_secret;
            string _uri;

        IdToken _idToken;
        AccessToken _accessToken;
        RefreshToken _refreshToken;

        /// <summary>
        /// The updated access token
        /// </summary>
        public AccessToken AccessToken { get => _accessToken; set => _accessToken = value; }
        /// <summary>
        /// The updated IdToken
        /// </summary>
        public IdToken IdToken { get => _idToken; set => _idToken = value; }
        /// <summary>
        /// The Refresh Token Object used to Refresh the IdToken and the AccessToken
        /// </summary>
        public RefreshToken RefreshToken { get => _refreshToken; set => _refreshToken = value; }


        /// <summary>
        /// Returns the 3 user tokens used in the Salesforce Operations.
        /// </summary>
        /// <returns>UserTokens</returns>
        public UserTokens GetUserTokens()
        {
            UserTokens returnValue = new UserTokens();
            returnValue.AccessToken = AccessToken;
            returnValue.RefreshToken = RefreshToken;
            returnValue.IdToken = IdToken;

            return returnValue;

        }




        /// <summary>
        /// Will connect to Salesforce and Update the current users IdToken and AccessToken.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            
            /*
                POST / services / oauth2 / token HTTP / 1.1
                Host: login.salesforce.com /
                grant_type = refresh_token & client_id = 3MVG9lKcPoNINVBIPJjdw1J9LLM82HnFVVX19KY1uA5mu0
                QqEWhqKpoW3svG3XHrXDiCQjK1mdgAvhCscA9GE & client_secret = 1955279925675241571
                & refresh_token = your token here



                client_secret_basic, client_secret_post, client_secret_jwt: Use one of these methods when the client has a client secret.
                Public clients (such as single-page and mobile apps) that can't protect a client secret must use none below.

                private_key_jwt: Use this when you want maximum security. This method is more complex and requires a server, so it can't be used with public clients.
                none - Use this with clients that don't have a client secret (such as applications that use the authorization code flow with PKCE or the implicit flow).

            */



            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("grant_type", "refresh_token");
            parameters.Add("client_id", _client_id);
            parameters.Add("refresh_token", _refresh_token);


            using (HttpClient _client = new HttpClient())
            {

                _client.BaseAddress = new Uri(_uri);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _client.PostAsync("", content);


                response.EnsureSuccessStatusCode();

                string responseText = await response.Content.ReadAsStringAsync();

                RefreshAccessTokenResponse responseToken = Newtonsoft.Json.JsonConvert.DeserializeObject<RefreshAccessTokenResponse>(responseText);

                //if the response token contains any errors, then throw the error to exit this operation.
                if(responseToken.HasErrors)
                {
                    throw new SalesForceException(responseToken.error, responseToken.error_description);
                }


                //assign the token properties for this current operation. these will later be retrieved in the UserTokens object.
                _accessToken = new AccessToken() { TokenValue = responseToken.access_token };
                _refreshToken = new RefreshToken() { TokenValue = _refresh_token };
                _idToken = IdToken.GetIdToken(responseToken.id_token);


            }
           
        }
    }





}
