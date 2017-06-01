using CouldProjectAzureV2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouldProjectAzureV2
{
    public partial class CreateRole_WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void createRole(string rolename)
        {
            ApplicationDbContext dbcontext = new ApplicationDbContext();
            var role = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new Microsoft.AspNet.Identity.EntityFramework.RoleStore<IdentityRole>(dbcontext));
            var roleresult = role.Create(new IdentityRole(rolename));
            if (roleresult.Succeeded)
            {
                Response.Write("<script>alert('Created role:   '"+rolename +")</script>");
            }
            else
            {
                Response.Write("<script>alert('Error when creating role)</script>");
                Debug.WriteLine("cant create user");
            }
        }

        protected void createRoleButton_Click(object sender, EventArgs e)
        {
            createRole(roleNameTextBox.Text);
        }
    }
}