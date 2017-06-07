using CouldProjectAzureV2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CouldProjectAzureV2.Account
{
    //Class for the role assign page
    public partial class RoleAssignPage_WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                roleListPopulation();
            }
        }

        //Populate the role dropdownlist with users
        private void roleListPopulation()
        {
            RoleDropDownList.Items.Clear();
            var Usermanager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            string[] roleNames = null;
            var roles = RoleManager.Roles.Select(r => r.Name).ToArray();
            roleNames = roles;

            for (int i = 0; i < roleNames.Count(); i++)
                RoleDropDownList.Items.Insert(RoleDropDownList.Items.Count, roleNames[i]);
        }

        private void giveRoleToUser(string roleName)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser applicationUser = userManager.FindByEmail(DataStorage.getInstance().getSelectedUser());

            //Add user to the new role
            userManager.AddToRole(applicationUser.Id, roleName);
            context.SaveChanges();

            Response.Write("<script>alert('Assigned role to user')</script>");
        }

        protected void RoleDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RoleListSelectedRole"] = RoleDropDownList.SelectedValue.ToString();
        }

        protected void roleAssignButton_Click(object sender, EventArgs e)
        {
            if (ViewState["RoleListSelectedRole"] != null)
            {
                giveRoleToUser((string)ViewState["RoleListSelectedRole"]);              
            }
            else
            {
                Response.Write("<script>alert('You have to select a role')</script>");
            }

        }
    }
}