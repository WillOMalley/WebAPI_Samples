using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SqlCommunicatorWebAPI.Framework.Models.SqlServer;


namespace SqlCommunicatorWebAPI.Framework.Helpers.JsonHelper
{
    public class JsonFormattingHelper
    {
        #region Private variables:

        /***********************************************************************
         * RegEx info: 
         * https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
         *
         ***********************************************************************/
        private Regex _regExTab;
        private Regex _regExNewLine;

        #endregion

        public JsonFormattingHelper()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._regExTab = new Regex("/\t/");
            this._regExNewLine = new Regex("/\r\n/");
        }

        public dynamic GetThatPrettyJsonString()
        {
            return this.PrettyJsonStringBuilder();
        }

        #region Private helpers

        private dynamic PrettyJsonStringBuilder()
        {
            // dynamic returnValue;
            /*
            JsonSerializerSettings jSerializerSettings = new JsonSerializerSettings();
            jSerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            jSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            jSerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // Parse it!
            returnValue = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(
                                                                                     this.JsonDocumentationGenerator()
                                                                                    ,typeof(SqlCommunicatorConnection)
                                                                                    ,jSerializerSettings                                                                                    
                                                                                   )
                                                       ,typeof(string)
                                                       ,jSerializerSettings
                                                       );
            */
            return this.JsonDocumentationGenerator();
        }

