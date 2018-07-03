using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections;

namespace CountryAPI.Context
{
    public static class CountryDatabase
    {
        private static MySqlConnection connection;
        private static string server = "98.179.199.29";
        private static string database = "world";
        private static string UID = "readOnlyUser";
        private static string password = "readOnlyUser";



        public static void connect()
        {
            string connectionString = "SERVER="+server+";" +
                                "DATABASE="+database+";" +
                                "UID="+UID+";" +
                                "PASSWORD="+password+";";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public static ArrayList getAllCountryCode()
        {
            connect();

            MySqlCommand comm = new MySqlCommand("SELECT Code FROM Country", connection);

            MySqlDataReader reader = comm.ExecuteReader();

            ArrayList codes = new ArrayList();

            while (reader.Read()) {
                codes.Add(reader["Code"]);
            }
            connection.Close();
            return codes;
        }

        public static ArrayList getHighest(int num)
        {
            connect();

            MySqlCommand comm = new MySqlCommand("SELECT Name, Population FROM world.country Order By Population desc LIMIT "+num+";", connection);

            MySqlDataReader reader = comm.ExecuteReader();

            ArrayList codes = new ArrayList();

            while (reader.Read())
            {
                ArrayList tempList = new ArrayList();
                tempList.Add(reader["Name"]);
                tempList.Add(reader["Population"]);
                codes.Add(tempList);
            }
           
            return codes;
        }

        //return cities from provided country
        public static ArrayList getCities(string country) {
            //SELECT world.country.Code, world.country.name, world.city.Name FROM world.country, world.city where world.country.Code = world.city.CountryCode and Code = "AFG";

            connect();

            MySqlCommand comm = new MySqlCommand("SELECT world.country.Code, world.country.name as Country, world.city.Name as City FROM world.country, world.city where world.country.Code = world.city.CountryCode and world.country.Name LIKE '%"+country+"%';", connection);

            MySqlDataReader reader = comm.ExecuteReader();

            ArrayList codes = new ArrayList();

            while (reader.Read())
            {
                ArrayList tempList = new ArrayList();
                tempList.Add(reader["Code"]);
                tempList.Add(reader["Country"]);
                tempList.Add(reader["City"]);
                codes.Add(tempList);
            }

            return codes;
        }

        public static ArrayList getAllCountryCode(string city)
        {
            //SELECT  world.city.Name as city, world.country.Code as code, world.country.Name as Country FROM world.city, world.country where world.city.CountryCode = world.country.Code and world.city.Name LIKE "%New Orleans%"
            connect();

            MySqlCommand comm = new MySqlCommand("SELECT world.city.Name as City, world.city.District as State, world.country.Name as Country FROM world.city, world.country where world.city.CountryCode = world.country.Code and world.city.Name LIKE  '%" + city+"%'; ", connection);

            MySqlDataReader reader = comm.ExecuteReader();

            ArrayList codes = new ArrayList();

            while (reader.Read())
            {
                ArrayList tempList = new ArrayList();
                tempList.Add(reader["City"]);
                tempList.Add(reader["State"]);
                tempList.Add(reader["Country"]);
                codes.Add(tempList);
            }

            return codes;
        }

        public static ArrayList getCountryCodeByName(string search)
        {
            //SELECT  world.city.Name as city, world.country.Code as code, world.country.Name as Country FROM world.city, world.country where world.city.CountryCode = world.country.Code and world.city.Name LIKE "%New Orleans%"
            connect();

            MySqlCommand comm = new MySqlCommand("SELECT * FROM world.country where country.Code LIKE '"+search+"%';", connection);
            //comm.Parameters.Add("@search", MySqlDbType.VarChar).Value = search;
            MySqlDataReader reader = comm.ExecuteReader();

            ArrayList codes = new ArrayList();

            while (reader.Read())
            {
                ArrayList tempList = new ArrayList();
                tempList.Add(reader["Code"]);
                codes.Add(tempList);
            }

            return codes;
        }


        public static Dictionary<string, Dictionary<string,string>> Login(string user, string pass)
        {
            //SELECT * FROM world.users WHERE world.users.Username = 'nmeadows' AND world.users.Password = 'testPass';

            connect();

            MySqlCommand comm = new MySqlCommand("SELECT * FROM world.users WHERE world.users.Username = '"+user+"' AND world.users.Password = '"+pass+"'", connection);

            MySqlDataReader reader = comm.ExecuteReader();

            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();

            while (reader.Read())
            {
                Dictionary<string, string> tempDictionary = new Dictionary<string, string>();


                tempDictionary.Add("Password", reader["Password"].ToString());

                if(reader["isLoggedIn"].ToString() == "0")
                    tempDictionary.Add("isLoggedIn", "false");
                else
                    tempDictionary.Add("isLoggedIn", "true");



                result.Add(reader["Username"].ToString(), tempDictionary);
            }

            if (result.Count() == 0)
            {
                Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
                tempDictionary.Add("No User", "No User exists whith those credintals");

                result.Add("Error", tempDictionary);
            }

            return result;
        }

        
    }
}
