using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouldProjectAzureV2
{
    public class DataStorage
    {
        private List<Entity> sensorData;
        private string selectedUser;

        private static DataStorage ourInstance = new DataStorage();

        public static DataStorage getInstance()
        {
            return ourInstance;
        }

        public void setSensorData(List<Entity> sensorData)
        {
         
            this.sensorData = sensorData;
        }
        public List<Entity> getSensorData()
        {
            List<Entity> sensorDataTemp = sensorData;
            return sensorData;
        }
        public string getSelectedUser()
        {
            return selectedUser;
        }
        public void setSelectedUser(string selectedUser)
        {
            this.selectedUser = selectedUser;
        }
    }
}