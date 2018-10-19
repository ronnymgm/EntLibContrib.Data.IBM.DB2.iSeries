using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IBM.Data.DB2.iSeries;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Configuration
{
    /// <summary>
    /// Describes a <see cref="Db2Database"/> instance, aggregating information from a 
    /// <see cref="ConnectionStringSettings"/> and any Ibm-specific database information.
    /// </summary>
    public class Db2DatabaseData : DatabaseData
    {
        ///<summary>
        /// Initializes a new instance of the <see cref="Db2DatabaseData"/> class with a connection string and a configuration
        /// source.
        ///</summary>
        ///<param name="connectionStringSettings">The <see cref="ConnectionStringSettings"/> for the represented database.</param>
        ///<param name="configurationSource">The <see cref="IConfigurationSource"/> from which Ibm-specific information 
        /// should be retrieved.</param>
        public Db2DatabaseData(ConnectionStringSettings connectionStringSettings, Func<string, ConfigurationSection> configurationSource)
            : base(connectionStringSettings, configurationSource)
        {

            var settings = (Db2ConnectionSettings)
                           configurationSource(Db2ConnectionSettings.SectionName);

            if (settings != null)
            {
                ConnectionData = settings.Db2ConnectionsData.Get(connectionStringSettings.Name);
            }
        }

        private Db2ConnectionData ConnectionData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Database BuildDatabase()
        {
            return new Db2Database(ConnectionString);
        }
    }
}
