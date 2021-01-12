using ExcelDna.Integration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace XlsWxg
{
    public class OraXE
    {
        [ExcelFunction(Category = "Database", Description = "Select By SQL, Return Data Arrays.")]
        public static string Batch([ExcelArgument(Description = "select cols from tables where ...", Name = "sql")]string batchSQL, string dataSource)
        {
            List<string> lstValue = new List<string>();
            try
            {
                using (var con = new OracleConnection(dataSource))
                {
                    con.Open();
                    DbTransaction transaction =  con.BeginTransaction();

                    string[] sqls = batchSQL.Split(";".ToCharArray());
                    foreach (var sql in sqls)
                    {
                        if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                        {
                            DataTable dt = new DataTable();
                            using (var adapter = new OracleDataAdapter(sql, con))
                            {
                                adapter.Fill(dt);
                            }
                            lstValue.Add(UtilWxg.DataTabletoString(dt));
                        }
                        else
                        {
                            using (var cmd = new OracleCommand(sql, con))
                            {
                                int count = cmd.ExecuteNonQuery();
                                lstValue.Add(count.ToString());
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                IOWxg.Log(ex.Message);
                return "error:" + ex.Message;
            }
            return string.Join(";", lstValue);
        }
        
        [ExcelFunction(Category = "Database", Description = "Select By SQL, Return Data Arrays.")]
        public static string Query([ExcelArgument(Description = "select cols from tables where ...", Name = "sql")]string sql, string dataSource)
        {
            DataSet ds = new DataSet();
            try
            {
                using (var adapter = new OracleDataAdapter(sql, dataSource))
                {
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex) 
            {
                IOWxg.Log(ex.StackTrace);
                return "error:" + ex.Message;
            }
            if (ds.Tables.Count == 0) return string.Empty;

            return UtilWxg.DataTabletoString(ds.Tables[0]);
        }

        [ExcelFunction(Category = "Database", Description = "Select  one object by ExecuteScalar, Return object value.")]
        public static object GetObject([ExcelArgument(Description = "select cols from tables where ...", Name = "sql")]string sql, string dataSource)
        {
            object retValue;
            try
            {
                using (var con = new OracleConnection(dataSource))
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand(sql, con);
                    retValue = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                retValue = "error:" + ex.Message;
                IOWxg.Log(ex.Message);
            }            
            return retValue;
        }

        [ExcelFunction(Category = "Database", Description = "Insert Update or Delete by ExecuteNoQuery, Return int.")]
        public static string Update([ExcelArgument(Description = "select cols from tables where ...", Name = "sql")]string sql, string dataSource)
        {
             
            string retValue;
            try
            {
                using (var con = new OracleConnection(dataSource))
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand(sql, con);
                    int count = cmd.ExecuteNonQuery();
                    retValue = count.ToString();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                retValue = "error:"+ ex.Message;
                IOWxg.Log(ex.StackTrace);
            }            
            return retValue;
        }

    }
}
