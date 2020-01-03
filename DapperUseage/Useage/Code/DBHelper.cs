using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Useage.Code
{
    public class DBHelper
    {
        #region 批量插入
        public static void BulkInsert(DataTable dt, string targetTableName)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr1"].ConnectionString;
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connStr))
                {
                    bulkCopy.DestinationTableName = targetTableName;
                    bulkCopy.WriteToServer(dt);
                }
            }
        }
        #endregion
    }
}