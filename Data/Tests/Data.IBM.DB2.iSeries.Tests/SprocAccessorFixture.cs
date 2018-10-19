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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IBM.Data.DB2.iSeries;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
#pragma warning disable 612, 618
    [TestClass]
    public class WhenSprocAccessorIsCreatedForDb2Database : ArrangeActAssert
    {
        Db2Database database;

        protected override void Arrange()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            database = factory.Create("Db2Test") as Db2Database;
        }

        [TestMethod]
        public void ThenCanExecuteParameterizedSproc()
        {
            var accessor = database.CreateDb2SprocAccessor<Customer>("GetCustomerTest");
            var result = accessor.Execute("BLAUS", null).ToArray();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        private class Customer
        {
            public string ContactName { get; set; }
        }
    }
#pragma warning restore 612, 618
}
