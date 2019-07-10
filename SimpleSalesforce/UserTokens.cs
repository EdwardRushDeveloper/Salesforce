using System;
namespace SimpleSalesforce
{
    /// <summary>
    /// The collection of tokens used for Salesforce access and integration.
    /// </summary>
    public class UserTokens
    {
        public UserTokens()
        {
            //default the date and time for later access
            //during deserialization, this will be overwritten.
            CreateDateTime = DateTime.Now;
        }


        RefreshToken _refreshToken;
        AccessToken _accessToken;
        IdToken _idToken;
        DateTime _createDateTime;

        /// <summary>
        /// The Refresh Token used to refresh both the AccessToken, and the IdToken
        /// </summary>
        public RefreshToken RefreshToken { get => _refreshToken; set => _refreshToken = value; }
        /// <summary>
        /// The Access Token used for updates and queries of the Saleforce instance
        /// </summary>
        public AccessToken AccessToken { get => _accessToken; set => _accessToken = value; }
        /// <summary>
        /// The User Information Token.
        /// </summary>
        public IdToken IdToken { get => _idToken; set => _idToken = value; }
        /// <summary>
        /// The Date and Time this instance was created.
        /// </summary>
        public DateTime CreateDateTime { get => _createDateTime; set => _createDateTime = value; }
    }
}
