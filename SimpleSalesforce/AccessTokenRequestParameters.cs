using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleSalesforce
{
    public class AccessTokenRequestParameters
    {
        /// <summary>
        ///  Salesforce Reference
        ///  https://help.salesforce.com/articleView?id=remoteaccess_oauth_user_agent_flow.htm&type=5
        ///  https://help.salesforce.com/articleView?id=remoteaccess_oauth_scopes.htm&type=5
        ///
        ///  Salesforce Following Open Id Standards:
        ///  https://openid.net/specs/openid-connect-basic-1_0-28.html
        /// 
        /// Example of a Request this is Generate
        ///https://mypkscommunity-developer-edition.na102.force.com/NTOCustomers/services/oauth2/authorize
        ///?
        ///response_type=token+id_token
        ///client_id=[clientid]
        ///redirect_uri=finsmaaa://sflogin
        ///state=FINSM
        ///nonce=1234
        ///scope=openid refresh_token
        /// </summary>
        /// 
        /// <param name="response_Type">Required for the Request</param>
        /// <param name="clientId">Required for the Request</param>
        /// <param name="redirectUri">Required for the Request</param>
        [JsonConstructor()]
        public AccessTokenRequestParameters(string Uri,List<Response_Type> response_Type, string clientId, string redirectUri)
        {
            _uri = Uri;
            _responseType = response_Type;
            _clientId = clientId;
            _redirectUri = redirectUri;

            Prompt = Prompt.NotSet;
            Display = Display.NotSet;


        }

        string _uri;
        List<Response_Type> _responseType;
        string _clientId;
        string _redirectUri;
        string _state;
        List<Scope> _scope;
        Display _display;
        string _loginHint;
        string _nonce;
        Prompt _prompt;

        /// <summary>
        /// The Authorization URI to use to generate the Request String
        /// </summary>
        public string Uri { get => _uri; set => _uri = value; }
        /// <summary>
        /// Value can be token, or token id_token with the scope parameter openid and a nonce parameter.
        /// If you specify token id_token, Salesforce returns an ID token in the response. 
        /// </summary>
        [JsonProperty("response_type")]
        public List<Response_Type> ResponseType { get => _responseType; private set => _responseType = value; }
        /// <summary>
        /// Consumer key from the connected app definition.
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get => _clientId; private set => _clientId = value; }
        /// <summary>
        /// URI to redirect the user after approval.
        /// The URI must match one of the values in the Callback URL field in the connected app definition.
        /// This value must be URL encoded.
        ///
        /// This value is SET as a Decoded URI, but during final string creation, the URI is encoded.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get => _redirectUri; private set => _redirectUri = value; }
        /// <summary>
        /// (Optional) Any state that the consumer wants returned in the callback after approval.
        /// </summary>
        [JsonProperty("state")]
        public string State { get => _state; set => _state = value; }
        /// <summary>
        /// A space-separated list of scope values. The scope parameter fine-tunes the permissions associated with the tokens that you’re requesting.
        /// Scope is a subset of values that you specified when defining the connected app. 
        /// </summary>
        [JsonProperty("scope")]
        public List<Scope> Scope { get => _scope; set => _scope = value; }
        /// <summary>
        /// (Optional) Changes the login page’s display type.
        /// page  — Full-page authorization screen(default)
        /// popup — Compact dialog optimized for modern web browser popup windows
        /// touch — Mobile-optimized dialog designed for modern smartphones, such as Android and iPhone
        /// </summary>
        [JsonProperty("display")]
        public Display Display { get => _display; set => _display = value; }
        /// <summary>
        /// Provide a valid username value with this parameter to pre-populate the login page with the username.
        /// For example: login_hint=username@company.com.
        /// If a user already has an active session in the browser, the login_hint parameter does nothing. The active user session continues.
        /// </summary>
        [JsonProperty("login_hint")]
        public string LoginHint { get => _loginHint; set => _loginHint = value; }
        /// <summary>
        /// Required with the openid scope for getting a user ID token.
        /// The value is returned in the response and useful for detecting “replay” attacks.
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get => _nonce; set => _nonce = value; }
        /// <summary>
        ///(Optional) Specifies how the authorization server prompts the user for reauthentication and reapproval.Salesforce supports these values.
        ///login   — The authorization server must prompt the user for reauthentication, forcing the user to log in again.
        ///consent — The authorization server must prompt the user for reapproval before returning information to the client.
        ///select_account — If presented, take one of the following actions.
        ///                If zero or one hint is available and the user is logged in, show the approval page without prompting for login.
        ///                If zero or one hint is available and the user isn’t logged in, prompt for login.
        ///                If more than one hint is available, show the account chooser.
        ///You can pass login and consent values, separated by a space, to require the user to log in and reauthorize.
        ///For example: ?prompt=login%20consent
        /// </summary>
        [JsonProperty("prompt")]
        public Prompt Prompt { get => _prompt; set => _prompt = value; }
  

        /// <summary>
        /// Combines all the parameters that are not null or empty into a Query Parameter Formated string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnValue = string.Empty;
            string qp = "{0}={1}";
            StringBuilder builder = new StringBuilder();

            //Uri
            builder.Append(Uri);

            List<string> rt = (from i in ResponseType select i.ToString()).ToList();

            string rts = string.Join("+", rt.ToArray());

            /*
                    Parameters.
                    • response_type
                    • client_id
                    • redirect_uri
                    • state —(Optional) 
                    • scope —
                    • display—(Optional) 
                    • login_hint
                    • nonce 
                    • prompt—(Optional) 
            */
            builder.Append("?");
            builder.Append(string.Format(qp, "response_type", rts));
            builder.Append("&");
            builder.Append(string.Format(qp, "client_id", ClientId));
            builder.Append("&");
            builder.Append(string.Format(qp, "redirect_uri",RedirectUri));

            if(!string.IsNullOrEmpty(State))
            {
                builder.Append("&");
                builder.Append(string.Format(qp, "state", State));
            }

            if(Display != Display.NotSet)
            {
                builder.Append("&");
                builder.Append(string.Format(qp, "display", Display.ToString()));
            }

            if(Prompt != Prompt.NotSet)
            {
                builder.Append("&");
                builder.Append(string.Format(qp, "prompt", Prompt.ToString()));
            }

            if (!string.IsNullOrEmpty(LoginHint))
            {
                builder.Append("&");
                builder.Append(string.Format(qp, "login_hint", LoginHint));
            }

            if (!string.IsNullOrEmpty(Nonce))
            {
                builder.Append("&");
                builder.Append(string.Format(qp, "nonce", Nonce));
            }


            if (Scope != null && Scope.Count > 0)
            {
                List<string> sc = (from i in Scope select i.ToString()).ToList();
                string scs = string.Join("%20", sc.ToArray());

                builder.Append("&");
                builder.Append(string.Format(qp, "scope", scs));
            }

            returnValue = builder.ToString();

            return returnValue;
        }

    }

    /// <summary>
    /// (Optional) Changes the login page’s display type.
    /// page  — Full-page authorization screen(default)
    /// popup — Compact dialog optimized for modern web browser popup windows
    /// touch — Mobile-optimized dialog designed for modern smartphones, such as Android and iPhone
    /// </summary>
    public enum Display
    {
        NotSet = 0, page = 1, popup = 2, touch = 3

    }
    /// <summary>
    /// Value can be token, or token id_token with the scope parameter openid and a nonce parameter.
    /// If you specify token id_token, Salesforce returns an ID token in the response. 
    /// </summary>
    public enum Response_Type
    {
        token = 0, id_token = 1
    }
    /// <summary>
    /// A space-separated list of scope values. The scope parameter fine-tunes the permissions associated with the tokens that you’re requesting.
    /// Scope is a subset of values that you specified when defining the connected app. 
    /// </summary>
    public enum Scope
    {
              api = 0
            , chatter_api = 1
            , custom_permissions = 2
            , full = 3
            , id = 4
            , openid = 5
            , refresh_token = 6
            , visualforce = 7
            , web = 8
    }

    /// <summary>
    ///(Optional) Specifies how the authorization server prompts the user for reauthentication and reapproval.Salesforce supports these values.
    ///login   — The authorization server must prompt the user for reauthentication, forcing the user to log in again.
    ///consent — The authorization server must prompt the user for reapproval before returning information to the client.
    ///select_account — If presented, take one of the following actions.
    ///                If zero or one hint is available and the user is logged in, show the approval page without prompting for login.
    ///                If zero or one hint is available and the user isn’t logged in, prompt for login.
    ///                If more than one hint is available, show the account chooser.
    ///You can pass login and consent values, separated by a space, to require the user to log in and reauthorize.
    ///For example: ?prompt=login%20consent
    /// </summary>
    public enum Prompt
    {
          NotSet = 0
        , login = 1
        , consent = 2
        , select_account = 3
    }

}
