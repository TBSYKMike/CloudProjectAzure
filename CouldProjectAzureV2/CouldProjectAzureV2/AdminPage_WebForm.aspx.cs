﻿using Microsoft.AspNet.Identity.EntityFramework;
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
    public partial class AdminPage_WebForm : System.Web.UI.Page
    {
        private DatabaseConnector databaseConnector = new DatabaseConnector();
        private DataTable dataTable;
        private Boolean pageRefresh;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();

                System.Diagnostics.Debug.WriteLine("Not Post back");
                dataTable = new DataTable();
                createDataTable();
                addUsersToGridView();
                gridViewDataBind();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Postback");
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
        private void addUsersToGridView()
        {
            var context = new IdentityDbContext();
            var users = context.Users.ToList();
            //var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("rolename")).ToList();   Använd denna senare
            List<string> usersIdList = new List<string>();

            for (int i = 0; i < users.Count; i++)
            {
                usersIdList.Add(users[i].Id);
            }

            ViewState["usersIdInTable"] = usersIdList;

            for (int i = 0; i < users.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Email"] = users[i].Email;
                dataRow["UserName"] = users[i].UserName;
                dataTable.Rows.Add(dataRow);
            }


        }

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
                databaseConnector.setSettingForUser("2", collumnName, 1); //Ska vara usersIdAsList[index] istället för 2

            }
            else if (button.Text.Equals("Off"))
            {
                changeButtonAppearanceOnOff(false, onButton, offButton);
                databaseConnector.setSettingForUser("2", collumnName, 0); //Ska vara usersIdAsList[index] istället för 2
            }
        }


        public void changeSamplingFrequencyClick(object sender, EventArgs e)
        {
            Debug.Write("changeSamplingFrequency_Click");
            if (!pageRefresh)
                changeSamplingFrequency(sender, "samplingRate");
        }

        private void changeSamplingFrequency(object sender, String collumName)
        {
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
                databaseConnector.setSettingForUser("2", collumName, 1); //Ska vara usersIdAsList[index] istället för 2
            }
            else if (button.Text.Equals("Medium"))
            {
                changeButtonAppereanceSensor(slowButton, mediumButton, fastButton, "Medium");
                databaseConnector.setSettingForUser("2", collumName, 2); //Ska vara usersIdAsList[index] istället för 2
            }
            else
            {
                changeButtonAppereanceSensor(slowButton, mediumButton, fastButton, "Fast");
                databaseConnector.setSettingForUser("2", collumName, 3); //Ska vara usersIdAsList[index] istället för 2
            }
        }
    }
    }

        