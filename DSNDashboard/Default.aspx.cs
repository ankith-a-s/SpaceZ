using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSNDashboard
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLearnMore_Click(object sender, EventArgs e) {
           

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DSNService.Service1Client dsnServiceClient = new DSNService.Service1Client();
            string spacecraftData = dsnServiceClient.GetSpacecraftData("1");
            string emptyString = "";
        }
    }
}