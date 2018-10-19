//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBM.Data.DB2.iSeries;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
#pragma warning disable 612, 618
    [TestClass]
    public class Db2DatabaseFixture
    {
        IConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            configurationSource = Db2TestConfigurationSource.CreateConfigurationSource();
        }

        [TestMethod]
        public void CanConnectToDb2AndExecuteAReader()
        {
            var Db2Database = new DatabaseSyntheticConfigSettings(this.configurationSource).GetDatabase("Db2Test").BuildDatabase();
            DbConnection connection = Db2Database.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is iDB2Connection);
            connection.Open();
            DbCommand cmd = Db2Database.GetSqlStringCommand("Select * from Region");
            Db2Database.ExecuteReader(cmd);
        }

        [TestMethod]
        public void CanConnectToDb2AndExecuteCommand()
        {
            ConnectionStringSettings data = ConfigurationManager.ConnectionStrings["Db2Test"];
            Db2Database Db2Database = new Db2Database(data.ConnectionString);
            DbConnection connection = Db2Database.CreateConnection();
            
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is iDB2Connection);
            connection.Open();
            DbCommand cmd = Db2Database.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
            cmd.Connection = connection;
            cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructingAnDb2DatabaseWithConnectionStringThrows()
        {
            new Db2Database(null);
        }
    }
#pragma warning restore 612, 618
}
