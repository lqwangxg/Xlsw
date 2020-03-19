using ExcelDna.Integration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                            lstValue.Add(DataTabletoString(dt));
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

            return DataTabletoString(ds.Tables[0]);
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

        private static string DataTabletoString(DataTable dt)
        {
            string header = string.Join("|", dt.Columns.OfType<DataColumn>().Select(x => x.ColumnName));
            List<string> lstTable = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                List<string> lstRow = new List<string>();
                lstRow.Clear();
                foreach (DataColumn col in dt.Columns)
                {
                    if (!row.IsNull(col))
                    {
                        lstRow.Add(row[col].ToString());
                    }
                    else
                    {
                        lstRow.Add(string.Empty);
                    }
                }
                lstTable.Add(string.Join("|", lstRow));
            }
            string datas = string.Join(Environment.NewLine, lstTable);
            return header + Environment.NewLine + datas;
        }

    }
}
