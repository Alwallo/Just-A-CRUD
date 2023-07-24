using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DataAccess.Repositories
{
    public abstract class MasterRepository : Repository
    {
        protected List<SqlParameter> parameters;

        protected int ExecuteNonQuery (string transactSql)
        {
            using(var conn = GetConnection())
            {
                conn.Open ();
                using(var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = transactSql;
                    cmd.CommandType = CommandType.Text;
                    foreach(var param in parameters)
                    {
                        cmd.Parameters.Add (param);
                    }
                    int result = cmd.ExecuteNonQuery ();
                    parameters.Clear ();
                    return result;
                }
            }
        }

        protected DataTable ExecuteReader(string transactSql, List<string> attributes = null, List<dynamic> values = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = transactSql;
                    if (attributes != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach(var attr in attributes)
                        {
                            cmd.Parameters.AddWithValue(attr, values[attributes.IndexOf(attr)]);
                        }
                    }
                    else
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (var table = new DataTable())
                    {
                        table.Load (reader);
                        reader.Dispose();
                        cmd.Parameters.Clear();
                        return table;
                    }
                }
            }
        }
    }
}
