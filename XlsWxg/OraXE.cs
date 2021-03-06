﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace XlsWxg
{
    internal class OraXE
    {
        public static string Batch(string sql, string dataSource)
        {
            IOWxg.Log(string.Format("sql:{0}", sql));
            IOWxg.Log(string.Format("dataSource:{0}", dataSource));

            List<string> lstValue = new List<string>();
            try
            {
                using (var con = new OracleConnection(dataSource))
                {
                    con.Open();
                    DbTransaction transaction =  con.BeginTransaction();

                    string[] sqls = sql.Split(";".ToCharArray());
                    foreach (var sql1 in sqls)
                    {
                        if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                        {
                            DataTable dt = new DataTable();
                            using (var adapter = new OracleDataAdapter(sql1, con))
                            {
                                adapter.Fill(dt);
                            }
                            lstValue.Add(UtilWxg.DataTabletoString(dt));
                        }
                        else
                        {
                            using (var cmd = new OracleCommand(sql1, con))
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
        
        public static string Query(string sql, string dataSource)
        {
            IOWxg.Log(string.Format("sql:{0}", sql));
            IOWxg.Log(string.Format("dataSource:{0}", dataSource));

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

        public static object GetObject(string sql, string dataSource)
        {
            IOWxg.Log(string.Format("sql:{0}", sql));
            IOWxg.Log(string.Format("dataSource:{0}", dataSource));

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

        public static string Update(string sql, string dataSource)
        {
            IOWxg.Log(string.Format("sql:{0}", sql));
            IOWxg.Log(string.Format("dataSource:{0}", dataSource));

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
