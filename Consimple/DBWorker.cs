using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Consimple
{
    public class DBWorker
    {
        private static string connectionString = @"Server=localhost\MSSQLSERVER01;Database=Consimple;Trusted_Connection=True;TrustServerCertificate=True";

        public static DataTable ExecQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
        public static DataTable ExecQueryWithParameters(string query, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
    }
}
