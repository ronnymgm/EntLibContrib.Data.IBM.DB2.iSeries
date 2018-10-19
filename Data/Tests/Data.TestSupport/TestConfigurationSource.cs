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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using EntLibContrib.Data.IBM.DB2.iSeries.Configuration;

namespace EntLibContrib.Data.TestSupport
{
    public class TestConfigurationSource
    {
        public const string NorthwindDummyUser = "system";
        public const string NorthwindDummyPassword = "admin";

        public static DictionaryConfigurationSource CreateConfigurationSource()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();

            DatabaseSettings settings = new DatabaseSettings();
            settings.DefaultDatabase = "Db2Test";
            settings.ProviderMappings.Add(new DbProviderMapping("IBM.Data.DB2.iSeries", "EntLibContrib.Data.IBM.DB2.iSeries.Db2Database, EntLibContrib.Data.IBM.DB2.iSeries, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null"));

            Db2ConnectionSettings db2ConnectionSettings = new Db2ConnectionSettings();
            Db2ConnectionData data = new Db2ConnectionData();
            db2ConnectionSettings.Db2ConnectionsData.Add(data);

            source.Add(DatabaseSettings.SectionName, settings);
            source.Add(Db2ConnectionSettings.SectionName, db2ConnectionSettings);

            return source;
        }
    }
}
