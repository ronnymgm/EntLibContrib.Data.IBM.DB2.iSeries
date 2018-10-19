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
using System.Data.Common;
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EntLibContrib.Data.TestSupport;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    /// <summary>
    /// Use the Data Access Application Block to execute a create a stored procedure script using ExecNonQuery.
    /// </summary>
    [TestClass]
    public class Db2StoredProcedureCreatingFixture : StoredProcedureCreationBase
    {
        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            var factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("Db2Test");
            CompleteSetup(db);
        }

        [TestCleanup]
        public void TearDown()
        {
            Cleanup();
        }

        protected override void CreateStoredProcedure()
        {
            string storedProcedureCreation =
@"CREATE Procedure ENTLIBTEST.TestProc
(	
	INOUT vCount	NUMERIC(5,0),
	IN vCustomerId	VARCHAR(5)
)
DYNAMIC RESULT SETS 1 
LANGUAGE SQL 
NOT DETERMINISTIC 
CALLED ON NULL INPUT
SET OPTION COMMIT = *RR
BEGIN 
	SET vCount = (SELECT COUNT(*) FROM ENTLIBTEST.Orders WHERE CustomerId = vCustomerId);
END";

            DbCommand command = db.GetSqlStringCommand(storedProcedureCreation);
            db.ExecuteNonQuery(command);
        }

        protected override void DeleteStoredProcedure()
        {
            string storedProcedureDeletion = "DROP SPECIFIC PROCEDURE ENTLIBTEST.TestProc";
            DbCommand command = db.GetSqlStringCommand(storedProcedureDeletion);
            db.ExecuteNonQuery(command);
        }

        [TestMethod]
        public void CanGetOutputValueFromStoredProcedure()
        {
            baseFixture.CanGetOutputValueFromStoredProcedure();
        }

        [TestMethod]
        public void CanGetOutputValueFromStoredProcedureWithCachedParameters()
        {
            baseFixture.CanGetOutputValueFromStoredProcedureWithCachedParameters();
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void ArgumentExceptionWhenThereAreTooFewParameters()
        {
            baseFixture.ArgumentExceptionWhenThereAreTooFewParameters();
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void ArgumentExceptionWhenThereAreTooManyParameters()
        {
            baseFixture.ArgumentExceptionWhenThereAreTooFewParameters();
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues()
        {
            baseFixture.ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues();
        }
    }
}
