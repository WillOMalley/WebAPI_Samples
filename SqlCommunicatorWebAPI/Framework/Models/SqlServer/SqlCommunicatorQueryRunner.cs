using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace SqlCommunicatorWebAPI.Framework.Models.SqlServer
{
    public class SqlCommunicatorQueryRunner
    {
        #region Private variables
        private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();


        #endregion

        #region Public Properties

        #endregion

        public SqlCommunicatorQueryRunner(SqlConnectionStringBuilder value)
        {
            this._builder = value;
        }

        private void InitializeComponent()
        {

        }

        /// <summary>
        /// Call this method to actually perform the query.
        /// </summary>
        /// <param name="qry">SQL Query String</param>
        /// <returns>XmlDocument</returns>
        public XmlDocument ExecuteQuery(string qry)
        {
            XmlDocument returnValue = new XmlDocument();

            using (SqlConnection connection = new SqlConnection(this._builder.ConnectionString))
            {
                connection.Open();

                using (SqlCommand com = new SqlCommand(qry,connection))
                {
                    returnValue.Load(com.ExecuteXmlReader());
                }

                connection.Close();
            }
            
            return returnValue;
        }
    }
}
