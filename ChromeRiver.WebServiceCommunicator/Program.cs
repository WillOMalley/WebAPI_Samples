using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

// Installed via NuGet
// Vendor Web Site: https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm

// Installed Via NuGet
// Vendor Web Site: http://restsharp.org/usage/serialization.html#newtonsoftjson-aka-json-net 
using RestSharp;
using RestSharp.Authenticators;


namespace HarrisBeach.ChromeRiver.WebServiceCommunicator
{
    enum WebServiceRequestEnvironment
    {
        Development = 0,
        TestOrQA = 1,
        Production = 2,
        Default = 3
    }

    class Program
    {
        private ConsoleKeyInfo _cKeyInfo;
        private WebServiceRequestEnvironment _webServiceEnvironment = WebServiceRequestEnvironment.Default;

        private string _webServiceRequestUserName = "";
        private string _webServiceRequestPassword = "";
        private string _webServiceRequestUserDomain = "";

        private string _webServiceUrlProtocol = "";
        private string _webServiceHostName = "";
        private string _webServiceControllerVersion = "";
        private string _webServiceEndPointPath = "";
        private string _webServiceAPIResource = "";

        private string _webServiceAPIKey = "";
        private string _webServiceAPICustomerCode = "";

        // Objects I created:
        private CustomObjects.WebServiceClient _wsClient;
        private CustomObjects.WebServiceRequest _wsRequest;
        private CustomObjects.WebServiceCredentials _ntlmWebServiceCredentials;

        private RestClient _rClient;
        private RestRequest _rRequest;

        public static void Main()
        {
            int returnValue;

            Program p = new Program();

            returnValue = p.StartProgram();

            Console.WriteLine($"Returned with Status Code ({ returnValue })");

            return;
        }

        private int StartProgram()
        {
            int returnValue;

            // Tell .NET Framework to accept more certificate types than it does by default.
            // I'm just going to add all of them.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Tls13
                                                 | SecurityProtocolType.Ssl3;

            // This method sets up various pieces of information necessary for both
            // the RestSharp.RestClient and the RestSharp.RestRequest.
            this.InitializeConponent();

            // Initialise our RestSharp.RestClient object:
            this.InitializeClient();

            // Initialise our RestSharp.RestResponse object:
            this.InitializeRequest();

            // Using the Credentials we set up in the initializeComponent method.
            // We are going to create a new RestSharp.Authenticators.IAuthenticator interface Object.
            IAuthenticator wsAuthenticator = this._ntlmWebServiceCredentials.GetWebServiceNtlmAuthenticator();

            // We can now attempt to Authenticate both the RestSharp.RestClient and the RestSharp.RestRequest
            // using the RestSharp.Authenticators.IAuthenticator interface Object created above.
            wsAuthenticator.Authenticate(this._rClient, this._rRequest);

            // Execute request and evaluate the returned values:
            
            returnValue = this.EvaluateWebServiceResponse( this._rClient.Execute(this._rRequest ) );

            // 
            return returnValue;
        }

        private int EvaluateWebServiceResponse(IRestResponse WebServiceResponse)
        {
            string responseParameters = "";

            responseParameters = string.Join(", "
                                             ,this._rRequest.Parameters.Select(p => 
                                                                                "Name: " + p.Name + 
                                                                                " Value: " + p.Value == null ? "NULL" : p.Value
                                                                              ).ToArray()
                                            );

            Console.WriteLine(Environment.NewLine);

            if (WebServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(
                                   $"Request to {this._webServiceHostName} Resource: { this._rRequest.Resource } " + Environment.NewLine +
                                   $"Succeeded with status code: ({ WebServiceResponse.StatusCode }) " + Environment.NewLine +
                                   $"Parameters Passed: { responseParameters } " + Environment.NewLine +
                                   $"Content: { WebServiceResponse.Content }"
                                 );

            }
            else
            {
                Console.WriteLine(
                                   $"Request to {this._webServiceHostName}/{this._webServiceEndPointPath} Resource: { this._rRequest.Resource } " + Environment.NewLine +
                                   $"Failed with status code: ({ WebServiceResponse.StatusCode }) " + Environment.NewLine +
                                   $"Parameters Passed: { responseParameters } " + Environment.NewLine +
                                   $"Content: { WebServiceResponse.Content }"
                                 );

                Exception wsRequestException = WebServiceResponse.ErrorException;

                if (wsRequestException != null)
                {
                    Console.WriteLine(
                                       $"The following Exception occured:" + Environment.NewLine +
                                       $"Source: { wsRequestException.Source } " + Environment.NewLine +
                                       $"Message: { wsRequestException.Message } " + Environment.NewLine +
                                       (wsRequestException.InnerException != null ? $"Inner Exception Message: { wsRequestException.InnerException.Message }" : "") + Environment.NewLine +
                                       $"Stack Trace: { wsRequestException.StackTrace  }"
                                     );

                }
                else
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("No exceptions were encountered during execution.");
                }
            }

