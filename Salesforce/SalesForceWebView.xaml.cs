using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleSalesforce;
using Xamarin.Forms;

namespace Salesforce
{
    public partial class SalesForceWebView : ContentPage
    {

        RequestCallback _requestCallback = null;
        ErrorCallback _errorParser = null;

        string _nonce = string.Empty;
        string _state = string.Empty;

        public SalesForceWebView()
        {
            InitializeComponent();

            CommunitiesWebView.Navigated  += OnWebViewNavigated;
            CommunitiesWebView.Navigating += OnWebViewNavigating;

             _requestCallback = new RequestCallback();
            _errorParser = new ErrorCallback();

            _nonce = Guid.NewGuid().ToString("N").ToUpper();
            _state = Guid.NewGuid().ToString("N").ToUpper();
        }

        void OnStartCall(object sender, System.EventArgs e)
        {

            List<SimpleSalesforce.Response_Type> responseType = new List<SimpleSalesforce.Response_Type>();
            responseType.Add(SimpleSalesforce.Response_Type.token);
            responseType.Add(SimpleSalesforce.Response_Type.id_token);


            string uri = "https://<salesforcelink>/services/oauth2/authorize";
            string clientId = "";
            string redirectUri = "finsmaaa://sflogin";
            

            SimpleSalesforce.TokenRequest request = new SimpleSalesforce.TokenRequest(uri, responseType, clientId, redirectUri);


            List<SimpleSalesforce.Scope> scope = new List<SimpleSalesforce.Scope>();
            scope.Add(SimpleSalesforce.Scope.openid);
            scope.Add(SimpleSalesforce.Scope.refresh_token);

            request.Scope = scope;
            request.State = _state;
            request.Nonce = _nonce;


            string linkString = request.ToString();
            CommunitiesWebView.Source = linkString;

        }

        /// <summary>
        /// Event fired once the navigation has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            
  
            if(await CheckForError())
            {



            }

        }


        /// <summary>
        /// Checks for the existence of Error Data in the posted page.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckForError()
        {
            bool returnValue = false;


            //get our body of html from our page, and check to see if there are any errors per the Salesforce documentation
            string javascriptExecution = ErrorCallback.ERROR_CHECKING_JAVASCRIPT;
            var result = await CommunitiesWebView.EvaluateJavaScriptAsync(javascriptExecution);

            //execute our expression tester to see if any errors are located.
            returnValue = _errorParser.Validate(result);

            if (returnValue)
            {
               
                System.Diagnostics.Debug.WriteLine(result);

            }

            return returnValue;
        }


        /// <summary>
        /// Even fired when Navigating starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            string formattedUrl = WebUtility.UrlDecode(e.Url);

            Console.WriteLine(e.Url);

            bool parseResult = _requestCallback.Validate(e.Url);

            if(parseResult)
            {
                System.Diagnostics.Debug.Write(parseResult);
                Device.OpenUri(new Uri("https://www.google.com"));
            }

            e.Cancel = true;

        }
    }
}
