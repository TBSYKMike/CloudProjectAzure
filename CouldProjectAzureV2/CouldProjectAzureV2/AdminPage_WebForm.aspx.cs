using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
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

            for(int i = 0; i < users.Count; i++)
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

        private void changeButtonAppearance(Boolean on, Button onButton, Button offButton)
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

        public void acceleomneterCellClick(object sender, EventArgs e)
        {
            if(!pageRefresh)
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
            List<string> usersAsList = (List<string>)ViewState["usersIdInTable"];

            if (button.Text.Equals("On"))
            {            
                databaseConnector.setSettingForUser(usersAsList[index], collumnName, 1);
            }
            else if (button.Text.Equals("Off"))
            {
                databaseConnector.setSettingForUser(usersAsList[index], collumnName, 0);
            }
        }



        private void turnOnOffSensor_Click(object sender, String buttonType)
        {
            //   if (!pageRefresh)
          /*  {
                System.Diagnostics.Debug.WriteLine("Delete product");
                GridViewRow row = null;
                if (buttonType == "button")
                {
                    Button button = (Button)sender;
                    row = (GridViewRow)button.NamingContainer;
                    

                    TableCell cell = null;
                    Control parent = button;
                    while ((parent = parent.Parent) != null && cell == null)
                        cell = parent as TableCell;
                    int indexOfTextBoxCell = -1;
                    if (cell != null)
                        indexOfTextBoxCell = sender.Cells.GetCellIndex(cell);

                    if (button.Text.Equals("On"))
                    {
                        int index = row.RowIndex;
                        string userId = usersAsList[index].Id;
                        UserGridView.Rows[0].Cells[i].Text
                        databaseConnector.setUserSettings(userId,);
                    }
                    else if (button.Text.Equals("Off"))
                    {

                    }
                    */
                }
                /*else if (buttonType == "linkButton")
                {
                    LinkButton button = (LinkButton)sender;
                    row = (GridViewRow)button.NamingContainer;
                }
                else if (buttonType == "textBox")
                {
                    TextBox button = (TextBox)sender;
                    row = (GridViewRow)button.NamingContainer;
                }

                if (row != null)
                {
                    int index = row.RowIndex;

                }*/

                // }
            }
        }


        