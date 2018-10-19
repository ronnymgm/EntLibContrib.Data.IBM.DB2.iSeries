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


using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    internal sealed class Db2DataSetHelper
    {
        public static void CreateDataAdapterCommandsDynamically(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            insertCommand = db.GetStoredProcCommandWithSourceColumns("RegionInsert", "RegionID", "RegionDescription");
            updateCommand = db.GetStoredProcCommandWithSourceColumns("RegionUpdate", "RegionID", "RegionDescription");
            deleteCommand = db.GetStoredProcCommandWithSourceColumns("RegionDelete", "RegionID");
        }

        public static void CreateDataAdapterCommands(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            insertCommand = db.GetStoredProcCommand("RegionInsert");
            updateCommand = db.GetStoredProcCommand("RegionUpdate");
            deleteCommand = db.GetStoredProcCommand("RegionDelete");

            db.AddInParameter(insertCommand, "vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(insertCommand, "vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            db.AddInParameter(updateCommand, "vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            db.AddInParameter(deleteCommand, "vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }

        public static void CreateStoredProcedures(Database db)
        {
            DbCommand command;
            string sql;

            //try
            //{
            //    DeleteStoredProcedures(db);
            //}
            //catch { }

            //Create Select SP
            sql =
@"CREATE Procedure ENTLIBTEST.RegionSelect()
DYNAMIC RESULT SETS 1
LANGUAGE SQL
NOT DETERMINISTIC
CALLED ON NULL INPUT
SET OPTION COMMIT = *RR
BEGIN
    DECLARE TEMP_CURSOR1 CURSOR WITHOUT HOLD WITH RETURN TO CLIENT FOR

    SELECT * FROM ENTLIBTEST.REGION
    ORDER BY RegionID;

    OPEN TEMP_CURSOR1;
END";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);

            //Create Insert SP
            sql =
@"CREATE Procedure ENTLIBTEST.RegionInsert
(	
	vRegionId	NUMERIC(5,0),
	vRegionDescription	VARCHAR(36)
)
LANGUAGE SQL 
NOT DETERMINISTIC 
MODIFIES SQL DATA 
CALLED ON NULL INPUT 
SET OPTION COMMIT = *RR
BEGIN 
	INSERT INTO ENTLIBTEST.REGION
	(
		RegionId,RegionDescription
	)
	VALUES
	(
		vRegionId,vRegionDescription
	);
END";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);

            //Create Update SP
            sql =
@"CREATE Procedure ENTLIBTEST.RegionUpdate
(	
	vRegionId	NUMERIC(5,0),
	vRegionDescription	VARCHAR(36)
)
LANGUAGE SQL 
NOT DETERMINISTIC 
MODIFIES SQL DATA 
CALLED ON NULL INPUT 
SET OPTION COMMIT = *RR
BEGIN 
	UPDATE ENTLIBTEST.REGION
	SET RegionDescription = vRegionDescription
	WHERE RegionId = vRegionId;
END";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);

            //Create Delete SP
            sql =
@"CREATE Procedure ENTLIBTEST.RegionDelete
(	
	vRegionId	NUMERIC(5,0)
)
LANGUAGE SQL 
NOT DETERMINISTIC 
MODIFIES SQL DATA 
CALLED ON NULL INPUT 
SET OPTION COMMIT = *RR
BEGIN 
	DELETE ENTLIBTEST.REGION
	WHERE REGIONID = vRegionId;
END";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
        }

        public static void DeleteStoredProcedures(Database db)
        {
            DbCommand command;
            string sql = "DROP SPECIFIC PROCEDURE ENTLIBTEST.RegionSelect";
            command = db.GetSqlStringCommand(sql);
            try { db.ExecuteNonQuery(command); }
            catch { }
            sql = "DROP SPECIFIC PROCEDURE ENTLIBTEST.RegionInsert";
            command = db.GetSqlStringCommand(sql);
            try { db.ExecuteNonQuery(command); }
            catch { }
            sql = "DROP SPECIFIC PROCEDURE ENTLIBTEST.RegionDelete";
            command = db.GetSqlStringCommand(sql);
            try { db.ExecuteNonQuery(command); }
            catch { }
            sql = "DROP SPECIFIC PROCEDURE ENTLIBTEST.RegionUpdate";
            command = db.GetSqlStringCommand(sql);
            try { db.ExecuteNonQuery(command); }
            catch { }
        }

        public static void AddTestData(Database db)
        {
            string sql =
                "BEGIN " +
                    "insert into REGION values (99, 'Midwest');" +
                    "insert into REGION values (100, 'Central Europe');" +
                    "insert into REGION values (101, 'Middle East');" +
                    "insert into REGION values (102, 'Australia');" +
                "END";
            DbCommand testDataInsertion = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(testDataInsertion);
        }
    }
}

