using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Vietbait.Lablink.Utilities
{
    public static class Common
    {
        /// <summary>
        ///     Hàm trả về connection string mặc định
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultConnectionString()
        {
            try
            {
                string server = LablinkServiceConfig.GetServer();
                string database = LablinkServiceConfig.GetDatabase();
                string userId = LablinkServiceConfig.GetUserId();
                string password = LablinkServiceConfig.GetPassword();
                return string.Format(@"Data Source={0}; Initial Catalog={1}; User ID={2};Password={3}", server, database,
                    userId, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Kiem tra connection
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool CheckConnection(string server, string database, string user, string pass)
        {
            var conn =
                new SqlConnection("Data Source=" + server + ";Initial Catalog=" + database + ";User Id=" + user +
                                  ";Password=" + pass + "");
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool CheckConnection(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        ///     Kiểm tra đầu vào là số hay không
        /// </summary>
        /// <param name="inputValue">Tham số đầu vào</param>
        /// <returns>True: là số, False: không là số </returns>
        public static bool IsItNumber(string inputValue)
        {
            var isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputValue);
        }
    }
}