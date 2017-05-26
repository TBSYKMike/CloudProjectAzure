using CouldProjectAzureV2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouldProjectAzureV2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //AzureTableConnector azureTableConnector = new AzureTableConnector();
            //azureTableConnector.RetriveDataFromSensors("people");
            //create a cookie
            //    var context = new IdentityDbContext();
            //  var role = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new Microsoft.AspNet.Identity.EntityFramework.RoleStore<IdentityRole>(dbcontext));


            //var users = context.Users.ToList();
         //   var list = Roles.GetUsersInRole("patient").Select(Membership.GetUser).ToList();
           // Debug.Write("looksdsdsd:    " + list[0].UserName);

      //  var role = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new Microsoft.AspNet.Identity.EntityFramework.RoleStore<IdentityRole>(dbcontext));
      //       var context = new ApplicationDbContext();
      //     var users = from u in context.Users
      //               where u.Roles.Any(r => r.Role.Name== "patient")
      //             select u;
      // Debug.Write("Look!!" )


            /*

            var roleUserIdsQuery = from role in context.Roles
                                   where role.Name == "patient"
                                   from user in role.Users
                                   select user.UserId;
            var users = context.Users.Where(u => roleUserIdsQuery.Contains(u.Id)).ToList();
            Debug.Write("Look" + users[0].UserName);
            */
        }
    }
}