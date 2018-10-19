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
using System.Data;
using System.Data.Common;
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    [TestClass]
    public class Db2BugFixingRegressionFixture
    {
        Guid referenceGuid = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("Db2Test");
            CreateTableWithGuidAndBinary();
        }

        [TestCleanup]
        public void TearDown()
        {
            DropTableWithGuidAndBinary();
        }
            

        [TestMethod]
        public void CanGetGuidFromReader()
        {
            using (IDataReader reader = db.ExecuteReader(CommandType.Text, "SELECT * FROM GUID_BINARY_TABLE"))
            {
                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());
                Guid guidValue = reader.GetGuid(0);
                Assert.IsNotNull(guidValue);
                Assert.AreEqual(referenceGuid, guidValue);
                bool boolValue = reader.GetBoolean(1);
                Assert.IsTrue(boolValue);
                Assert.IsFalse(reader.Read());
            }
        }

        void CreateTableWithGuidAndBinary()
        {
            string commandText = null;
            string guidText = referenceGuid.ToString("N");

            commandText = @"DROP TABLE GUID_BINARY_TABLE";
            try
            {
                db.ExecuteNonQuery(CommandType.Text, commandText);
            }
            catch {}

            commandText = @"CREATE TABLE GUID_BINARY_TABLE(GUID_COL BINARY(16), BOOL_COL VARCHAR(10))";
            db.ExecuteNonQuery(CommandType.Text, commandText);

            commandText = @"INSERT INTO GUID_BINARY_TABLE(GUID_COL, BOOL_COL) VALUES (@Param1, @Param2)";
            DbCommand cmd = db.GetSqlStringCommand(commandText);
            db.AddInParameter(cmd, "@Param1", DbType.Binary, referenceGuid.ToByteArray());
            db.AddInParameter(cmd, "@Param2", DbType.String, "true");
            db.ExecuteNonQuery(cmd);
        }

        void DropTableWithGuidAndBinary()
        {
            string commandText = null;

            commandText = @"DROP TABLE GUID_BINARY_TABLE";
            try
            {
                db.ExecuteNonQuery(commandText);
            }
            catch {}
        }
    }
}
