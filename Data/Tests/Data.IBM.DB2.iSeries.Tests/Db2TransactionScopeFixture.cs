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
    /// <summary>
    ///		The Db2 client depends on MTS support in order to work with TransactionScope. Details
    ///		are here:
    /// <para>
    ///			http://support.microsoft.com/kb/843044
    /// </para>
    /// <para>
    ///		Also, the default Db2 9.2 installation needs this patch to work correctly with MTS. Without
    ///		this patch, you'll get an error about not being able to load oramts.dll on some computers.
    /// </para>
    /// <para>
    ///			http://www.Db2.com/technology/software/tech/windows/ora_mts/htdocs/utilsoft.html
    /// </para>
    /// <remarks>
    ///		Note: Although you can use TransactionScope with the Db2 client, it will use distributed
    ///		transactions, which generally are not a good idea because of the performance hit.
    /// </remarks>
    /// </summary>
    [TestClass]
   // [Ignore]
    public class Db2TransactionScopeFixture
    {
        TransactionScopeFixture baseFixture;
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("Db2Test");

            try
            {
                DeleteStoredProcedures();
            }
            catch {}
            CreateStoredProcedures();

            baseFixture = new TransactionScopeFixture(db);
            baseFixture.Cleanup();
        }

        [TestCleanup]
        public void Clenaup()
        {
            try
            {
                baseFixture.Cleanup();
                DeleteStoredProcedures();
            }
            catch {}
        }

        [TestMethod]
        public void TransactionScope_ShouldDiscardChangesOnDispose()
        {
            baseFixture.TransactionScope_ShouldDiscardChangesOnDispose();
        }

        //
        // Note: This test is commented out because the Db2 client DOES promote the transaction
        //		 to a distributed transaction.
        //[TestMethod]
        //public void TransactionScope_ShouldNotPromoteToDTC()
        //{
        //    baseFixture.TransactionScope_ShouldNotPromoteToDTC();
        //}

        [TestMethod]
        public void Commit_ShouldKeepChanges()
        {
            baseFixture.Commit_ShouldKeepChanges();
        }

        [TestMethod]
        public void Comit_ShouldKeepInnerChangesForNestedTransaction()
        {
            baseFixture.Comit_ShouldKeepInnerChangesForNestedTransaction();
        }

        [TestMethod]
        public void Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted()
        {
            baseFixture.Complete_ShouldDiscardInnerChangesWhenOuterNotCompleted();
        }

        [TestMethod]
        public void Insert_ShouldAddRowsWhenNoTransactionActive()
        {
            baseFixture.Insert_ShouldAddRowsWhenNoTransactionActive();
        }

        [TestMethod]
        public void ShouldAllowCommandsAfterInnerScopeDisposed()
        {
            baseFixture.ShouldAllowCommandsAfterInnerScopeDisposed();
        }

        // Note: This test is commented
        [TestMethod]
        public void Commit_ShouldDisposeTransactionConnection()
        {
            baseFixture.Commit_ShouldDisposeTransactionConnection();
        }

        [TestMethod]
        public void Rollback_ShouldDisposeTransactionConnection()
        {
            baseFixture.Rollback_ShouldDisposeTransactionConnection();
        }

        [TestMethod]
        public void ExecuteNonQueryWithTextCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithTextCommand_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteNonQueryWithCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithCommand_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction()
        {
            baseFixture.ExecuteNonQueryWithStoredProcedure_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithCommand_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithCommand_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithCommandText_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithCommandText_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithStoredProcedure_ShouldUseTransaction()
        {
            baseFixture.ExecuteScalarWithStoredProcedure_ShouldUseTransaction();
        }

        [TestMethod]
        public void ExecuteDataSetWithCommandText_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithCommandText_ShouldRetriveDataSet();
        }

        [TestMethod]
        public void ExecuteDataSetWithCommand_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithCommand_ShouldRetriveDataSet();
        }

        [TestMethod]
        public void ExecuteDataSetWithStoredProcedure_ShouldRetriveDataSet()
        {
            baseFixture.ExecuteDataSetWithStoredProcedure_ShouldRetriveDataSet();
        }

        [TestMethod]
        public void ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithCommandText_ShouldRetrieveDataInTransaction();
        }

        [TestMethod]
        public void ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithCommand_ShouldRetrieveDataInTransaction();
        }

        //
        // This test won't pass because of a difference between SQL Server and Db2. In order for a
        // stored procedure in Db2 to return a result set, it must return it as an output parameter.
        // However, the ExecuteDataSet overload that takes a stored procedure name fails because the
        // number of parameters passed (zero) doesn't match the number of parameters discovered (one).
        //
        [TestMethod]
        public void ExecuteReaderWithStoredProcedure_ShouldRetrieveDataInTransaction()
        {
            baseFixture.ExecuteReaderWithStoredProcedure_ShouldRetrieveDataInTransaction();
        }

        [TestMethod]
        public void LoadDataSetWithCommandText_LoadsDataInTransaction()
        {
            baseFixture.LoadDataSetWithCommandText_LoadsDataInTransaction();
        }

        [TestMethod]
        public void LoadDataSetWithCommand_LoadsDataInTransaction()
        {
            baseFixture.LoadDataSetWithCommand_LoadsDataInTransaction();
        }

        [TestMethod]
        public void UpdateDataSet_ShouldAddToTransaction()
        {
            baseFixture.UpdateDataSet_ShouldAddToTransaction();
        }

        [TestMethod]
        public void UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction()
        {
            baseFixture.UpdateDataSetWithUpdateBlockSize_ShouldAddToTransaction();
        }

        void CreateStoredProcedures()
        {
            Db2DataSetHelper.CreateStoredProcedures(db);
        }

        void DeleteStoredProcedures()
        {
            Db2DataSetHelper.DeleteStoredProcedures(db);
        }
    }
}
