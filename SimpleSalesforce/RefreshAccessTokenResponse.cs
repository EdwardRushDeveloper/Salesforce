using System;
namespace SimpleSalesforce
{

    /// <summary>
    /// The refresh token response
    /// https://help.salesforce.com/articleView?id=remoteaccess_oauth_refresh_token_flow.htm&type=5
    /// </summary>
    public class RefreshAccessTokenResponse
    {
        public RefreshAccessTokenResponse()
        {

            _error = string.Empty;
            _error_description = string.Empty;

        }



        string _access_token;
        string _sfdc_community_url;
        string _sfdc_community_id;
        string _signature;
        string _scope;
        string _id_token;
        string _instance_url;
        string _id;
        string _token_type;
        string _issued_at;
        string _error;
        string _error_description;


        /// <summary>
        /// Salesforce session ID that can be used with the web services API.
        /// </summary>
        public string access_token { get => _access_token; set => _access_token = value; }
        /// <summary>
        /// If the user is a member of a Salesforce community, the community URL is provided.
        /// </summary>
        public string sfdc_community_url { get => _sfdc_community_url; set => _sfdc_community_url = value; }
        /// <summary>
        /// If the user is a member of a Salesforce community, the user’s community ID is provided.
        /// </summary>
        public string sfdc_community_id { get => _sfdc_community_id; set => _sfdc_community_id = value; }
        /// <summary>
        /// Base64-encoded HMAC-SHA256 signature signed with the client_secret (private key) containing the concatenated ID and issued_at.
        /// Used to verify that the identity URL hasn’t changed since the server sent it.
        /// </summary>
        public string signature { get => _signature; set => _signature = value; }
        /// <summary>
        /// A space-separated list of scope values. The scope parameter fine-tunes the permissions associated with the tokens that you’re requesting.
        /// Scope is a subset of values that you specified when defining the connected app. 
        /// </summary>
        public string scope { get => _scope; set => _scope = value; }
        /// <summary>
        /// Salesforce value conforming to the OpenID Connect specifications.
        /// This parameter is returned if an existing approval includes the openid scope, and is picked up by the bearer token process.
        /// </summary>
        public string id_token { get => _id_token; set => _id_token = value; }
        /// <summary>
        /// A URL indicating the instance of the user’s org. For example: https://yourInstance.salesforce.com/.
        /// </summary>
        public string instance_url { get => _instance_url; set => _instance_url = value; }
        /// <summary>
        /// Identity URL that can be used to both identify the user and query for more information about the user
        /// </summary>
        public string id { get => _id; set => _id = value; }
        /// <summary>
        /// Value is Bearer for all responses that include an access token.
        /// </summary>
        public string token_type { get => _token_type; set => _token_type = value; }
        /// <summary>
        /// When the signature was created.
        /// </summary>
        public string issued_at { get => _issued_at; set => _issued_at = value; }

        /*
            Error               Error Description

            error               —Error code
            error_description   —Description of the error with additional information
            invalid_client_id   —Invalid client identifier
            invalid_request     —refresh_token scope is required. Install and preauthorize the app.
            invalid_app_acess   —User isn’t approved by an admin to access this app
            invalid_grant       —User hasn’t approved this consumer
            invalid_grant       —Invalid assertion
            invalid_grant       —Invalid audience
            invalid_grant       —Authentication failure

        */
        /// <summary>
        /// Any error that may be returned:
        /// See possible values above
        /// </summary>
        public string error { get => _error; set => _error = value; }
        /// <summary>
        ///  See possible values above
        /// </summary>
        public string error_description { get => _error_description; set => _error_description = value; }


        /// <summary>
        /// Returns the error state of the current operation.
        /// If there are errors in the Response, the rest of the property values will be null or empty except for the error properties.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                bool returnValue = false;

                returnValue = string.IsNullOrEmpty(error) ? false : true;

                return returnValue;
            }

        }


    }
}
