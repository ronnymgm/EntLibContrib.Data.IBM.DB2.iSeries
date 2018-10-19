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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntLibContrib.Data.IBM.DB2.iSeries.Configuration;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    /// <summary>
    /// Summary description for Db2DatabaseDataExtraInformationFixture
    /// </summary>
    [TestClass]
    public class Db2DatabaseDataExtraInformationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            Db2ConnectionSettings rwSettings = new Db2ConnectionSettings();

            Db2ConnectionData rwDb2ConnectionData = new Db2ConnectionData();
            rwDb2ConnectionData.Name = "name0";
            rwSettings.Db2ConnectionsData.Add(rwDb2ConnectionData);
            rwDb2ConnectionData = new Db2ConnectionData();
            rwDb2ConnectionData.Name = "name1";
            rwSettings.Db2ConnectionsData.Add(rwDb2ConnectionData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[Db2ConnectionSettings.SectionName] = rwSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            Db2ConnectionSettings roSettings = (Db2ConnectionSettings)configurationSource.GetSection(Db2ConnectionSettings.SectionName);
            Assert.AreEqual(2, roSettings.Db2ConnectionsData.Count);
            Assert.AreEqual("name0", roSettings.Db2ConnectionsData.Get(0).Name);
        }
    }
}
