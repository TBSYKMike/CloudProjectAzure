﻿using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CouldProjectAzureV2.Models;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CouldProjectAzureV2.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }
        
        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        string userId = signinManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();                
                        getAndStoreUserSettings(userId); // Call create setting cookies 
                        addCookieForAndroid("true", "loginSuccessCookie");  //Call create login success cookie

                        // get the user with user manager 
                        ApplicationDbContext context = new ApplicationDbContext();
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                        //check if user is admin, and set the user role in session
                        if (UserManager.IsInRole(userId, "admin"))
                        {

                            this.Session["userRole"] = "admin";
                        }
                        else
                        {
                            this.Session["userRole"] = "patient";
                        }

                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);                  
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        addCookieForAndroid("false", "loginSuccessCookie");
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }

        private void addCookieForAndroid(String cookieStatus, String cookieName) //Store cookies that Android can use on login
        {
            HttpCookie loginCookie = new HttpCookie(cookieName);
            loginCookie.Values.Add(cookieStatus, "status");
            loginCookie.Expires = DateTime.Now.AddHours(12);
            Response.Cookies.Add(loginCookie);
            Debug.WriteLine("Added cookie for Android");
        }

        private void getAndStoreUserSettings(String userId)  // Retrieves settings from the database and call the addCookie method
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserSettings userSettings = databaseConnector.getUserSettings(userId);
            addCookieForAndroid(userSettings.getAcceleroMeterOnoff().ToString(), "AccelerometerOnoff");
            addCookieForAndroid(userSettings.getLightOnOff().ToString(), "LightOnOff");
            addCookieForAndroid(userSettings.getProximityOnoff().ToString(), "ProximityOnoff");
            addCookieForAndroid(userSettings.getSamplingRate().ToString(), "SamplingRate");

        }

    }
}