        private string JsonDocumentationGenerator()
        {
            StringBuilder strBuilder = new StringBuilder();
            StringWriter strWriter = new StringWriter(strBuilder);
            JsonWriter jsnWriter = new JsonTextWriter(strWriter);
            char IndentChar = '\t';

            string returnValue = "";

            using (strWriter)
            {
                using (jsnWriter)
                {
                    jsnWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jsnWriter.AutoCompleteOnClose = true;
                    
                    jsnWriter.WriteComment(
                                           "* <B>Created by</B>: Will O\'Malley" + Environment.NewLine +
                                           "* <B>For</B>: Used by the Query Wizard Sharepoint Web Part" + Environment.NewLine +
                                           "* <B>Routes</B>:" + Environment.NewLine +
                                           "*" + new string(IndentChar ,1) + "<B>[</B><FONT COLOR=\"MidnightBlue\"><B>HttpGet</B></FONT><B>]</B> <FONT COLOR=\"DarkRed\">Get()</FONT>" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<B>URL</B>: /api/SqlServer" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Param</B>:</FONT> <FONT COLOR=\"DimGray\"><B>None</B></FONT>" + Environment.NewLine +
                                           "*" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "<B>[</B><FONT COLOR=\"MidnightBlue\"><B>HttpGet</B></FONT><B>]</B> <FONT COLOR=\"DarkRed\">Get(<FONT COLOR=\"DimGray\"><B>StringFormat</B></FONT>)</FONT>" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<B>URL</B>: /api/SqlServer/json" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Param</B>:</FONT> <FONT COLOR=\"DimGray\"><B>StringFormat</B></FONT>" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Type</B>:</FONT> string" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Values</B>:</FONT> Json, Xml, HTML" + Environment.NewLine +
                                           "*" + new string(IndentChar, 3) + "These values are <B><U>NOT</U></B> case sensitive." + Environment.NewLine +
                                           "*" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "<B>[</B><FONT COLOR=\"MidnightBlue\"><B>HttpPost</B></FONT><B>]</B> <FONT COLOR=\"DarkRed\">Post(<FONT COLOR=\"DimGray\"><B>SqlCommunicatorConnection</B> value</FONT>)</FONT>" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Param</B>:</FONT> <FONT COLOR=\"DarkGray\"><B>value</B></FONT>" + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Type</B>:</FONT> " + typeof(SqlCommunicatorConnection).FullName.Trim() + Environment.NewLine +
                                           "*" + new string(IndentChar, 2) + "<FONT COLOR=\"DarkBlue\"><B>Values</B>:</FONT> Serialized Json string representing Object." + Environment.NewLine +
                                           
                                           // Add section seperator line:
                                           "*" + new string('*', 90) + Environment.NewLine +

                                           // Begin Example section:
                                           "* <B>Example</B>:" + Environment.NewLine +
                                           "* ||[" + Environment.NewLine +

                                           // I'm using Concat to generate the Key/Value pairs.
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"WindowsUserName\"", "</FONT>", " <B>:</B> ", "\"DOMAIN\\UserName\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"SqlServerName\"", "</FONT>", " <B>:</B> ", "\"SQL Server Name\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"SqlServerInstanceName\"", "</FONT>", " <B>:</B> ", "\"Instance\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"SqlServerPort\"", "</FONT>", " <B>:</B> ", "1433", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"UseSqlServerAuthentication\"", "</FONT>", " <B>:</B> ", "\"true / false\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"UseWindowsAuthentication\"", "</FONT>", " <B>:</B> ", "\"true / false\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"UseAzureEncryption\"", "</FONT>", " <B>:</B> ", "\"true / false\"" + "," + Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"UseTrustedConnection\"", "</FONT>", " <B>:</B> ", "\"true / false\"" + "," + Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"UseNamedInstance\"", "</FONT>", " <B>:</B> ", "\"true / false\"" + "," + Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"SqlServerLoginUserName\"", "</FONT>", " <B>:</B> ", "\"SQL User Login Name\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"SqlServerLoginPassword\"", "</FONT>", " <B>:</B> ", "\"SQL User Login Password\"", ",", Environment.NewLine) +
                                           "*" + new string(IndentChar, 1) + string.Concat("<FONT COLOR=\"DarkRed\">", "\"SqlQuery\"", "</FONT>", " <B>:</B> ", "\"SELECT * FROM SOME_TABLE...\"", Environment.NewLine) +
                                           "* ]||" + Environment.NewLine +
                                           // End Example section:

                                           // Add section seperator line:
                                           new string('*', 90) + Environment.NewLine +

                                           // Begin Notes section:
                                           "* <B>Notes</B>:" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "<B>Comments</B> are\'t really permitted in a JSON string, so, you will want to copy the string" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "below and paste it into NOTEPAD and remove them. " + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "This should also help remove the HTML formatting."
                                           // End Notes section:
                                           );

                    jsnWriter.WriteStartObject();

                    jsnWriter.WriteComment(
                                           "* <B>Windows User Name</B>:" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "The application will try to get this info from SharePoint" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "If the app is not able to do that, the value for SqlServerLoginUserName gets passed." + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "However, because the app was unable to get the users User Name, this value never gets used." + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "This service will check the UseWindowsAuthentication and UseSqlServerAuthentication values to make sure" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "the user did not select Windows Authentication on accident."
                                           );
                    jsnWriter.WritePropertyName("WindowsUserName", false);
                    jsnWriter.WriteValue("Windows User Name");

                    jsnWriter.WritePropertyName("SqlServerName", false);
                    jsnWriter.WriteValue("Server Name");

                    jsnWriter.WritePropertyName("SqlServerInstanceName", false);
                    jsnWriter.WriteValue("Instance Name");

                    jsnWriter.WritePropertyName("SqlServerPort", false);
                    jsnWriter.WriteValue("Depends on Server Configuration.");

                    jsnWriter.WritePropertyName("UseSqlServerAuthentication", false);
                    jsnWriter.WriteValue("true OR false");

                    jsnWriter.WritePropertyName("UseWindowsAuthentication", false);
                    jsnWriter.WriteValue("true OR false");

                    jsnWriter.WritePropertyName("UseAzureEncryption", false);
                    jsnWriter.WriteValue("true OR false");
                    
                    jsnWriter.WriteComment( 
                                           "* <B>Trusted Connection</B>:" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "When connecting to SQL Server, this value tells us to use a Trusted Connection. " + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "This value is set on the backend and is not configurable from the connection wizard UI."
                                          );
                    jsnWriter.WritePropertyName("UseTrustedConnection", false);
                    jsnWriter.WriteValue("true OR false");

                    jsnWriter.WritePropertyName("UseNamedInstance", false);
                    jsnWriter.WriteValue("true OR false");

                    jsnWriter.WriteComment(
                                           "* <B>SQL Server Login User Name</B>: " + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "Make sure the SQL Login you are using has access to the Server and Database you are trying to query."
                                          );
                    jsnWriter.WritePropertyName("SqlServerLoginUserName", false);
                    jsnWriter.WriteValue("SQL Login User Name");

                    jsnWriter.WritePropertyName("SqlServerLoginPassword", false);
                    jsnWriter.WriteValue("SQL Login Password");

                    jsnWriter.WriteComment(
                                           "* <B>SQL Query</B>:" + Environment.NewLine +
                                           "*" + new string(IndentChar, 1) + "This is the final/formatted query that gets passed to SQL Server for execution."
                                          );
                    jsnWriter.WritePropertyName("SqlQuery", false);
                    jsnWriter.WriteValue("SQL Query");

                    jsnWriter.WriteEndObject();

                    jsnWriter.Close();
                }

                strWriter.Close();
            }

            returnValue = strBuilder.ToString();

            strBuilder.Clear();

            return returnValue.Replace(Environment.NewLine, "<BR />")
                              .Replace("/*", string.Concat("<BR />", "<FONT COLOR=\"DarkGreen\">", "/", new string('*', 90), "<BR />"))
                              .Replace("*/", string.Concat("<BR />", new string('*', 90), "/</FONT><BR />"))
                               // I'm going to place the Json in a DIV.
                              .Replace("{",  string.Concat("<BR />", "<B>", "Formatted JSON string:", "</B>", "<BR />", "<DIV STYLE=\"background: #d3d3d3; width: 95%; height: 60%; overflow: scroll;\">", "<B>", "{", "</B>", "<BR />"))
                              .Replace("}",  string.Concat("<BR />", "<B>", "}", "</B>", "</DIV>", "<BR />"))
                               // Get rid of extra <BR /> tags.
                              .Replace("<BR /><BR />", "<BR />")
                               // Indenting:
                              .Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
                              .Replace("||[", "<B>{</B>")
                              .Replace("]||", "<B>}</B>")
                              .Trim();
        }

        #endregion

    }
}
