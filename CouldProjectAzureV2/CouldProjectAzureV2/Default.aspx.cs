using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            HttpCookie myCookie = new HttpCookie("myCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("userid", "Look at this you fool!");

            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddHours(12);

            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);

        }
    }
}