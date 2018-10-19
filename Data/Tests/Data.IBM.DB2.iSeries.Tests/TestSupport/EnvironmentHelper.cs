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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport
{
    internal class EnvironmentHelper
    {
        private static bool? db2ClientIsInstalled;
        private static string db2ClientNotInstalledErrorMessage;

        public static void AssertDb2ClientIsInstalled()
        {
            if (!db2ClientIsInstalled.HasValue)
            {
                try
                {
                    var factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
                    var db = factory.Create("Db2Test");
                    var conn = db.CreateConnection();
                    conn.Open();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message != null && ex.Message.Contains(Db2TestConfigurationSource.Db2ConnectionString))
                    {
                        db2ClientIsInstalled = false;
                        db2ClientNotInstalledErrorMessage = ex.Message;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (db2ClientIsInstalled.HasValue && db2ClientIsInstalled.Value == false)
            {
                Assert.Inconclusive(db2ClientNotInstalledErrorMessage);
            }
        }
    }
}