            return (int)WebServiceResponse.StatusCode;

        }

        #region Helper Functions

        /// <summary>
        /// Called by the StartProgram method.
        /// This method is used to set up various pieces of information used by
        /// the RestSharp.RestClient object, RestSharp.RestRequest object and
        /// the RestSharp.Authenticators.IAuthenticator interface Object.
        /// </summary>
        private void InitializeConponent()
        {
            // In the event there are multiple environments for the Web Service you want to connect to.
            // There could be different Key values for (a) given environment.
            // Here we can set the environment we want to use for this Web Service Request.
            this._webServiceEnvironment = this.PromptUserForWebServiceEnvironment();

            // Based on the Environment, we can now set up our Web Service URL, User Name, Password and Credentials.
            this.SetWebServiceUrlAndCredentials();

            // This tells us what the endpoint version to use
            // Currently I've seen references to both V1 and V2.
            this._webServiceControllerVersion = "v2";

            // This tells us what endpoint to use:
            this._webServiceEndPointPath = "";

            // This tells us what {controller} to use:
            this._webServiceAPIResource = "persons";

        }

        /// <summary>
        /// Called by the initializeConponent method.
        /// This method is used to ask the user what Environment they would like to use
        /// for the Web Service Request.
        /// </summary>
        /// <returns>
        /// <see cref="WebServiceRequestEnvironment"/>
        /// </returns>
        private WebServiceRequestEnvironment PromptUserForWebServiceEnvironment()
        {
            WebServiceRequestEnvironment returnValue = WebServiceRequestEnvironment.Default;

            string selectedEnvironment = string.Empty;

            // intercept:
            //  Determines whether to display the pressed key in the console window. 
            //      true to not display the pressed key.
            //      false to display the pressed key.
            bool intercept = false;

            // Display Environment Prompt.
            Console.Clear();
            Console.WriteLine("Please choose an Environment.");
            Console.WriteLine("Valid values are D for Development, T for TestOrQA and P for Production.");
            Console.WriteLine("You may also just hit Enter to default to T for TestOrQA.");
            Console.Write("Environment:");
         
            // Obtains the next character or function key pressed by the user. 
            // The pressed key is optionally displayed in the console window using the
            // intercept value.
            this._cKeyInfo = Console.ReadKey(intercept);
            
            if(this._cKeyInfo.Key == ConsoleKey.Enter)
            {
                selectedEnvironment = "T";
            }
            else
            {
                selectedEnvironment = this._cKeyInfo.KeyChar.ToString().ToUpper();
            }

            switch (selectedEnvironment)
            {
                case "D":
                    returnValue = WebServiceRequestEnvironment.Development;
                    break;

                case "T":
                    returnValue = WebServiceRequestEnvironment.TestOrQA;
                    break;

                case "P":
                    returnValue = WebServiceRequestEnvironment.Production;
                    break;
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"You will be using the { returnValue.ToString() } Environment");

            return returnValue;
        }

        /// <summary>
        /// Called by the initializeConponent method.
        /// This method is used to set the User Name and Password for the Web Service Environment.
        /// Default values are provided for all Environments except Production.
        /// The Production Environment Prompts the user for a Password and uses the User Name of the
        /// currently logged on User. 
        /// </summary>
        private void SetWebServiceUrlAndCredentials()
        {
            // Protocol will always be https:
            this._webServiceUrlProtocol = "https";

            // The Current Users Domain will not change.
            this._webServiceRequestUserDomain = Environment.UserDomainName.ToString();


            // Set URL, API Key and Credentials based on environment:
            switch (this._webServiceEnvironment)
            {
                case WebServiceRequestEnvironment.Production:

                    // Set up our Production Web Service URL:
                    this._webServiceHostName = "service.chromeriver.com";

                    // TODO: Add Service API Key
                    this._webServiceAPIKey = "";

                    // This is our Customer Code for Chrome River.
                    this._webServiceAPICustomerCode = "HRB";

                    // Get the current users UserName:
                    this._webServiceRequestUserName = Environment.UserName.ToString();

                    // Get the Current Users Password for the Production Enironment.
                    this._webServiceRequestPassword = this.PromptForWebServiceEnvironmentPassword();

                    break;

                default:

                    // Set up our Default Web Service URL:
                    this._webServiceHostName = "qa-service.chromeriver.com";

                    // TODO: Add Service API Key
                    this._webServiceAPIKey = "";

                    // TODO: Add Customer Code
                    this._webServiceAPICustomerCode = "";

                    // Set up our Default User Credentials:
                    this._webServiceRequestUserName = "";
                    this._webServiceRequestPassword = "";

                    break;
            }

            // Initialize our Web Service Credentials Object.
            this._ntlmWebServiceCredentials = new CustomObjects.WebServiceCredentials(this._webServiceRequestUserName, this._webServiceRequestPassword, this._webServiceRequestUserDomain);
        }

