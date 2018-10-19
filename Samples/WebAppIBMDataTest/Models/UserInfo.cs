using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace WebAppIBMDataTest.Models
{
    public class UserInfo
    {
        public decimal UserID { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public static UserInfo GetUserInfo(decimal userId)
        {
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand("SEC_GetUser")) 
            {
                db.AddInParameter(cmd, "UserID", DbType.Decimal, userId);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    if (dr.Read())
                    {
                        var userInfo = new UserInfo()
                        {
                            UserID = dr.GetDecimal(0),
                            UserName = dr.GetString(1),
                            Password = dr.GetString(2)
                        };

                        //User Permissions
                        dr.NextResult();
                        while (dr.Read());

                        //User UserInstallations
                        dr.NextResult();
                        while (dr.Read());

                        return userInfo;
                    }
                }

                return null;
            }
        }
    }
}