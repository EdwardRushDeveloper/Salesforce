using System;
namespace SimpleSalesforce
{
    /// <summary>
    /// The access token instance that will hold the AccessToken used for
    /// direct calls to Salesforce for user update or query operations.
    ///
    /// The life of this token is limited and must be refereshed using the refresh token.
    /// </summary>
    public class AccessToken
    {
        
        public AccessToken()
        {
            _createDateTime = DateTime.Now;
        }


        string _tokenValue;
        DateTime _createDateTime;

        /// <summary>
        /// The current access token
        /// </summary>
        public string TokenValue { get => _tokenValue; set => _tokenValue = value; }
        /// <summary>
        /// The create date and time of the access token
        /// </summary>
        public DateTime CreateDateTime { get => _createDateTime; set => _createDateTime = value; }
    }
}
