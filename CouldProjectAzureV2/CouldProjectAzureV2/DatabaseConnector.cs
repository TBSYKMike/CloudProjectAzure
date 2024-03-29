﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CouldProjectAzureV2
{
    //This class is used for connection to the SQL database. It is only for the added UserSensorSettings table. Other tables are altered with "mvc methods"
    public class DatabaseConnector
    {
        private String connectionString = "Server=tcp:cloudprojectserver.database.windows.net,1433;Initial Catalog=cloudDatabase;Persist Security Info=False;User ID=has;Password=Pta7skr23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //Called when executing a sql command
        private void executeSQLCommand(String commandText)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommmand = new SqlCommand();
            sqlCommmand.CommandType = System.Data.CommandType.Text;
            sqlCommmand.CommandText = commandText;
            sqlCommmand.Connection = sqlConnection;

            sqlConnection.Open();
            sqlCommmand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        //For retrieving all the sensor settings of a user
        public UserSettings getUserSettings(String userId)
        {
            UserSettings userSettings = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserSensorSettings WHERE userId = '" + userId + "'", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userSettings = new UserSettings(reader.GetSqlValue(0).ToString(), Int32.Parse( reader.GetSqlValue(1).ToString()), Int32.Parse( reader.GetSqlValue(2).ToString()), Int32.Parse( reader.GetSqlValue(3).ToString()), Int32.Parse( reader.GetSqlValue(4).ToString()));
                        }
                        reader.Close();
                    }
                }
                con.Close();
                return userSettings;
            }
        }

        //Initializing a users sensor settings
        public void setUserSettings(UserSettings userSettings)
        {
            executeSQLCommand("INSERT INTO UserSensorSettings VALUES ('"+userSettings.getUserId()+"',"+ userSettings.getAcceleroMeterOnoff()+","+userSettings.getProximityOnoff()+","+userSettings.getLightOnOff()+","+userSettings.getSamplingRate()+")");
        }

        //Changing a certain user sensor setting
        public void setSettingForUser(String userId, String collumName, int value)
        {
            executeSQLCommand("UPDATE UserSensorSettings SET " + collumName + "=" + value + " WHERE userId = '" + userId +"'");
        }

    }
}