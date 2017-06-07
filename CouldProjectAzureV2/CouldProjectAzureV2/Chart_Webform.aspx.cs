using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;

namespace CouldProjectAzureV2
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Speech.Recognition;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    /*This class is for the chart presentation page*/
    public partial class Chart_Webform : System.Web.UI.Page
    {
        private AzureTableConnector azureTableConnector = new AzureTableConnector();
        private List<Entity> sensorData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateUserDropDownList();
            }
        }

        //To insert all patients in the dropdownlist
        private void populateUserDropDownList()
        {
            var context = new IdentityDbContext();
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext()));
            var users = rm.FindByName("patient").Users.Select(x => x.UserId);
            var usersInRole = context.Users.Where(u => users.Contains(u.Id)).ToList();

            for (int i = 0; i < usersInRole.Count; i++)
            {
                Debug.WriteLine("patient: " + usersInRole[i].UserName);
                UserList.Items.Add(usersInRole[i].UserName);
            }
            if (usersInRole != null)
                ViewState["UserListSelectedUser"] = usersInRole[0].UserName; // Ändra till usersInRole[0].UserName;
        }

        //To insert all different measurements in the dropdownlist
        private void populateMeasurementList()
        {
            for (int i = 0; i < sensorData.Count; i++)
            {
                if (sensorData[i].METAData != null)
                {
                    if (sensorData[i].METAData.Equals("Measurement START"))     //Only inserts if found a start of a measurement
                    {
                        string[] rowKeyValues = sensorData[i].RowKey.Split(';');
                        MeasurementList.Items.Add(rowKeyValues[1]);
                    }
                }
            }
        }

        //Called when creating a sensor chart with one serie (all except accelerometer)
        private void createOneSerieGraph(string chartType, List<Entity> sensorListParameter)
        {
            string serie = "";
            Table1.Rows.Add(new TableRow());
            if (chartType.Equals("LightChart"))
            {
                serie = "lightSensor";
                LightChart.Series.Add(serie);
                LightChart.Series[serie].ChartType = SeriesChartType.FastLine;
            }
            else if (chartType.Equals("ProximityChart"))
            {
                serie = "proximitySensor";
                ProximityChart.Series.Add(serie);
                ProximityChart.Series[serie].ChartType = SeriesChartType.FastLine;
            }
            else if (chartType.Equals("BatteryChart"))
            {
                serie = "batterySensor";
                BatteryChart.Series.Add(serie);
                BatteryChart.Series[serie].ChartType = SeriesChartType.FastLine;
            }

            //Loop through the enteties and add them to the correct graph
            for (int i = 0; i < sensorListParameter.Count(); i++)
            {
                if (sensorListParameter[i].SensorLight != null && chartType.Equals("LightChart"))
                {
                    LightChart.Series[serie].Points.Add(double.Parse(sensorListParameter[i].SensorLight, CultureInfo.InvariantCulture));
                }
                else if (sensorListParameter[i].SensorProximity != null && chartType.Equals("ProximityChart"))
                {
                    ProximityChart.Series[serie].Points.Add(double.Parse(sensorListParameter[i].SensorProximity, CultureInfo.InvariantCulture));
                }
                else if (sensorListParameter[i].BatteryLevel != null && chartType.Equals("BatteryChart"))
                {
                    BatteryChart.Series[serie].Points.Add(double.Parse(sensorListParameter[i].BatteryLevel, CultureInfo.InvariantCulture));
                }
                else if (sensorListParameter[i].METAData != null && chartType.Equals("METADATATable"))
                {

                    string[] stringSplit = sensorListParameter[i].RowKey.Split(';');

                    TableRow tRow = new TableRow();
                    Table1.Rows.Add(tRow);

                    // Create a new cell and then add the cell to the row.
                    TableCell tCell = new TableCell();
                    tCell.Text = "Time " + stringSplit[1] + ", Metadata: " + sensorListParameter[i].METAData;
                    tRow.Cells.Add(tCell);

                }
            }
        }

        //Called when creating a one serie chart
        private void createAccelerometerGraph(List<Entity> sensorListParameter)
        {
            string xSerie = "x";
            string ySerie = "y";
            string zSerie = "z";

            addAcceleroMeterDataToGraph(xSerie);
            addAcceleroMeterDataToGraph(ySerie);
            addAcceleroMeterDataToGraph(zSerie);

            AcclerometerChart.Series[0].Color = System.Drawing.Color.Green;
            AcclerometerChart.Series[1].Color = System.Drawing.Color.Black;
            AcclerometerChart.Series[2].Color = System.Drawing.Color.CornflowerBlue;
        
            //Loop through the enteties and add the accelerometer data to the graph
            for (int i = 0; i < sensorListParameter.Count(); i++)
            {
                if (sensorListParameter[i].SensorAccelerometerX != null && sensorListParameter[i].SensorAccelerometerY != null && sensorListParameter[i].SensorAccelerometerZ != null)
                {
                    AcclerometerChart.Series[xSerie].Points.Add(double.Parse(sensorListParameter[i].SensorAccelerometerX, CultureInfo.InvariantCulture));
                    AcclerometerChart.Series[ySerie].Points.Add(double.Parse(sensorListParameter[i].SensorAccelerometerY, CultureInfo.InvariantCulture));
                    AcclerometerChart.Series[zSerie].Points.Add(double.Parse(sensorListParameter[i].SensorAccelerometerZ, CultureInfo.InvariantCulture));
                }
            }
        }

        //When filtering the entity list on a measurement
        private List<Entity> filterSensorListOnMeasurement(string date)
        {
            List<Entity> fulleSensorDataList = DataStorage.getInstance().getSensorData();
            List<Entity> sortedSensors = new List<Entity>();
            Boolean isStarted = false;

            for (int i = 0; i < fulleSensorDataList.Count; i++)
            {
                string[] rowKeyValues = fulleSensorDataList[i].RowKey.Split(';');
                if (fulleSensorDataList[i].METAData != null)
                {
                    if (fulleSensorDataList[i].METAData.Equals("Measurement START") && rowKeyValues[1].Equals(date))//Check if measurment started on correct timestamp
                        isStarted = true;
                    else if (fulleSensorDataList[i].METAData.Equals("Measurement STOP") && isStarted)//If it finds the stop of the measurement (and it has already found a measurement start)
                    {
                        sortedSensors.Add(fulleSensorDataList[i]);
                        break;
                    }
                }
                if (isStarted)  //Adds as long as we have not reached the end of the measurement
                    sortedSensors.Add(fulleSensorDataList[i]);

            }
            return sortedSensors;
        }

        //Method for adding data to the acclerometer graph
        private void addAcceleroMeterDataToGraph(string serie)
        {
            AcclerometerChart.Series.Add(serie);
            AcclerometerChart.Series[serie].ChartType = SeriesChartType.FastLine;      
        }

        protected void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MeasurementList.Items.Clear();
            ViewState["UserListSelectedUser"] = UserList.SelectedValue.ToString();
        }

        //When the user picks a data in the calender. Retrieves the data from the table storage for that date based on the user
        protected void CalendarOne_SelectionChanged(object sender, EventArgs e)
        {
            if (ViewState["UserListSelectedUser"] != null)
            {
                string date = CalendarOne.SelectedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string nextDate = CalendarOne.SelectedDate.AddDays(1).ToString();
                sensorData = azureTableConnector.RetriveDataFromSensors("people", date, nextDate, (string)ViewState["UserListSelectedUser"]);
                DataStorage.getInstance().setSensorData(sensorData);
                MeasurementList.Items.Clear();
                populateMeasurementList();
                callCreateChartMethods(sensorData);
            }
            else
            {
                Response.Write("<script>alert('You have to select a user')</script>");
            }
        }

        protected void MeasurementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            callCreateChartMethods(filterSensorListOnMeasurement(MeasurementList.SelectedValue.ToString()));
        }

        //Method that calls the chart methods
        private void callCreateChartMethods(List<Entity> chartSensorEntities)
        {
            clearCharts();
            createAccelerometerGraph(chartSensorEntities);
            createOneSerieGraph("LightChart", chartSensorEntities);
            createOneSerieGraph("ProximityChart", chartSensorEntities);
            createOneSerieGraph("BatteryChart", chartSensorEntities);
            createOneSerieGraph("METADATATable", chartSensorEntities);
        }

        //Clear every charts
        private void clearCharts()
        {
            AcclerometerChart.Series.Clear();
            ProximityChart.Series.Clear();
            LightChart.Series.Clear();
            BatteryChart.Series.Clear();
        }
    }

}

