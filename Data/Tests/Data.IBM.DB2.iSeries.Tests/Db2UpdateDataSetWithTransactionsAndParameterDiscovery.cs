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

using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;
using EntLibContrib.Data.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    [TestClass]
    public class Db2UpdateDataSetWithTransactionsAndParameterDiscovery : UpdateDataSetWithTransactionsAndParameterDiscovery
    {
        [TestInitialize]
        public void Initialize()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("Db2Test");
            try
            {
                DeleteStoredProcedures();
            }
            catch { }
            CreateStoredProcedures();
            base.SetUp();
        }

        [TestCleanup]
        public void Dispose()
        {
            base.TearDown();
            DeleteStoredProcedures();
        }

        [TestMethod]
        public void Db2ModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }

        protected override void CreateStoredProcedures()
        {
            Db2DataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            Db2DataSetHelper.DeleteStoredProcedures(db);
        }

        protected override void CreateDataAdapterCommands()
        {
            Db2DataSetHelper.CreateDataAdapterCommandsDynamically(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void AddTestData()
        {
            Db2DataSetHelper.AddTestData(db);
        }
    }
}
