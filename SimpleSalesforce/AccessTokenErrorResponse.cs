using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleSalesforce
{
    /// <summary>
    /// Class used to determine if any errors are returned from a Salesforce Call
    /// </summary>
    public class AccessTokenErrorResponse
    {


        /// <summary>
        /// Returns Matchs [error=redirect_uri_mismatch&amp;error_description=]
        /// </summary>
        public const string ERR0R_CHECK_EXPRESSION = "(?:(?<Errors>(?:(?:error=)[^ \n].*?(?:&amp;|&))(?:error_description=)))";
        /// <summary>
        /// Returns Matches 
        /// </summary>
        public const string ERROR_PARSE_EXPRESSION = @"(?:(?<Error>error)=(?<ErrorValue>[^ \n]*)(?=(?:&amp;|&))(?:&amp;|&))(?<ErrorDescription>error_description)=(?<ErrorDescriptionValue>\b[^ \n].*\b(?=(?:<\/|\\u003C)))";

        /// <summary>
        /// Javascript that is used to inject in the WebView to pull the HTML from the page and check for the existence of errors
        /// that are sent back by Salesforce and written to an html page.
        /// </summary>
        public const string ERROR_CHECKING_JAVASCRIPT = "var controlReturn=function(){return document.documentElement.innerHTML};controlReturn();";
        /*

            Test Text Returned from call to salesforce
            This Text is written to a page and not to a URL.
            The expression defined:
                ERR0R_CHECK_EXPRESSION: checks for any number of matches
                ERROR_PARSE_EXPRESSION: will break the values down into parameters.

            \u003Chead>\u003C/head>\u003Cbody>\u003Cpre style=\"word-wrap: break-word; white-space: pre-wrap;\">error=redirect_uri_mismatch&amp;error_description=redirect_uri%20must%20match%20configuration\u003C/pre>\u003C/body>

        */
        public AccessTokenErrorResponse()
        {

            _regexErrorCheck = new Regex(ERR0R_CHECK_EXPRESSION);
            _regexErrorParse = new Regex(ERROR_PARSE_EXPRESSION);

            _errors = new List<ErrorEntity>();
        }


        Regex _regexErrorCheck;
        Regex _regexErrorParse;
        List<ErrorEntity> _errors;

     
        /// <summary>
        /// Validates if the incoming Text contains errors
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool Validate(string content)
        {
            bool returnValue = false;

            Match errorMatch = _regexErrorCheck.Match(content);

            if(errorMatch.Success)
            {
                MatchCollection matches = _regexErrorParse.Matches(content);

                foreach(Match current in matches)
                {
                    Group ErrorValue            = current.Groups["ErrorValue"];
                    Group ErrorDescriptionValue = current.Groups["ErrorDescriptionValue"];


                    ErrorEntity errorEntity = new ErrorEntity() { ErrorValue = ErrorValue.Value, ErrorDescriptionValue = ErrorDescriptionValue.Value };

                    _errors.Add(errorEntity);

                    returnValue = true;
                }
            }

            return returnValue;
        }
    }

    /// <summary>
    /// Error Entity as returned by SalesForce
    ///         Example Return \u003Chead>\u003C/head>\u003Cbody>\u003Cpre style=\"word-wrap: break-word; white-space: pre-wrap;\">error=redirect_uri_mismatch&amp;error_description=redirect_uri%20must%20match%20configuration\u003C/pre>\u003C/body>
    ///
    /// Documentation from SalesForce Site
    /// https://help.salesforce.com/articleView?id=remoteaccess_oauth_user_agent_flow.htm&type=5
    ///If access is denied or an error occurs during this step, the user is redirected to the redirect_uri. An error code and the description of the error are included in the URI after the hash tag (#). This string isn’t a query.
    ///error—Error code
    ///error_description—Description of the error with additional information
    ///unsupported_response_type—Response type not supported
    ///invalid_client_id—Client identifier invalid
    ///invalid_request—HTTPS required
    ///invalid_request—Must use HTTP GET
    ///invalid_request—Out-of-band not supported
    ///access_denied—User denied authorization
    ///redirect_uri_missing—Redirect_uri not provided
    ///redirect_uri_mismatch—Redirect_uri mismatch with connected app object
    ///immediate_unsuccessful—Immediate unsuccessful
    ///invalid_grant—Invalid user credentials
    ///invalid_grant—IP restricted or invalid login hours
    ///inactive_user—User is inactive
    ///inactive_org—Org is locked, closed, or suspended
    ///rate_limit_exceeded—Number of logins exceeded
    ///invalid_scope—Requested scope is invalid, unknown, or malformed
    ///state—State that was passed into the approval step.This value is included only if the state parameter is included in the original query string.
    /// </summary>
    public class ErrorEntity
    {

        string _errorValue;
        string _errorDescriptionValue;

        public string ErrorValue { get => _errorValue; set => _errorValue = value; }
        public string ErrorDescriptionValue { get => _errorDescriptionValue; set => _errorDescriptionValue = value; }
    }


}
