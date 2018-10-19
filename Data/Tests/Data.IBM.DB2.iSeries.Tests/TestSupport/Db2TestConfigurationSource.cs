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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using EntLibContrib.Data.TestSupport;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport
{
    public static class Db2TestConfigurationSource
    {
        public const string Db2ConnectionString = "DataSource=Localhost;UserID=sa;Password=abc123;DefaultCollection=ENTLIBTEST;Connection Timeout=15";
        public const string Db2ConnectionStringName = "Db2Test";
        public const string Db2ProviderName = "IBM.Data.DB2.iSeries";

        public static DictionaryConfigurationSource CreateConfigurationSource()
        {
            DictionaryConfigurationSource configSource = TestConfigurationSource.CreateConfigurationSource();

            var connectionString = new ConnectionStringSettings(
                Db2ConnectionStringName,
                Db2ConnectionString,
                Db2ProviderName);

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(connectionString);

            configSource.Add("connectionStrings", connectionStrings);
            return configSource;
        }
    }
}
