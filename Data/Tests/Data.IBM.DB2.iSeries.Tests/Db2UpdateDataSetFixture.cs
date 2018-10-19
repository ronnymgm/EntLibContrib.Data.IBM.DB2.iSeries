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
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EntLibContrib.Data.TestSupport;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    /// <summary>
    /// Tests executing a batch of commands with insert, delete and update 
    /// using ExecuteUpdateDataTable
    /// </summary>
    [TestClass]
    public class Db2UpdateDataSetFixture : UpdateDataSetFixture
    {
        [TestInitialize]
        public void TestInitialize()
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
        public void OneTimeTearDown()
        {
            base.TearDown();
            DeleteStoredProcedures();
        }

        [TestMethod]
        public void Db2ModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }

        [TestMethod]
        public void Db2DeleteRowWithStoredProcedure()
        {
            base.DeleteRowWithStoredProcedure();
        }

        [TestMethod]
        public void Db2InsertRowWithStoredProcedure()
        {
            base.InsertRowWithStoredProcedure();
        }

        [TestMethod]
        public void Db2DeleteRowWithMissingInsertAndUpdateCommands()
        {
            base.DeleteRowWithMissingInsertAndUpdateCommands();
        }

        [TestMethod]
        public void Db2UpdateRowWithMissingInsertAndDeleteCommands()
        {
            base.UpdateRowWithMissingInsertAndDeleteCommands();
        }

        [TestMethod]
        public void Db2InsertRowWithMissingUpdateAndDeleteCommands()
        {
            base.InsertRowWithMissingUpdateAndDeleteCommands();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Db2UpdateDataSetWithAllCommandsMissing()
        {
            base.UpdateDataSetWithAllCommandsMissing();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Db2UpdateDataSetWithNullTable()
        {
            base.UpdateDataSetWithNullTable();
        }

        protected override void CreateDataAdapterCommands()
        {
            Db2DataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures()
        {
            Db2DataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            Db2DataSetHelper.DeleteStoredProcedures(db);
        }

        protected override void AddTestData()
        {
            Db2DataSetHelper.AddTestData(db);
        }
    }
}
