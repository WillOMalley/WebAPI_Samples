using System.Collections.Generic;
using System.Web;

// Installed Via NuGet
// Vendor Web Site: http://restsharp.org/usage/serialization.html#newtonsoftjson-aka-json-net 
using RestSharp;

using RestSharp.Serializers.NewtonsoftJson;

namespace HarrisBeach.ChromeRiver.WebServiceCommunicator.CustomObjects
{
    class WebServiceRequest
    {
        #region Private Variables:
        private RestRequest _rRequest;
        private string _webServiceResource = "";
        private Method _webServiceRequestMethod;
        private DataFormat _webServicerequestDataFormat;
        private List<KeyValuePair<string, string>> _qryUrlParamList = new List<KeyValuePair<string, string>>();
        private List<KeyValuePair<string, string>> _requestHeaderList = new List<KeyValuePair<string, string>>();

        private readonly JsonHelper _jHelper;

        private readonly WebServiceCredentials _ntlmWebServiceCredentials;
        #endregion

        #region Public Properties:
        public string WebServiceResource 
        { 
            get => this._webServiceResource; 
            set => this._webServiceResource = value; 
        }
        public Method WebServiceRequestMethod 
        { 
            get => this._webServiceRequestMethod; 
            set => this._webServiceRequestMethod = value; 
        }
        public DataFormat WebServicerequestDataFormat 
        { 
            get => this._webServicerequestDataFormat; 
            set => this._webServicerequestDataFormat = value; 
        }
        public List<KeyValuePair<string, string>> WebServiceRequestUrlQryParamList
        {
            get => this._qryUrlParamList;
            set => this._qryUrlParamList = value;
        }
        public List<KeyValuePair<string, string>> WebServiceRequestHeaderList
        {
            get => this._requestHeaderList;
            set => this._requestHeaderList = value;
        }
        #endregion

        // Constructor
        public WebServiceRequest(WebServiceCredentials NtlmWebServiceCredentials)
        {
            this._jHelper = new JsonHelper();

            this._ntlmWebServiceCredentials = NtlmWebServiceCredentials;
        }

        public RestRequest SetClientRequest()
        {
            this._rRequest = new RestRequest()
            {
                Resource = this._webServiceResource,
                Method = this._webServiceRequestMethod,
                RequestFormat = this._webServicerequestDataFormat,
                Credentials = this._ntlmWebServiceCredentials.GetSystemNetworkCredentials()
            };

            // Add our headers:
            this._rRequest.AddHeaders(this._requestHeaderList);

            // add our query parameters:
            this._qryUrlParamList.ForEach((KeyValuePair<string, string> urlParam) =>
            {
                this._rRequest.AddQueryParameter(urlParam.Key, urlParam.Value, false);
            });

            
            // Format the Request Json:
            this._rRequest.UseNewtonsoftJson(this._jHelper.JsonSettings);

            return this._rRequest;
        }

    }
}
