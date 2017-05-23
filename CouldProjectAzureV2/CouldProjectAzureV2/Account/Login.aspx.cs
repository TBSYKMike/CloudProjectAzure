using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CouldProjectAzureV2.Models;
using System.Diagnostics;

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
                        addCookieForAndroid("true");
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
                        addCookieForAndroid("false");
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }

        private void addCookieForAndroid(String cookieStatus)
        {
            HttpCookie loginCookie = new HttpCookie("loginSuccessCookie");
            loginCookie.Values.Add(cookieStatus, "status");
            loginCookie.Expires = DateTime.Now.AddHours(12);
            Response.Cookies.Add(loginCookie);
            Debug.WriteLine("Added cookie for Android");
        }

    }
}