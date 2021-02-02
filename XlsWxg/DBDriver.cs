using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsWxg
{
    public enum DBType 
    {
        Oracle = 1,
        SqlServer
    }

    public class DBDriver
    {
        private static string ToSQLConnectString(string dataSource)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            
            builder.DataSource = UtilWxg.GetMatchGroup(dataSource, @"host=(\w)+", 1);   // 接続先の SQL Server インスタンス
            builder.UserID = UtilWxg.GetMatchGroup(dataSource, @"User ID=(\w)+", 1);    // 接続ユーザー名
            builder.Password = UtilWxg.GetMatchGroup(dataSource, @"Password=(\w)+", 1); // 接続パスワード
            builder.InitialCatalog = UtilWxg.GetMatchGroup(dataSource, @"SERVICE_NAME=(\w)+", 1); ;  // 接続するデータベース(ここは変えないでください)
                          // builder.ConnectTimeout = 60000;  // 接続タイムアウトの秒数(ms) デフォルトは 15 秒
            return builder.ConnectionString;
        }
        [ExcelFunction(Category = "Database", Description = "Select By SQL, Return Data Arrays.")]
        public static string Batch(
            [ExcelArgument(Description = "ex:oracle, sqlserver", Name = "dbType")] string dbType,
            [ExcelArgument(Description = "select cols from tables where ...", Name = "sql")] string sql,
            [ExcelArgument(Description = "connectionstring of dbtype", Name = "dataSource")] string dataSource)
        {
            if ("oracle".Equals(dbType, StringComparison.OrdinalIgnoreCase)) 
            {
                return OraXE.Batch(sql, dataSource);
            }
            if ("sqlserver".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                dataSource = ToSQLConnectString(dataSource);
                return SQLServerDriver.Batch(sql, dataSource);
            }

            return string.Format("unknown DBtype:[{0}]", dbType);
        }

        [ExcelFunction(Category = "Database", Description = "Select By SQL, Return Data Arrays.")]
        public static string Query(
            [ExcelArgument(Description = "ex:oracle, sqlserver", Name = "dbType")] string dbType,
            [ExcelArgument(Description = "select cols from tables where ...", Name = "sql")] string sql,
            [ExcelArgument(Description = "connectionstring of dbtype", Name = "dataSource")] string dataSource)
        {
            if ("oracle".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                return OraXE.Query(sql, dataSource);
            }
            if ("sqlserver".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                dataSource = ToSQLConnectString(dataSource);
                return SQLServerDriver.Query(sql, dataSource);
            }

            return string.Format("unknown DBtype:[{0}]", dbType);
        }

        [ExcelFunction(Category = "Database", Description = "Select  one object by ExecuteScalar, Return object value.")]
        public static object GetObject(
            [ExcelArgument(Description = "ex:oracle, sqlserver", Name = "dbType")] string dbType,
            [ExcelArgument(Description = "select cols from tables where ...", Name = "sql")] string sql,
            [ExcelArgument(Description = "connectionstring of dbtype", Name = "dataSource")] string dataSource)
        {
            if ("oracle".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                return OraXE.GetObject(sql, dataSource);
            }
            if ("sqlserver".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                dataSource = ToSQLConnectString(dataSource);
                return SQLServerDriver.GetObject(sql, dataSource);
            }

            return string.Format("unknown DBtype:[{0}]", dbType);
        }

        [ExcelFunction(Category = "Database", Description = "Insert Update or Delete by ExecuteNoQuery, Return int.")]
        public static string Update(
            [ExcelArgument(Description = "ex:oracle, sqlserver", Name = "dbType")] string dbType, 
            [ExcelArgument(Description = "select cols from tables where ...", Name = "sql")] string sql,
            [ExcelArgument(Description = "connectionstring of dbtype", Name = "dataSource")] string dataSource)
        {
            if ("oracle".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                return OraXE.Update(sql, dataSource);
            }
            if ("sqlserver".Equals(dbType, StringComparison.OrdinalIgnoreCase))
            {
                dataSource = ToSQLConnectString(dataSource);
                return SQLServerDriver.Update(sql, dataSource);
            }

            return string.Format("unknown DBtype:[{0}]", dbType);
        }

    }
}
