﻿using System;
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

    public partial class Chart_Webform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            createLightOrProximityGraph("LightChart");
            createLightOrProximityGraph("ProximityChart");
            createAcceleroMeterGraph();
        }

        private void createLightOrProximityGraph(string chartType)
        {
            AzureTableConnector azureTableConnector = new AzureTableConnector();
            List<Entity> sensorData = azureTableConnector.RetriveDataFromSensors("people");
            string serie = "";
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

            for (int i = 0; i < sensorData.Count(); i++)
            {
                if (sensorData[i].SensorLight != null && chartType.Equals("LightChart"))
                {
                    LightChart.Series[serie].Points.Add(double.Parse(sensorData[i].SensorLight, CultureInfo.InvariantCulture));
                }
                else if (sensorData[i].SensorProximity != null && chartType.Equals("ProximityChart"))
                {
                    ProximityChart.Series[serie].Points.Add(double.Parse(sensorData[i].SensorProximity, CultureInfo.InvariantCulture));
                }
            }

        }

        private void createAcceleroMeterGraph()
        {
            AzureTableConnector azureTableConnector = new AzureTableConnector();
            List<Entity> sensorData = azureTableConnector.RetriveDataFromSensors("people");

            string xSerie = "x";
            string ySerie = "y";
            string zSerie = "z";

            addAcceleroMeterDataToGraph(xSerie);
            addAcceleroMeterDataToGraph(ySerie);
            addAcceleroMeterDataToGraph(zSerie);

            AcclerometerChart.Series[0].Color = System.Drawing.Color.Green;
            AcclerometerChart.Series[1].Color = System.Drawing.Color.Black;
            AcclerometerChart.Series[2].Color = System.Drawing.Color.CornflowerBlue;

            for (int i = 0; i < sensorData.Count(); i++)
            {
                if (sensorData[i].SensorAccelerometerX != null && sensorData[i].SensorAccelerometerY != null && sensorData[i].SensorAccelerometerZ != null)
                {
                    AcclerometerChart.Series[xSerie].Points.Add(double.Parse(sensorData[i].SensorAccelerometerX, CultureInfo.InvariantCulture));
                    AcclerometerChart.Series[ySerie].Points.Add(double.Parse(sensorData[i].SensorAccelerometerY, CultureInfo.InvariantCulture));
                    AcclerometerChart.Series[zSerie].Points.Add(double.Parse(sensorData[i].SensorAccelerometerZ, CultureInfo.InvariantCulture));
                }
            }
        }

        private void addAcceleroMeterDataToGraph(string serie)
        {
            AcclerometerChart.Series.Add(serie);
            AcclerometerChart.Series[serie].ChartType = SeriesChartType.FastLine;
            // DataChart.Series[xSeries].BorderWidth = 2;      
        }




        





















     //      var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //     var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
        //    var result = manager.GetEmailAsync("8cc2231a-ecf5-4c95-9b62-ca6610d5c3a7");
       //     var user = manager.FindById("8cc2231a-ecf5-4c95-9b62-ca6610d5c3a7");
         //   Debug.WriteLine("Look at the result:   " + user.ToString());
        

        // var manager = new UserManager<MyUser>(new UserStore<MyUser>(new MyDbContext()));

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

    }
}