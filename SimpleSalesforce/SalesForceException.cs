using System;
namespace SimpleSalesforce
{
    /// <summary>
    /// Salesforce specific Exception
    ///If a problem occurs during this step, the response contains an error message with the following information.
    /// </summary>
    public class SalesForceException : Exception
    {

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
        public SalesForceException(string error, string errorDescription) : base($"Salesforce Error {error}: Description {errorDescription}")
        {
            _error = error;
            _errorDescription = errorDescription;

            
        }

        string _error;
        string _errorDescription;

        public string Error { get => _error; }
        public string ErrorDescription { get => _errorDescription;}
    }
}
