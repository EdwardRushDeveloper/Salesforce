using System;
namespace SimpleSalesforce
{
    public class RefreshToken
    {
        public RefreshToken()
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
