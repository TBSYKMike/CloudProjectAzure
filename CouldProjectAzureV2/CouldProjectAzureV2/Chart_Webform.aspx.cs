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

        private void populateMeasurementList()
        {
            //Splitta metadatan innan om mer metadata tex tal
            for (int i = 0; i < sensorData.Count; i++)
            {
                if (sensorData[i].METAData != null)
                {
                    if (sensorData[i].METAData.Equals("Measurement START"))
                    {
                        string[] rowKeyValues = sensorData[i].RowKey.Split(';');
                        MeasurementList.Items.Add(rowKeyValues[1]);
                    }
                }
            }
        }

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

                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    tCell.Text = "Time " + stringSplit[1] + ", Cell " + sensorListParameter[i].METAData;
                    tRow.Cells.Add(tCell);

                }


            }
        }

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

        private List<Entity> filterSensorListOnMeasurement(string date)
        {
            List<Entity> fulleSensorDataList = DataStorage.getInstance().getSensorData();
            Boolean isStarted = false;

            for (int i = 0; i < fulleSensorDataList.Count; i++)
            {
                string[] rowKeyValues = fulleSensorDataList[i].RowKey.Split(';');
                if (fulleSensorDataList[i].METAData != null)
                {
                    if (fulleSensorDataList[i].METAData.Equals("Measurement START") && rowKeyValues[1].Equals(date))
                        isStarted = true;
                    else if (fulleSensorDataList[i].METAData.Equals("Measurement STOP"))
                    {
                        fulleSensorDataList.Add(fulleSensorDataList[i]);
                        break;
                    }
                }
                if (isStarted)
                    fulleSensorDataList.Add(fulleSensorDataList[i]);

            }
            return fulleSensorDataList;
        }

        private void addAcceleroMeterDataToGraph(string serie)
        {
            AcclerometerChart.Series.Add(serie);
            AcclerometerChart.Series[serie].ChartType = SeriesChartType.FastLine;
            // DataChart.Series[xSeries].BorderWidth = 2;      
        }

        protected void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MeasurementList.Items.Clear();
            ViewState["UserListSelectedUser"] = UserList.SelectedValue.ToString();
        }

        protected void CalendarOne_SelectionChanged(object sender, EventArgs e)
        {
            if (ViewState["UserListSelectedUser"] != null)
            {
                string date = CalendarOne.SelectedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                sensorData = azureTableConnector.RetriveDataFromSensors("people", date, (string)ViewState["UserListSelectedUser"]);
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

        private void callCreateChartMethods(List<Entity> chartSensorEntities)
        {
            clearCharts();
            createAccelerometerGraph(chartSensorEntities);
            createOneSerieGraph("LightChart", chartSensorEntities);
            createOneSerieGraph("ProximityChart", chartSensorEntities);
            createOneSerieGraph("BatteryChart", chartSensorEntities);

            createOneSerieGraph("METADATATable", chartSensorEntities);

        }

        private void clearCharts()
        {
            AcclerometerChart.Series.Clear();
            ProximityChart.Series.Clear();
            LightChart.Series.Clear();
            BatteryChart.Series.Clear();
        }
    }

}











//      var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
//     var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
//    var result = manager.GetEmailAsync("8cc2231a-ecf5-4c95-9b62-ca6610d5c3a7");
//     var user = manager.FindById("8cc2231a-ecf5-4c95-9b62-ca6610d5c3a7");
//   Debug.WriteLine("Look at the result:   " + user.ToString());


//var manager = new UserManager<MyUser>(new UserStore<MyUser>(new MyDbContext()));

//            var user = new MyUser {};
//          user.
//        IdentityResult result = manager

/*    var userSettings = new UserSensorSettings() { };
    userSettings.userId = 34;
    userSettings.accelerometerOnOff = 1;
    userSettings.lightOnOff = 1;
    userSettings.proximityOnOff = 0;
    userSettings.samplingRate = 1000;*/

//var user = new MyUser() {  };
// user.UserSensorSettings.accelerometerOnOff = 1;
// user.UserSensorSettings.userId = 12;

// var currentUser = manager.FindById(User.Identity.GetUserId());
// var ef = currentUser.UserSensorSettings.proximityOnOff;




//var conf = new MyDbContext();
// conf.MyUserInfo = user;
// conf.SaveChanges();


/*   IdentityResult result = manager.Create();

   if (result.Succeeded)
   {
       signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

       string role = (string)Session["selectedRole"];
       giveRoleToUser(user, role);
       Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie); //We cant find the users role if we do not log out and in
       signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
       IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
   }*/


