// Installed Via NuGet
// Vendor Web Site: http://restsharp.org/usage/serialization.html#newtonsoftjson-aka-json-net 
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;


namespace HarrisBeach.ChromeRiver.WebServiceCommunicator.CustomObjects
{

    class WebServiceClient
    {
        #region Private Variables:
        private RestClient _rClient = new RestClient();
        private readonly JsonHelper _jHelper;
        private readonly WebServiceCredentials _ntlmWebServiceCredentials;

        private string _webServiceEndPointPath = "";
        private string _webServiceBaseURL = "";
        private string _webServiceFullURL = "";

        #endregion

        #region Public Properties:
        public string WebServiceEndPointPath 
        { 
            get => this._webServiceEndPointPath; 
            set => this._webServiceEndPointPath = value; 
        }
        public string WebServiceBaseURL 
        { 
            get => this._webServiceBaseURL; 
            set => this._webServiceBaseURL = value; 
        }
        public string WebServiceFullURL 
        { 
            get => this._webServiceFullURL; 
        }
        #endregion

        // Constructor
        public WebServiceClient(WebServiceCredentials NtlmWebServiceCredentials)
        { 
            this._jHelper = new JsonHelper();

            this._ntlmWebServiceCredentials = NtlmWebServiceCredentials;
        }

        // Public Methods:
        public RestClient SetClientConnection()
        {
            // Full URL:
            this._webServiceFullURL = $"{ this._webServiceBaseURL }/{ this.WebServiceEndPointPath }";

            // Initialize our client object
            this._rClient = new RestClient(this._webServiceFullURL);

            this._rClient.Timeout = -1;

            // Setup our Authentication info:
            this.SetClientAuthenticationMethod();

            // Now tell the client how to format the connection criteria:
            this._rClient.UseNewtonsoftJson(this._jHelper.JsonSettings);

            return this._rClient;
        }

        // Private Methods:
        private void SetClientAuthenticationMethod()
        {
            this._rClient.Authenticator = this._ntlmWebServiceCredentials.GetWebServiceNtlmAuthenticator();
        }

    }
}
