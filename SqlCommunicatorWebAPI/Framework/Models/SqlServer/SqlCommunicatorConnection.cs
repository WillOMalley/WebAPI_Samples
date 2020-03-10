using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SqlCommunicatorWebAPI.Framework.Models.SqlServer;
using System.Xml;

namespace SqlCommunicatorWebAPI.Framework.Models.SqlServer
{
    public class SqlCommunicatorConnection
    {

        #region Private variables
        private string _currentUserWindowsUserName = "";
        private string _sqlServerName = "";
        private string _sqlServerPort = "";
        private string _sqlServerInstanceName = "";
        private bool _useSqlServerAuthentication;
        private bool _useWindowsAuthentication;
        private bool _useAzureEncryption;
        private bool _useTrustedConnection;
        private bool _useNamedInstance;
        private string _sqlServerLoginUserName = "";
        private string _sqlServerLoginPassword = "";
        private string _sqlQuery = "";

        private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
        private SqlCommunicatorQueryRunner _runner;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the current users Windows User Name.
        /// </summary>
        [BindRequired]
        public string WindowsUserName
        {
            get { return this._currentUserWindowsUserName; }
            set { this._currentUserWindowsUserName = value; }
        }


        /// <summary>
        /// Gets or Sets the name of the Host.
        /// </summary>
        [BindRequired]
        public string SqlServerName
        {
            get { return this._sqlServerName; }
            set { this._sqlServerName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Instance of Sql Server you would like to connect to on a given host.
        /// Don't use this unless you know for sure you need to connect to a named instance.
        /// </summary>
        [BindRequired]
        public string SqlServerInstanceName
        {
            get { return this._sqlServerInstanceName; }
            set { this._sqlServerInstanceName = value; }
        }

        /// <summary>
        /// Gets or Sets the Port to use to connect to the SQL Server Host.
        /// Default value is 1433 but this can change. If you don't know what port to use for a specific instance of SQL Server
        /// contact your DBA or System Administrator.
        /// </summary>
        [BindRequired]
        public string SqlServerPort
        {
            get { return this._sqlServerPort; }
            set { this._sqlServerPort = value; }
        }

        /// <summary>
        /// Gets or Sets the option to use SQL Server Authentication.
        /// When selected, you must supply both the SQL Login User Name and Password.
        /// </summary>
        [BindRequired]
        public bool UseSqlServerAuthentication
        {
            get { return this._useSqlServerAuthentication; }
            set { this._useSqlServerAuthentication = value; }
        }

        /// <summary>
        /// Gets or Sets the option to use Windows Authentication.
        /// Depending on how you are connecting and how this Web Service API is being hosted, this may not be an option.
        /// Be sure to ask your System Administrator how this Web Service is being hosted.
        /// </summary>
        [BindRequired]
        public bool UseWindowsAuthentication
        {
            get { return this._useWindowsAuthentication; }
            set { this._useWindowsAuthentication = value; }
        }

        /// <summary>
        /// Gets or Sets the option to encrypt the connection string being passed to the server.
        /// Most of the documentation you will find for connecting to an SQL Server database being hosted on Azure says to encrypt the connection string.
        /// Be sure to check the configuration in Azure in order to make sure connection requests coming from this Web API have been allowed.
        /// </summary>
        [BindRequired]
        public bool UseAzureEncryption
        {
            get { return this._useAzureEncryption; }
            set { this._useAzureEncryption = value; }
        }

        /// <summary>
        /// Gets or Sets the option to use a Trusted Connection.
        /// When Windows Authentication is set to True this property will be set to true by default.
        /// Otherwise this property will be defaulted to false.
        /// </summary>
        [BindRequired]
        public bool UseTrustedConnection
        {
            get { return this._useTrustedConnection; }
            set { this._useTrustedConnection = value; }
        }

        /// <summary>
        /// Gets or Sets the option to use a Named Instance when connecting to SQL Server.
        /// If you would like to use a Named Instance Ex: MYSERVER/MYINSTANCE when connecting to SQL Server you need to set this to True.
        /// If this has not been set to True, any value set for the SqlServerInstanceName property will not be used.
        /// </summary>
        [BindRequired]
        public bool UseNamedInstance
        {
            get { return this._useNamedInstance; }
            set { this._useNamedInstance = value; }
        }

        /// <summary>
        /// Gets or Sets the SQL Login User Name.
        /// </summary>
        [BindRequired]
        public string SqlServerLoginUserName
        {
            get { return this._sqlServerLoginUserName; }
            set { this._sqlServerLoginUserName = value; }
        }

        /// <summary>
        /// Gets or Sets the SQL Login Password.
        /// </summary>
        [BindRequired]
        public string SqlServerLoginPassword
        {
            get { return this._sqlServerLoginPassword; }
            set { this._sqlServerLoginPassword = value; }
        }

        /// <summary>
        /// T-SQL Query to execute.
        /// </summary>
        [BindRequired]
        public string SqlQuery
        {
            get { return this._sqlQuery; }
            set { this._sqlQuery = value; }
        }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlCommunicatorConnection()
        {

        }

        /// <summary>
        /// Called by the constructor to perform any initialization code
        /// </summary>
        private void InitializeComponent()
        {

        }

        #region Public methods

        /// <summary>
        /// Use this method to build the connection string that will be passed to SQL Server.
        /// This must be called before attempting to execute a query.
        /// </summary>
        public void BuildSqlServerConnectionString()
        {
            this._builder.DataSource = this.SqlServerName + (this.UseNamedInstance ? "\\" + this.SqlServerInstanceName : "") + "," + this.SqlServerPort.ToString();
            this._builder.InitialCatalog = "master";
            if (this.UseSqlServerAuthentication)
            {
                this._builder.IntegratedSecurity = false;
                this._builder.UserID = this.SqlServerLoginUserName;
                this._builder.Password = this.SqlServerLoginPassword;
            }
            else
            {
                this._builder.IntegratedSecurity = true;
            }
            this._builder.ApplicationIntent = ApplicationIntent.ReadOnly;

            Console.WriteLine("Connection String generated: " + this._builder.ConnectionString);
        }

        /// <summary>
        /// Call this method to execute the SQL Query.
        /// This method calls the ExecuteQuery method in the SqlCommunicatorQueryRunner class.
        /// </summary>
        /// <returns>XmlDocument</returns>
        public XmlDocument ExecuteQuery()
        {
            XmlDocument returnValue = new XmlDocument();

            this._runner = new SqlCommunicatorQueryRunner(this._builder);

            returnValue = this._runner.ExecuteQuery(this.SqlQuery);

            return returnValue;
        }

        /// <summary>
        /// Call this method to do a little house cleaning.
        /// Note, this method does not perform any garbage collection. 
        /// It only resets in memory objects that have been instantiated for use in this Class.
        /// </summary>
        public void Dispose()
        {
            this._builder.Clear();
        }
        #endregion

        #region Helpers


        #endregion

    }
}
