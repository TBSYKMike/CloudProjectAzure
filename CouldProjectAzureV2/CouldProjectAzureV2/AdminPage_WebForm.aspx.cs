﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouldProjectAzureV2
{
    /*This class is for the admin page where user settings adjustments can be changed*/
    public partial class AdminPage_WebForm : System.Web.UI.Page
    {
        private DatabaseConnector databaseConnector = new DatabaseConnector();
        private DataTable dataTable;
        private Boolean pageRefresh;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) //Initialize gridview components if not postback
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();

                Debug.WriteLine("Not Post back");
                dataTable = new DataTable();
                createDataTable();
                addUsersToGridView();
                gridViewDataBind();
                initializeButtonColors();
            }
            else
            {
                Debug.WriteLine("Postback");
                dataTable = (DataTable)ViewState["DataTable"];

                if (ViewState["postids"].ToString() != Session["postid"].ToString())
                {
                    pageRefresh = true;
                }
                Session["postid"] = System.Guid.NewGuid().ToString();
                ViewState["postids"] = Session["postid"].ToString();
            }
            ViewState["DataTable"] = dataTable;
     
        }

        private void createDataTable()
        {
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
        }



        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void gridViewDataBind()
        {
            UserGridView.DataSource = dataTable;
            UserGridView.DataBind();
        }

        //Adds all patients to the gridview
        private void addUsersToGridView()
        {
            var context = new IdentityDbContext();
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext()));
            var users = rm.FindByName("patient").Users.Select(x => x.UserId);
            var usersInRole = context.Users.Where(u => users.Contains(u.Id)).ToList();

            List<string> usersIdList = new List<string>();

            for (int i = 0; i < usersInRole.Count; i++)
            {
                usersIdList.Add(usersInRole[i].Id);
            }

            ViewState["usersIdInTable"] = usersIdList;

            for (int i = 0; i < usersInRole.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Email"] = usersInRole[i].Email;
                dataRow["UserName"] = usersInRole[i].UserName;
                dataTable.Rows.Add(dataRow);
            }

        }

        //Retrieves patient user settings from database and set current settings in the gridview
        private void initializeButtonColors() 
        {
            List<string> usersIdAsList = (List<string>)ViewState["usersIdInTable"];
            for (int i = 0; i < usersIdAsList.Count; i++)
            {
                UserSettings userSettings = databaseConnector.getUserSettings(usersIdAsList[i]);

                Button acceleroMeterOnButton = (Button)UserGridView.Rows[i].FindControl("acceleroMeterOnButton");
                Button acceleroMeterOffButton = (Button)UserGridView.Rows[i].FindControl("acceleroMeterOffButton");
                Button proximityOnButton = (Button)UserGridView.Rows[i].FindControl("proximityOnButton");
                Button proximityOffButton = (Button)UserGridView.Rows[i].FindControl("proximityOffButton");
                Button lightOnButton = (Button)UserGridView.Rows[i].FindControl("lightOnButton");
                Button lightOffButton = (Button)UserGridView.Rows[i].FindControl("lightOffButton");
                Button samplingRateSlowButton = (Button)UserGridView.Rows[i].FindControl("samplingRateSlowButton");
                Button samplingRateMediumButton = (Button)UserGridView.Rows[i].FindControl("samplingRateMediumButton");
                Button samplingRateFastButton = (Button)UserGridView.Rows[i].FindControl("samplingRateFastButton");


                if (userSettings.getAcceleroMeterOnoff().Equals(1))
                    changeButtonAppearanceOnOff(true, acceleroMeterOnButton, acceleroMeterOffButton);              
                else
                    changeButtonAppearanceOnOff(false, acceleroMeterOnButton, acceleroMeterOffButton);

                if (userSettings.getProximityOnoff().Equals(1))
                    changeButtonAppearanceOnOff(true, proximityOnButton, proximityOffButton);
                else
                    changeButtonAppearanceOnOff(false, proximityOnButton, proximityOffButton);

                if (userSettings.getLightOnOff().Equals(1))
                    changeButtonAppearanceOnOff(true, lightOnButton, lightOffButton);
                else
                    changeButtonAppearanceOnOff(false, lightOnButton, lightOffButton);

                if (userSettings.getSamplingRate().Equals(1))
                    changeButtonAppereanceSensor(samplingRateSlowButton, samplingRateMediumButton, samplingRateFastButton, "Slow");
                else if (userSettings.getSamplingRate().Equals(2))
                    changeButtonAppereanceSensor(samplingRateSlowButton, samplingRateMediumButton, samplingRateFastButton, "Medium");
                else if (userSettings.getSamplingRate().Equals(3))
                    changeButtonAppereanceSensor(samplingRateSlowButton, samplingRateMediumButton, samplingRateFastButton, "Fast");
            }             
        }


        private void checkUserSettingList() { 
        }

        //Method for changing button classes for the sensor buttons
        private void changeButtonAppearanceOnOff(Boolean on, Button onButton, Button offButton)
        {
            if (on)
            {
                onButton.CssClass = "btn btn-success";
                offButton.CssClass = "btn btn-default";
            }
            else
            {
                onButton.CssClass = "btn btn-deafult";
                offButton.CssClass = "btn btn-danger";
            }

        }

        //Method for changing button classes for the sensor frequency buttons
        private void changeButtonAppereanceSensor(Button slow, Button medium, Button fast, String state)
        {
            if (state.Equals("Slow"))
            {
                slow.CssClass = "btn btn-success";
                medium.CssClass = "btn btn-default";
                fast.CssClass = "btn btn-default";
            }
            else if(state.Equals("Medium"))
            {
                medium.CssClass = "btn btn-success";
                slow.CssClass = "btn btn-default";
                fast.CssClass = "btn btn-default";
            }
            else
            {
                fast.CssClass = "btn btn-success";
                slow.CssClass = "btn btn-default";
                medium.CssClass = "btn btn-default";
            }
        }

        public void acceleomneterCellClick(object sender, EventArgs e)
        {
            if (!pageRefresh)
                updateSensorStatus(sender, "accelerometerOnOff");
        }
        public void proximityCellClick(object sender, EventArgs e)
        {
            if (!pageRefresh)
                updateSensorStatus(sender, "proximityOnOff");
        }
        public void lightCellClick(object sender, EventArgs ew)
        {
            if (!pageRefresh)
                updateSensorStatus(sender, "lightOnOff");
        }
     
        public void userNameOnClick(object sender, EventArgs ew)
        {
            LinkButton linkButton = (LinkButton)sender;
            DataStorage.getInstance().setSelectedUser(linkButton.Text);
            Response.Redirect("RoleAssignPage_WebForm");
        }

        //When the user is changning a sensor setting. Updates the button class and the database collumn
        private void updateSensorStatus(object sender, String collumnName)
        {
            GridViewRow row = null;
            Button button = (Button)sender;
            row = (GridViewRow)button.NamingContainer;

            int index = row.RowIndex;
            List<string> usersIdAsList = (List<string>)ViewState["usersIdInTable"];

            Button onButton = null;
            Button offButton = null;

            if (collumnName.Equals("accelerometerOnOff"))
            {
                onButton = (Button)UserGridView.Rows[index].FindControl("acceleroMeterOnButton");
                offButton = (Button)UserGridView.Rows[index].FindControl("acceleroMeterOffButton");
            }

            else if (collumnName.Equals("proximityOnOff"))
            {
                onButton = (Button)UserGridView.Rows[index].FindControl("proximityOnButton");
                offButton = (Button)UserGridView.Rows[index].FindControl("proximityOffButton");
            }

            else if (collumnName.Equals("lightOnOff"))
            {
                onButton = (Button)UserGridView.Rows[index].FindControl("lightOnButton");
                offButton = (Button)UserGridView.Rows[index].FindControl("lightOffButton");
            }


            if (button.Text.Equals("On"))
            {
                changeButtonAppearanceOnOff(true, onButton, offButton);
                databaseConnector.setSettingForUser(usersIdAsList[index], collumnName, 1);

            }
            else if (button.Text.Equals("Off"))
            {
                changeButtonAppearanceOnOff(false, onButton, offButton);
                databaseConnector.setSettingForUser(usersIdAsList[index], collumnName, 0);
            }
        }

        public void changeSamplingFrequencyClick(object sender, EventArgs e)
        {
            Debug.Write("changeSamplingFrequency_Click");
            if (!pageRefresh)
                changeSamplingFrequency(sender, "samplingRate");
        }

        //When the user is changning a sensor frequency. Updates the button class and the database collumn
        private void changeSamplingFrequency(object sender, String collumName)
        {
            List<string> usersIdAsList = (List<string>)ViewState["usersIdInTable"];
            GridViewRow row = null;
            Button button = (Button)sender;
            row = (GridViewRow)button.NamingContainer;
            int index = row.RowIndex;

            Button slowButton = (Button)UserGridView.Rows[index].FindControl("samplingRateSlowButton");
            Button mediumButton = (Button)UserGridView.Rows[index].FindControl("samplingRateMediumButton");
            Button fastButton = (Button)UserGridView.Rows[index].FindControl("samplingRateFastButton");


            if (button.Text.Equals("Slow"))
            {
                changeButtonAppereanceSensor(slowButton, mediumButton, fastButton, "Slow");
                databaseConnector.setSettingForUser(usersIdAsList[index], collumName, 1);
            }
            else if (button.Text.Equals("Medium"))
            {
                changeButtonAppereanceSensor(slowButton, mediumButton, fastButton, "Medium");
                databaseConnector.setSettingForUser(usersIdAsList[index], collumName, 2);
            }
            else
            {
                changeButtonAppereanceSensor(slowButton, mediumButton, fastButton, "Fast");
                databaseConnector.setSettingForUser(usersIdAsList[index], collumName, 3);
            }
        }
    }
    }

        