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
    /// 
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "Db2ConnectionDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "Db2ConnectionDataDisplayName")]
    public class Db2ConnectionData : NamedConfigurationElement
    {

        /// <summary>
        /// 
        /// </summary>
        public Db2ConnectionData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
        [ResourceDescription(typeof(DesignResources), "Db2ConnectionDataNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "Db2ConnectionDataNameDisplayName")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

    }
}
