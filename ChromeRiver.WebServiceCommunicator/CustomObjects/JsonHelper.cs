// Installed via NuGet
// Vendor Web Site: https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace HarrisBeach.ChromeRiver.WebServiceCommunicator.CustomObjects
{
    public class JsonHelper
    {
        private readonly JsonSerializerSettings _jSettings;

        /// <summary>
        /// Gets the Json settings for Serialization and Deserialization:
        /// </summary>
        public JsonSerializerSettings JsonSettings
        {
            get { return this._jSettings;  }
        }



        // Constructor:
        public JsonHelper() =>

            // Initialize object:
            this._jSettings = new JsonSerializerSettings
            {
                // Define the Json Serializer settings:
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                TypeNameHandling = TypeNameHandling.None
            };
    }
}
