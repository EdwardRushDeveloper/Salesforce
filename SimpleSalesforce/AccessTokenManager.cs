using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace SimpleSalesforce
{

    /// <summary>
    /// The Request Callback Values returned from the Salesforce Token Request.
    /// This class will create the Regular Expression operations to parse the return GET URL
    /// and validate that it conforms to the Schema specified, and contains all the necessary parameters 
    /// </summary>
    public class AccessTokenResponseManager
    {

        /// <summary>
        /// The default Regular Expression
        /// </summary>
        public const string REGULAR_EXPRESSION  = @"(?<SchemaName>finsmaaa:\/\/sflogin)|(?:(?:(?:(?:#|&)\b(?<PropertyName>access_token|refresh_token|instance_url|id|issued_at|signature|id_token|state|scope|token_type)\b))=(?<PropertyValue>[^& \n]+?)(?=$|&))";

        public const int PROPERTY_COUNT         = 11;
        /// <summary>
        /// The default Custom URI
        /// </summary>
        public const string CUSTOM_SCHEMA       = @"finsmaaa:\/\/sflogin";
        /*
         
            Custom URI [finsmaaa://sflogin]
            access_token
            refresh_token
            instance_url
            id
            issued_at
            signature
            sfdc_community_url
            sfdc_community_id
            id_token
            state
            scope
            token_type
       */
        private AccessTokenResponseManager()
        {


        }

        /// <summary>
        /// Constructor to handle the Raw Data being returned from the SalesForce Callback, and the
        /// Regular Expression To use for the Parsing. If no Regular Expression is passed in, the Constant in this class is used instead.
        /// </summary>
        /// <param name="regularExpression">The regular expression to use if the default is not wanted</param>
        /// <param name="customSchema">The custom schema if the default is not wanted.</param>
        public AccessTokenResponseManager(string regularExpression = "", string customSchema = "", int propertyCount = 0)
        {

            _regularExpression = string.IsNullOrEmpty(regularExpression) ? REGULAR_EXPRESSION : regularExpression;
            _customSchema = string.IsNullOrEmpty(customSchema) ? CUSTOM_SCHEMA : customSchema;
            _propertyCount = propertyCount == 0 ? PROPERTY_COUNT : propertyCount;

            _regex = new Regex(_regularExpression, RegexOptions.CultureInvariant);

            //add a start line regular expression character to the customer schema
            _regexSchema = new Regex($"^{_customSchema}", RegexOptions.CultureInvariant);

        }

        #region Private Variables


        /// <summary>
        /// The regular expression engine used for parsing the raw callback data.
        /// </summary>
        Regex _regex;
        Regex _regexSchema;
        int _propertyCount;

        string _rawCallback;
        string _regularExpression;
        string _customSchema;

        string _access_token;
        string _refresh_token;
        string _instanceUrl;
        string _id;
        string _issuedAt;
        string _signature;
        string _sfdcCommunityUrl;
        string _sfdcCommunityId;
        string _idToken;
        string _state;
        string _scope;
        string _tokenType;

        IdToken _IdToken;
        AccessToken _accessToken;
        RefreshToken _refreshToken;

        #endregion


        #region public properties


        /// <summary>
        /// Salesforce session ID that can be used with the web services API.
        /// </summary>
        [JsonProperty("access_token")]
        public string access_token { get => _access_token; set => _access_token = value; }
        /// <summary>
        /// Token that can be used in the future to obtain new access tokens (sessions).
        /// This value is a secret. Treat it like the user’s password, and use appropriate measures to protect it.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string refresh_token { get => _refresh_token; set => _refresh_token = value; }
        /// <summary>
        /// A URL indicating the instance of the user’s org. For example: https://yourInstance.salesforce.com/.
        /// </summary>
        [JsonProperty("instance_url")]
        public string instance_url { get => _instanceUrl; set => _instanceUrl = value; }
        /// <summary>
        /// Identity URL that can be used to both identify the user and query for more information about the user. See Identity URLs.
        /// </summary>
        [JsonProperty("id")]
        public string id { get => _id; set => _id = value; }
        /// <summary>
        /// When the signature was created.
        /// </summary>
        [JsonProperty("issued_at")]
        public string issued_at { get => _issuedAt; set => _issuedAt = value; }
        /// <summary>
        /// Base64-encoded HMAC-SHA256 signature signed with the client_secret (private key) containing the concatenated ID and issued_at.
        /// Used to verify that the identity URL hasn’t changed since the server sent it.
        /// </summary>
        [JsonProperty("signature")]
        public string signature { get => _signature; set => _signature = value; }
        /// <summary>
        /// If the user is a member of a Salesforce community, the community URL is provided.
        /// </summary>
        [JsonProperty("sfdc_community_url")]
        public string sfdc_community_url { get => _sfdcCommunityUrl; set => _sfdcCommunityUrl = value; }
        /// <summary>
        /// If the user is a member of a Salesforce community, the user’s community ID is provided.
        /// </summary>
        [JsonProperty("sfdc_community_id")]
        public string sfdc_community_id { get => _sfdcCommunityId; set => _sfdcCommunityId = value; }
        /// <summary>
        /// Salesforce value conforming to the OpenID Connect specifications.
        /// The token is returned only if the response_type is token id_token with the scope parameter openid and a nonce parameter.
        /// </summary>
        [JsonProperty("id_token")]
        public string id_token { get => _idToken; set => _idToken = value; }
        /// <summary>
        /// The oiginal State string passed with the original Request
        /// </summary>
        [JsonProperty("state")]
        public string state { get => _state; set => _state = value; }
        /// <summary>
        /// The Scope string passed with the original Request
        /// </summary>
        [JsonProperty("response_type")]
        public string scope { get => _scope; set => _scope = value; }
        /// <summary>
        /// Value is Bearer for all responses that include an access token.
        /// </summary>
        [JsonProperty("token_type")]
        public string token_type { get => _tokenType; set => _tokenType = value; }
        /// <summary>
        /// The Raw Callback Return string.
        /// </summary>
        [JsonProperty("RawCallback")]
        public string RawCallback { get => _rawCallback; set => _rawCallback = value; }
        /// <summary>
        /// The regular expression that is used to parse the incoming raw callback data.
        /// </summary>
        [JsonProperty("RegularExpression")]
        public string RegularExpression { get => _regularExpression; set => _regularExpression = value; }
        /// <summary>
        /// The custom uri schema that is used for a precheck validation of the incoming raw data.
        /// </summary>
        [JsonProperty("CustomSchema")]
        public string CustomSchema { get => _customSchema; set => _customSchema = value; }
        /// <summary>
        /// The number of Properties/Matches that should be found from this expression.
        /// </summary>
        [JsonProperty("PropertyCount")]
        public int PropertyCount { get => _propertyCount; set => _propertyCount = value; }

        /// <summary>
        /// The IdToken Instance containing the parsed values.
        /// </summary>
        [JsonProperty("IdToken")]
        public IdToken IdToken { get => _IdToken; set => _IdToken = value; }

        /// <summary>
        /// The Refresh Token Object used to Refresh the IdToken and the AccessToken
        /// </summary>
        public RefreshToken RefreshToken { get => _refreshToken; set => _refreshToken = value; }
        /// <summary>
        /// The Access Token used to query and update Salesforce.
        /// </summary>
        public AccessToken AccessToken { get => _accessToken; set => _accessToken = value; }


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

        #endregion

        #region public methods

        /// <summary>
        /// Validates that the incoming Raw Data is a return we can handle.
        /// </summary>
        /// <param name="rawCallback"></param>
        /// <returns></returns>
        public bool Validate(string rawCallback)
        {
            bool returnValue = false;

            /*
                Custom URI [finsmaaa://sflogin]
                access_token
                refresh_token
                instance_url
                id
                issued_at
                signature
                sfdc_community_url
                sfdc_community_id
                id_token
                state
                scope
                token_type
            */

            /*

            > matches[0].Groups
                Count = 4
	                [0]: {#access_token=00Dj00000028gCu%21ARYAQPKfYziAtCjk2Ycvs1wE2KCtOJseiQufoCZjI3p8OQBFRZI_ytKHg3PZbfaw4q1cKckeIyfxXlblUOJKSaLxtKpmQvEf}
	                [1]: {}
	                [2]: {access_token}
	                [3]: {00Dj00000028gCu%21ARYAQPKfYziAtCjk2Ycvs1wE2KCtOJseiQufoCZjI3p8OQBFRZI_ytKHg3PZbfaw4q1cKckeIyfxXlblUOJKSaLxtKpmQvEf}
	                Evaluation failed.
                > matches[0].Groups[2].Name
                "PropertyName"
                > matches[0].Groups[2].Value
                "access_token"
                > matches[0].Groups[3].Value
                "00Dj00000028gCu%21ARYAQPKfYziAtCjk2Ycvs1wE2KCtOJseiQufoCZjI3p8OQBFRZI_ytKHg3PZbfaw4q1cKckeIyfxXlblUOJKSaLxtKpmQvEf"
                > matches[0].Groups[3].Name
                "PropertyValue"
> 






            */

            Dictionary<string, string> requestProperties = new Dictionary<string, string>();

            //validate that the current custom schema/uri is present
            //Example: finsmaaa://sflogin
            Match match = _regexSchema.Match(rawCallback);


            //when we have an initial match for our Custom Schema



            /*

                > requestProperties
                Count = 11
	            [0]: {[SchemaName, finsmaaa://sflogin]}
	            [1]: {[access_token, 00Dj00000028gCu%21ARYAQPKfYziAtCjk2Ycvs1wE2KCtOJseiQufoCZjI3p8OQBFRZI_ytKHg3PZbfaw4q1cKckeIyfxXlblUOJKSaLxtKpmQvEf]}
	            [2]: {[refresh_token, 5Aep861E3ECfhV22nYuzbKoDWYBlo3oTokN1ATS_.dGHyLC5GGMTkXjKFtThUIi3sVuy724DNCb3JDbiAymUtqb]}
	            [3]: {[instance_url, https%3A%2F%2Fpksf-dev-ed.my.salesforce.com]}
	            [4]: {[id, https%3A%2F%2Flogin.salesforce.com%2Fid%2F00Dj0000å0028gCuEAI%2F0053Z00000KPlpVQAT]}
	            [5]: {[issued_at, 1561384497680]}
	            [6]: {[signature, ed1%2F6R2Z%2BtBHwLdlXuUcb3SD0yTKkDdV0J6NL%2FnlpIM%3D]}
	            [7]: {[id_token, eyJraWQiOiIyMjAiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdF9oYXNoIjoibVRmajh1UzY0MzVyRjA3dmhSLVEzQSIsInN1YiI6Imh0dHBzOi8vbG9naW4uc2FsZXNmb3JjZS5jb20vaWQvMDBEajAwMDAwMDI4Z0N1RUFJLzAwNTNaMDAwMDBLUGxwVlFBVCIsInpvbmVpbmZvIjoiR01UIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImFkZHJlc3MiOnt9LCJwcm9maWxlIjoiaHR0cHM6Ly9wa3NmLWRldi1lZC5teS5zYWxlc2ZvcmNlLmNvbS8wMDUzWjAwMDAwS1BscFZRQVQiLCJpc3MiOiJodHRwczovL215cGtzY29tbXVuaXR5LWRldmVsb3Blci1lZGl0aW9uLm5hMTAyLmZvcmNlLmNvbS9OVE9DdXN0b21lcnMiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJleHRlcm5hbHVzZXIxNTYxMTM0MTU0NTIyQGNvbXBhbnkuY29tIiwiZ2l2ZW5fbmFtZSI6IkVkd2FyZCIsImxvY2FsZSI6ImVuX1VTIiwibm9uY2UiOiIxMjM0IiwicGljdHVyZSI6Imh0dHBzOi8vbXlwa3Njb21tdW5pdHktZGV2ZWxvcGVyLWVkaXRpb24ubmExMDIuZm9yY2UuY29tL2ltZy91c2VycHJvZmlsZS9kZWZhdWx0X3Byb2ZpbGVfMjAwX3YyLnBuZyIsImF1ZCI6IjNNVkc5Zk10Q2tWNmVMaGNPTm05cUkxYjhNUXpDaHhBZ0doQXh0VmhrcGk2S2RNcXdPWUMwUzJNRjJQaUFMNEZHcmZuSHlJLnNIZDJkWnRwcDhkUlYsaHR0cHM6Ly9maW5zbS5hYWEuY29tIiwidXBkYXRlZF9hdCI6IjIwMTktMDYtMjFUMTY6MjI6MzRaIiwibmlja25hbWUiOiJFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwibmFtZSI6IkVkd2FyZCBFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwicGhvbmVfbnVtYmVyIjpudWxsLCJleHAiOjE1NjEzODQ2MTcsImlhdCI6MTU2MTM4NDQ5NywiZmFtaWx5X25hbWUiOiJFeHRlcm5hbFVzZXIxNTYxMTM0MTU0NTIyIiwiZW1haWwiOiJlZHJ1c2hAYWFhY2Fyb2xpbmFzLmNvbSJ9.qNm5XwQYEkzLWKX2jUhvcYsqJ_S8SM29lR2m80Q6pIT9Ge25hvovqp6HS_l4UAQ2e2zPHWZoB1PuqskdrQYLEK3uHSmR5U2HqtYVNCMwOCReYoMyqNJFNRiF3_zqBPcT-GFGEGd7SyrVhERCyrw9aG5-nM14sU35d9Y4e4H0GehjBo3Iki12DcLsZGwlmpsm5_6MCFK8V4kH0fMWSALKAXoSWwKDvqrJ4jJ2EptRnTHCB3YoOYqy2-tQCVruHP4sAP75J-D6kTTxmtzNa_Ka9AGx0FUEEpYqJqgipZYa9_U_M9MIaO-8UPPUkSKeHANhlHISn77WMsr2Vv9z1HghIqpDjj1LUZ02J8ksx2QdT3zlJzGb35N-0xq_qvDfYL8zYfndJjmI0eH7WSZUWc_5cngFOMxy0pkmeiROAIFyj7_v_i5_75QXTvKfcPYLZHxOB7DDJkqaYkosdsmSPCtLK0PkowMXM5Hr8Lhc5DD4dzBnTrzDxYppUpMqO_fCI71NHXR0SYaaxqZI4Aiiult27wN1oIkJSP8M6hAyLG1TYEHuEXVRbT2fMTJUPW6MC8wLzFSrQxbgdo30UxrBUTccZOxhE-WYSMcB46aU2yplX8Y89s_OlKjEgKskxcUnzftTOAvdf9i9_gc9GwPPAsL4q-pHvvOX5SmwPPXdJOzV6LU]}
	            [8]: {[state, FINSM]}
	            [9]: {[scope, refresh_token+openid]}
	            [10]: {[token_type, Bearer]}

	            Raw View: 

            */

            //if our initial validation is successful
            if (match.Success)
            {
                //*NOTE - unable to use LINQ on these objects. They do not implment IEnumerable<T>
                MatchCollection matches = _regex.Matches(rawCallback);

                //check our counts on matches. we should have a total of 11. See the example above.
                int matchCount = matches.Count;

                //once the SchemaName value has been pulled, we dont want to continously check
                //for this. so we only check once, grab the value and then set a flag that is was completed.
                bool schemaCollected = false;

                //refer to the documentation above, we should have 11 matches.
                if (matchCount == 11)
                {
                    foreach (Match current in matches)
                    {

                        //the schema name is treated differently in the
                        //regular expression, and will not have two groups associated to
                        //each match.
                        //There will be a match, and then 1 group called SchemaName
                        if (!schemaCollected)
                        {
                            Group schemaGroup = current.Groups["SchemaName"];
                            if (schemaGroup.Success)
                            {
                                requestProperties.Add("SchemaName", schemaGroup.Value);
                                schemaCollected = true;
                            }
                        }
                        else
                        {
                            Group propertyNameGroup  = current.Groups["PropertyName"];
                            Group propertyValueGroup = current.Groups["PropertyValue"];

                            if (propertyNameGroup.Success && propertyValueGroup.Success)
                            {
                                requestProperties.Add(propertyNameGroup.Value, propertyValueGroup.Value);
                            }
                        }

                    }

                    //now using the dictionary, we need to reflect and set our properties.
                    if(requestProperties.Count == 11)
                    {
                        //get all of the keys....except for the SchemaName. 
                        List<string> keys = requestProperties.Keys.Where(x => x != "SchemaName").ToList();

                        foreach (string current in keys)
                        {
                            string itemValue = requestProperties[current];
                            GetType().GetProperty(current, BindingFlags.Public | BindingFlags.Instance).SetValue(this, itemValue, null);

                        }

                       _rawCallback = rawCallback;


                        _IdToken      = IdToken.GetIdToken(this._idToken);
                        _accessToken  = new AccessToken() { TokenValue = access_token };
                        _refreshToken = new RefreshToken() { TokenValue = refresh_token };

                        returnValue = true;
                    }

                }

            }

            return returnValue;
        }


        #endregion
    }
}
