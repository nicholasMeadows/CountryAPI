using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace CountryAPI.Context
{
    public static class OAuth2Server
    {
        private static MySqlConnection connection;
        private static string server = "98.179.199.29";
        private static string database = "oauth2";
        private static string UID = "oauth2User";
        private static string dbPassword = "oauth2User";
        private static string connectionString = "SERVER=" + server + ";" +
                                "DATABASE=" + database + ";" +
                                "UID=" + UID + ";" +
                                "PASSWORD=" + dbPassword + ";";


        public static string ValidateAccessToken(string accessToken)
        {
            if (accessToken == null)
            {
                return "Missing access_token";
            }

            accessToken = accessToken.Substring(7);

            connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand accessQuery = new MySqlCommand("SELECT * FROM `oauth2`.`access_tokens` WHERE access_token = @accesstoken;", connection);
            accessQuery.Parameters.Add("@accesstoken", MySqlDbType.VarChar).Value = accessToken;
            MySqlDataReader reader = accessQuery.ExecuteReader();
            reader.Read();

            if (reader.HasRows)
            {
                DateTime timestamp = (DateTime)reader["timestamp"];
                DateTime now = DateTime.Now;
                TimeSpan span = now.Subtract(timestamp);

                if (span.Hours >= 1)
                {
                    return "Expired access_token";
                }
                return "Valid";
            }
            else {
                return "Invalid access";
            }
        }
    }
}
