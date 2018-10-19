using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Configuration
{
    /// <summary>
	/// Db2-specific configuration section.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "Db2ConnectionSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "Db2ConnectionSettingsDisplayName")]
    public class Db2ConnectionSettings : SerializableConfigurationSection
    {
        private const string Db2ConnectionDataCollectionProperty = "";

        /// <summary>
        /// The section name for the <see cref="Db2ConnectionSettings"/>.
        /// </summary>
        public const string SectionName = "db2ConnectionSettings";

        /// <summary>
        /// Initializes a new instance of the <see cref="Db2ConnectionSettings"/> class with default values.
        /// </summary>
        public Db2ConnectionSettings()
        {
        }

        /// <summary>
        /// Retrieves the <see cref="Db2ConnectionSettings"/> from the configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source to retrieve the configuration from.</param>
        /// <returns>The configuration section, or <see langword="null"/> (<b>Nothing</b> in Visual Basic) 
        /// if not present in the configuration source.</returns>
        public static Db2ConnectionSettings GetSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");

            return configurationSource.GetSection(SectionName) as Db2ConnectionSettings;
        }

        /// <summary>
        /// Collection of IBM-specific connection information.
        /// </summary>
        [ConfigurationProperty(Db2ConnectionDataCollectionProperty, IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(Db2ConnectionData))]
        [ResourceDescription(typeof(DesignResources), "Db2ConnectionSettingsDb2ConnectionsDataDescription")]
        [ResourceDisplayName(typeof(DesignResources), "Db2ConnectionSettingsDb2ConnectionsDataDisplayName")]
        public NamedElementCollection<Db2ConnectionData> Db2ConnectionsData
        {
            get { return (NamedElementCollection<Db2ConnectionData>)base[Db2ConnectionDataCollectionProperty]; }
        }
    }
}
