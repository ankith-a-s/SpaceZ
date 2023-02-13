using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DSNWebApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        /**
         * Retrieve all launch vehicles and populate in list
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                DSNService.Service1Client dsnService = new DSNService.Service1Client();
                IEnumerable<XElement> launchVehicles = dsnService.GetAllLaunchVehicleData();
                Repeater1.DataSource = from x in launchVehicles
                                       select new
                                       {
                                           name = x.Element("name").Value,
                                           orbit = x.Element("orbit").Value,
                                           status = x.Element("status").Value,
                                           id = x.Element("id").Value
                                       };
                Repeater1.DataBind();
            }
            catch { }
        }
    }
}