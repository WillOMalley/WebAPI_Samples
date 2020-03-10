using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text;
using SqlCommunicatorWebAPI.Framework.Helpers.JsonHelper;
using SqlCommunicatorWebAPI.Framework.Models.SqlServer;


namespace SqlCommunicatorWebAPI.Framework.Helpers.ControllerHelpers
{
    public class SqlServerControllerHelper
    {
        JsonFormattingHelper _jHelper = new JsonFormattingHelper();

        public SqlServerControllerHelper()
        {

            this.InitializeComponent();
        }

        private void InitializeComponent()
        {

        }

        public string GeneratePrettyJson()
        {
            return "<HTML> "
                 + "<STYLE> "
                 + "body { "
                 + "   font-family: courier; "
                 + "   font-size: x-small; "
                 + "   font-weight: normal; "
                 + "   -webkit-font-smoothing: antialiased; "
                 + "   text-decoration: none;"
                 + "} "
                 + "</STYLE> "
                 + " <BODY> "
                 + "<H1>Welcome to the SQL Communicator .NET Core Web API!</H1>"
                 // Warning!! There are HTML characters in here for formatting purposes!
                 + (string)this._jHelper.GetThatPrettyJsonString()
                 + " </BODY> "
                 + "</HTML>";
        }

        public string GeneratePrettyXml(SqlCommunicatorConnection connection)
        {
            XmlDocument xDoc = new XmlDocument();

            // Set our output settings:
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.OmitXmlDeclaration = false;
            xmlSettings.Indent = true;
            xmlSettings.NewLineOnAttributes = true;

            if (connection != null)
            {
                XPathNavigator xNav = xDoc.CreateNavigator();

                // Generate our Xml Document:
                using (XmlWriter xw = xNav.AppendChild())
                {
                    XmlSerializer xs = new XmlSerializer(connection.GetType());
                    xs.Serialize(xw, connection);
                }
            }

            return xDoc.ToString();
        }

        public dynamic GeneratePrettyHTML()
        {
            dynamic returnValue = "";



            return returnValue;
        }


        #region Helpers



        #endregion

    }
}
