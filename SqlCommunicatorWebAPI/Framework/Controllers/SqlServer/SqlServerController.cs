/************************************************************************************************************
 * Created by Will O'Malley
 * 
 * 
 * ----------------------------------------------------------------------------------------------------------
 * Notes:
 * 
 * ----------------------------------------------------------------------------------------------------------
 * Maintenance Log:
 * Date         Developer           Description
 * 
 ***********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlCommunicatorWebAPI.Framework.Models.SqlServer;
using SqlCommunicatorWebAPI.Framework.Helpers.ControllerHelpers;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json.Net;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SqlCommunicatorWebAPI.Framework.Controllers.SqlServer
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlServerController : Controller
    {
        private SqlCommunicatorConnection _myConnection = new SqlCommunicatorConnection();
        private SqlServerControllerHelper _controllerHelper = new SqlServerControllerHelper();

        // GET: Controllers/<controller>
        // Matches: 'api/<controller>'
        [HttpGet]
        public dynamic Get()
        {
            return this.Get("json");
        }

        // GET: Controllers/<controller>/Format
        // Matches: 'api/<controller>/{StringFormat}'
        [HttpGet("{StringFormat}")]
        public dynamic Get(string StringFormat)
        {
            Response.Clear();

            if (StringFormat != null)
            {
                if (StringFormat.ToLower() == "json" || StringFormat.ToLower() == "html")
                {
                    return new ContentResult{
                                              ContentType = "text/html",
                                              StatusCode = (int) System.Net.HttpStatusCode.OK,
                                              // Warning!! There are HTML characters in here for formatting purposes!
                                              Content = this._controllerHelper.GeneratePrettyJson()
                                            };
                }
                else if (StringFormat.ToLower() == "xml")
                {
                    return new ContentResult
                    {
                        ContentType = "application/xml",
                        StatusCode = (int)System.Net.HttpStatusCode.OK,
                        // Warning!! There are HTML characters in here for formatting purposes!
                        Content = this._controllerHelper.GeneratePrettyXml(this._myConnection)
                    };
                }
                else
                {
                    return this._myConnection;
                }
            }
            else
            {
                return this._myConnection;
            }
        }

        // POST Controllers/<controller>
        [HttpPost]
        public string Post([FromBody]SqlCommunicatorConnection value)
        {
            XmlDocument returnVal = new XmlDocument();
            string xmlString = "";

            if (value != null && value is SqlCommunicatorConnection)
            {

                this._myConnection = value;
                this._myConnection.BuildSqlServerConnectionString();

                returnVal = this._myConnection.ExecuteQuery();

                if (returnVal != null && !string.IsNullOrWhiteSpace(returnVal.OuterXml))
                {
                    xmlString = returnVal.OuterXml;
                }
                else
                {
                    xmlString = "<ERRORS><ERROR>No data was returned!</ERROR></ERRORS>";
                }
            }
            else
            {
                xmlString = "<ROOT><ERRORS><ERROR>No data was returned!</ERROR></ERRORS></ROOT>";
            }

            this._myConnection.Dispose();

            return xmlString;
        }

        // PUT Controllers/<controller>/5
        [HttpPut]
        public void Put([FromBody]string value)
        {
            Console.WriteLine("Data Transferred: " + value);
        }

        // DELETE Controllers/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