        /// <summary>
        /// Called by the SetWebServiceUrlAndCredentials method for the Web Services Production environment.
        /// This method prompts the user for a password to use when authenticating both the RestSharp.RestClient object
        /// and the RestSharp.RestRequest object.
        /// </summary>
        /// <returns>
        /// <see cref="string"/>
        /// </returns>
        private string PromptForWebServiceEnvironmentPassword()
        {
            string returnValue = "";

            Console.WriteLine($"Please enter the Password you would like to use for the { this._webServiceEnvironment } environment.");
            Console.Write("Password: ");

            do
            {
                this._cKeyInfo = Console.ReadKey(true);

                if (this._cKeyInfo.Key != ConsoleKey.Enter && this._cKeyInfo.Key != ConsoleKey.Backspace)
                {
                    returnValue += this._cKeyInfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (this._cKeyInfo.Key == ConsoleKey.Backspace && this._webServiceRequestPassword.Length > 0)
                    {
                        // remove the most recent character from the password string.
                        returnValue = returnValue.Substring(0, (returnValue.Length) - 1);

                        // move back 1 character, overwrite the last '*' character with a space then move back again.
                        Console.Write("\b \b");
                    }
                    else if (this._cKeyInfo.Key != ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            return returnValue;
        }

        /// <summary>
        /// Called by the StartProgram method.
        /// This method creates our RestSharp.RestClient object.
        /// </summary>
        private void InitializeClient()
        {
            // Initialize Custom Client Object:
            this._wsClient = new CustomObjects.WebServiceClient(this._ntlmWebServiceCredentials)
            {
                WebServiceBaseURL = $"{ this._webServiceUrlProtocol }://{this._webServiceHostName}",
                WebServiceEndPointPath = this._webServiceControllerVersion
            };

            // If there is nothing between the Version and the Resource in the Request URL.
            // We don't want to add anything additional.
            this._wsClient.WebServiceEndPointPath += (string.IsNullOrEmpty(this._webServiceEndPointPath) || string.IsNullOrWhiteSpace(this._webServiceEndPointPath)) ? "" : string.Concat("/", this._webServiceEndPointPath);

            // Initialize Client:
            this._rClient = this._wsClient.SetClientConnection();
        }

        /// <summary>
        /// Called by the StartProgram method.
        /// This method creates our RestSharp.RestRequest object.
        /// </summary>
        private void InitializeRequest()
        {
            // Initialize Custom Request Object:
            this._wsRequest = new CustomObjects.WebServiceRequest(this._ntlmWebServiceCredentials)
            {
                WebServiceResource = this._webServiceAPIResource,
                WebServiceRequestMethod = Method.GET,
                WebServicerequestDataFormat = DataFormat.Json,
                WebServiceRequestHeaderList = this.AddWebServiceRequestHeaders(),
                WebServiceRequestUrlQryParamList = this.AddWebServieUrlQueryParameters()
            };

            // Initialize Request:
            this._rRequest = this._wsRequest.SetClientRequest();
        }

        /// <summary>
        /// Called by the InitializeRequest method.
        /// This method sets up the RestSharp.RestRequest Header information.
        /// </summary>
        /// <returns>
        /// List< KeyValuePair<string, string> >
        /// </returns>
        private List<KeyValuePair<string, string>> AddWebServiceRequestHeaders()
        {
            List<KeyValuePair<string, string>> returnValue = new List<KeyValuePair<string, string>>
            {

                // Add Request Headers that need to get sent as part of the Request:
                new KeyValuePair<string, string>("x-api-key", this._webServiceAPIKey),
                new KeyValuePair<string, string>("Content-Type", "application/json"),
                new KeyValuePair<string, string>("customerCode", this._webServiceAPICustomerCode),
                new KeyValuePair<string, string>("Accept-Encoding", "gzip, deflate, br"),
                new KeyValuePair<string, string>("Connection", "keep-alive"),
                new KeyValuePair<string, string>("Accept", "*/*"),
                new KeyValuePair<string, string>("Cache-Control", "no-cache"),
                new KeyValuePair<string, string>("Host", this._webServiceHostName)

            };

            return returnValue;
        }

        /// <summary>
        /// Called by the InitializeRequest method.
        /// This method sets up the RestSharp.RestRequest Web Service Url Query parameter information.
        /// </summary>
        /// <returns>
        /// List< KeyValuePair<string, string> >
        /// </returns>
        private List<KeyValuePair<string, string>> AddWebServieUrlQueryParameters()
        {
            List<KeyValuePair<string, string>> returnValue = new List<KeyValuePair<string, string>>
            {

                // Add any URL Parameters that need to get sent as part of the URL string:
                
                // TODO: Add URL Query Parameters:
                new KeyValuePair<string, string>("primaryEmailAddress", "someone@somewhere.com"),

               // new KeyValuePair<string, string>("includePersonUdas", "true")
            };

            return returnValue;
        }

        #endregion
    }

}
