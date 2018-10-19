using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebAppIBMDataTest.Models
{
    public class CustomerInfo
    {
        public string CustID { get; set; }
        public string CustName { get; set; }

        public string Country { get; set; }

        public static CustomerInfo GetCustomerInfo(string custId)
        {
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand("GetCustomerTest"))
            {
                db.AddInParameter(cmd, "vCustId", DbType.String, custId);
                db.AddInParameter(cmd, "vTest", DbType.String, "");

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    if (dr.Read())
                    {
                        return new CustomerInfo()
                        {
                            CustID = dr.GetString(0),
                            CustName = dr.GetString(2).TrimEnd(),
                            Country = dr.GetString(8)
                        };
                    }
                }

                return null;
            }
        }
    }
}