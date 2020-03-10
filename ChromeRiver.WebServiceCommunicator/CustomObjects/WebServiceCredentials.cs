using System.Net;

// Installed Via NuGet
// Vendor Web Site: http://restsharp.org/usage/serialization.html#newtonsoftjson-aka-json-net 
using RestSharp.Authenticators;

namespace HarrisBeach.ChromeRiver.WebServiceCommunicator.CustomObjects
{
    class WebServiceCredentials
    {

        #region Private Variables:
        private readonly string _userName = "";
        private readonly string _userPassword = "";
        private readonly string _userDomain = "";

        ICredentials _webServiceCredentials;
        #endregion

        public WebServiceCredentials(string UserName, string UserPassword, string UserDomain)
        {
            this._userName = UserName;
            this._userPassword = UserPassword;
            this._userDomain = UserDomain;

            // Initialize the ICredentials interface and return the NetworkCredentials
            this.SetUserNtlmAuthenticatonCredentials();

        }

        /// <summary>
        /// Use this method to return a RestSharp.Authenticators.NtlmAuthenticator object. 
        /// </summary>
        /// <returns>
        /// RestSharp.Authenticators.NtlmAuthenticator
        /// </returns>
        public NtlmAuthenticator GetWebServiceNtlmAuthenticator()
        {
            // Now we can Instantiate a new NtlmAuthenticator object using the
            // information passed in.
            return new NtlmAuthenticator(this._webServiceCredentials);
        }

        /// <summary>
        /// Use this method to return the System.Net.ICredentials interface object.
        /// </summary>
        /// <returns>
        /// System.Net.ICredentials
        /// </returns>
        public ICredentials GetSystemNetworkCredentials()
        {
            // Simply return the ICredentials interface Object:
            return this._webServiceCredentials;
        }

        private ICredentials SetUserNtlmAuthenticatonCredentials()
        {
            this._webServiceCredentials = new NetworkCredential(this._userName, this._userPassword, this._userDomain);

            return this._webServiceCredentials;
        }

    }
}
