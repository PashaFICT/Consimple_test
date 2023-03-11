using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Consimple
{
    public class Utils
    {
        //internal static bool CheckUser(string login, string pass)
        //{
        //    bool res = false;
        //    string password = EncryptPassword(login, pass);

        //    string sql = $"select * from Users where Login=@Login and Password =@Password";
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("@Login", login));
        //    parameters.Add(new SqlParameter("@Password", password));
        //    if (DBWorker.ExecQueryWithParameters(sql, parameters).Rows.Count != 0)
        //    {
        //        res = true;
        //    }

        //    return res;
        //}

        //private static string EncryptPassword(string userName, string password)
        //{
        //    string tmpPassword = userName.ToLower() + password;

        //    UTF8Encoding textConverter = new UTF8Encoding();
        //    byte[] passBytes = textConverter.GetBytes(tmpPassword);

        //    string res = Convert.ToBase64String(new SHA384Managed().ComputeHash(passBytes));
        //    return res;
        //}

        public static void SaveError(string methodName, Exception ex, long userID, string login)
        {
            using (SqlConnection connection = new SqlConnection(@"Server=localhost\MSSQLSERVER01;Database=Consimple;Trusted_Connection=True;"))
            {
                connection.Open();
                string query = "insert into ErrorMessages(ErrorMessage,Login,UserID) values(@ErrorMessage, @Login,@UserID)";
                SqlCommand com = new SqlCommand(query, connection);
                com.Parameters.AddWithValue("@ErrorMessage", string.Format("{0}: {1}", methodName, ex.Message));
                com.Parameters.AddWithValue("@Login", login == null ? string.Empty : login);
                com.Parameters.AddWithValue("@UserID", userID);
                com.ExecuteNonQuery();
            }
        }
    }
}
