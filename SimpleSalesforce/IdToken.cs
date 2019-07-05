using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SimpleSalesforce
{
    public class IdToken
    {

        public const string REGULAR_EXPRESSION = "(?:(?<=(?:^))|(?<=(?:[.])))(?<TokenItem>[^ \n.]*)";

        string _at_hash;
        string _sub;
        string _zoneinfo;
        bool _email_verified;
        string _profile;
        string _iss;
        string _preferred_username;
        string _given_name;
        string _locale;
        string _nonce;
        string _picture;
        string _aud;
        string _updated_at;
        string _nickname;
        string _name;
        string _phone_number;
        int _exp;
        int _iat;
        string _family_name;
        string _email;


        string _idToken;

       
        /// <summary>
        /// Accepts an IdToken string, and extracts the body of the message, parses it into an Id Token Item.
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public static IdToken GetIdToken(string idToken)
        {
            IdToken returnItem = null;
            Regex regexTokenItems = new Regex(REGULAR_EXPRESSION, RegexOptions.CultureInvariant);

            MatchCollection elements = regexTokenItems.Matches(idToken);

            //if the element count is equal to 3, we possibly have an id token.
            //which will be the 2nd item in the match collection
            if(elements.Count == 3)
            {
                //the match groups are named TokenItem
                string ts = elements[1].Groups["TokenItem"].Value;

                ///due to base64 types between what is standard and used in openid, add = to the end of the string
                string paddedString = ts.PadRight(ts.Length + (ts.Length * 3) % 4, '=');

                //now the string can be converted.
                string json = Encoding.UTF8.GetString(Convert.FromBase64String(paddedString));

                returnItem = JsonConvert.DeserializeObject<IdToken>(json);

            }

            return returnItem;
        }

        /// <summary>
        /// https://openid.net/specs/openid-connect-basic-1_0-28.html
        /// 
        /// </summary>
        public IdToken()
        {
         
        }


        /// <summary>
        /// 
        /// </summary>
        public string at_hash {get => _at_hash; set => _at_hash = value; }
        /// <summary>
        /// REQUIRED. Subject identifier. A locally unique and never reassigned identifier within the Issuer for the End-User, which is intended to be consumed by the
        /// Client, e.g., 24400320 or AItOawmwtWwcT0k51BayewNvutrJUqsvl6qs7A4. It MUST NOT exceed 255 ASCII characters in length. The sub value is a case sensitive string.
        /// </summary>
        public string sub { get => _sub; set => _sub = value; }
        /// <summary>
        /// 
        /// </summary>
        public string zoneinfo { get => _zoneinfo; set => _zoneinfo = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool email_verified { get => _email_verified; set => _email_verified = value; }

        /// <summary>
        /// excluding address
        /// </summary>
        //public Address address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string profile { get => _profile; set => _profile = value; }
        /// <summary>
        /// REQUIRED. Issuer Identifier for the Issuer of the response. The iss value is a case sensitive URL using the https scheme that contains
        /// scheme, host, and OPTIONALLY, port number and path components and no query or fragment components.
        /// </summary>
        public string iss { get => _iss; set => _iss = value; }
        /// <summary>
        /// 
        /// </summary>
        public string preferred_username { get => _preferred_username; set => _preferred_username = value; }
        /// <summary>
        /// 
        /// </summary>
        public string given_name { get => _given_name; set => _given_name = value; }
        /// <summary>
        /// 
        /// </summary>
        public string locale { get => _locale; set => _locale = value; }

        /// <summary>
        /// OPTIONAL. String value used to associate a Client session with an ID Token, and to mitigate replay attacks.
        /// The value is passed through unmodified from the Authorization Request to the ID Token.
        /// The Client MUST verify that the nonce Claim Value is equal to the value of the nonce parameter sent in the Authorization Request.
        /// If present in the Authorization Request, Authorization Servers MUST include a nonce Claim in the ID Token with the Claim Value being the nonce value sent in the Authorization Request.
        /// Use of the nonce is OPTIONAL when using the code flow. The nonce value is a case sensitive string.
        /// </summary>
        public string nonce { get => _nonce; set => _nonce = value; }
        /// <summary>
        /// 
        /// </summary>
        public string picture { get => _picture; set => _picture = value; }
        /// <summary>
        /// REQUIRED. Audience(s) that this ID Token is intended for. It MUST contain the OAuth 2.0 client_id of the Relying Party as an audience value.
        /// It MAY also contain identifiers for other audiences. In the general case, the aud value is an array of case sensitive strings.
        /// In the special case when there is one audience, the aud value MAY be a single case sensitive string.
        /// </summary>
        public string aud { get => _aud; set => _aud = value; }
        /// <summary>
        /// 
        /// </summary>
        public string updated_at { get => _updated_at; set => _updated_at = value; }
        /// <summary>
        /// 
        /// </summary>
        public string nickname { get => _nickname; set => _nickname = value; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get => _name; set => _name = value; }
        /// <summary>
        /// 
        /// </summary>
        public string phone_number { get => _phone_number; set => _phone_number = value; }
        /// <summary>
        /// REQUIRED. Expiration time on or after which the ID Token MUST NOT be accepted for processing.
        /// The processing of this parameter requires that the current date/time MUST be before the expiration date/time listed in the value. Implementers MAY provide for some small leeway, usually no more than a few minutes, to account for clock skew. The time is represented as the number of seconds from 1970-01-01T0:0:0Z as measured in UTC until the date/time.
        /// See RFC 3339 [RFC3339] for details regarding date/times in general and UTC in particular. The exp value is a number.
        /// </summary>
        public int exp { get => _exp; set => _exp = value; }
        /// <summary>
        /// REQUIRED. Time at which the JWT was issued. The time is represented as the number of seconds from 1970-01-01T0:0:0Z as measured in UTC until the date/time.
        /// The iat value is a number.
        /// </summary>
        public int iat { get => _iat; set => _iat = value; }
        /// <summary>
        /// 
        /// </summary>
        public string family_name { get => _family_name; set => _family_name = value; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get => _email; set => _email = value; }
    }
}
