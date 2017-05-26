using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CouldProjectAzureV2
{
    public class DatabaseConnector
    {
        private String connectionString = "Server=tcp:cloudprojectserver.database.windows.net,1433;Initial Catalog=cloudDatabase;Persist Security Info=False;User ID=has;Password=Pta7skr23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
                            userSettings = new UserSettings( reader.GetSqlValue(0).ToString(), Int32.Parse( reader.GetSqlValue(1).ToString()), Int32.Parse( reader.GetSqlValue(2).ToString()), Int32.Parse( reader.GetSqlValue(3).ToString()), Int32.Parse( reader.GetSqlValue(4).ToString()));
                        }
                        reader.Close();
                    }
                }
                con.Close();
                return userSettings;
            }
        }

        //Anropa vid registrering
        public void setUserSettings(UserSettings userSettings)
        {
            executeSQLCommand("UPDATE UserSensorSettings SET userId = '" + userSettings.getUserId() + "', accelerometerOnOff = " + userSettings.getAcceleroMeterOnoff() + ", proximityOnOff = '" + userSettings.getProximityOnoff() + ", lightOnOff = " + userSettings.getLightOnOff() + ", samplingRate = " + userSettings.getSamplingRate());
        }

        public void setSettingForUser(String userId, String collumName, int value)
        {
            executeSQLCommand("UPDATE UserSensorSettings SET " + collumName + "=" + value + " WHERE userId = " + userId);
        }

    }
}