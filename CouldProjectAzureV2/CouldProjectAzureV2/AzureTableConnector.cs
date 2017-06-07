using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;
using Microsoft.Azure;

namespace CouldProjectAzureV2
{
    //Class for connecting to Azure tablestorage
    public class AzureTableConnector
    {
        //Method for retrieving the Entitys as a list
        public List<Entity> RetriveDataFromSensors(String tableName, String calenderDate, String nextDay, String username)
        {
            List<Entity> sensorDataEntityList = new List<Entity>();
            try
            {            
                string accountName = "hkrtest"; //cloud
                string accountKey = "xMmOQjMFbLY6R5cHcfUAQjZXRRp50eLTiFspybB929IGYsBnuVbCME/6bcxejT2kd3rEJLBBfcQXi8e0TLfPbg==";//cloud
                StorageCredentials credemtials = new StorageCredentials(accountName, accountKey);   //cloud
                CloudStorageAccount account = new CloudStorageAccount(credemtials, useHttps: true); //cloud

                CloudTableClient client = account.CreateCloudTableClient();
                CloudTable table = client.GetTableReference(tableName);

                Boolean onDate = false;
                TableQuery<Entity> query = new TableQuery<Entity>();

                foreach (Entity entity in table.ExecuteQuery(query))
                {
                      Debug.WriteLine(entity.PartitionKey);
                      Debug.WriteLine(entity.RowKey);
                      Debug.WriteLine(entity.SensorAccelerometerX);
                      Debug.WriteLine(entity.SensorAccelerometerY);
                      Debug.WriteLine(entity.SensorAccelerometerZ);
                      Debug.WriteLine(entity.SensorLight);
                      Debug.WriteLine(entity.SensorProximity);
                      Debug.WriteLine("----------------------------------");
                     
                 
                    string[] values = entity.RowKey.Split(';');
                    string userNameAzure = values[0];
                    string fullDate = values[1];

                    if ((fullDate.StartsWith(calenderDate) || fullDate.StartsWith(nextDay)) && username.Equals(userNameAzure))
                    {
                        onDate = true;
                        sensorDataEntityList.Add(entity);
                    }
                    else if (!(fullDate.StartsWith(calenderDate) || fullDate.StartsWith(nextDay)) && onDate)
                    {
                        break;
                    }           
                 
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR");
                Debug.WriteLine(ex.Message);

            }
            return sensorDataEntityList;
        }
    }
 
}