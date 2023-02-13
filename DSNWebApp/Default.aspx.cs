using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DSNWebApp
{
    public partial class _Default : Page
    {
        /**
         * Populate the Active and Waiting tabs with the launch vehicle data
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DSNService.Service1Client dsnService = new DSNService.Service1Client();
                IEnumerable<XElement> launchVehicles = dsnService.GetAllLaunchVehicleData();
                // Filter the launchvehicles based on the status of "active"
                Repeater1.DataSource = from x in launchVehicles
                                       where x.Element("status").Value == "active"
                                       select new
                                       {
                                           name = x.Element("name").Value,
                                           orbit = x.Element("orbit").Value,
                                           status = x.Element("status").Value,
                                           id = x.Element("id").Value
                                       };
                Repeater1.DataBind();
                // Filter the launchvehicles based on the status of "waiting"
                Repeater2.DataSource = from x in launchVehicles
                                       where x.Element("status").Value == "waiting"
                                       select new
                                       {
                                           name = x.Element("name"),
                                           orbit = x.Element("orbit"),
                                           status = x.Element("status"),
                                           id = x.Element("id").Value
                                       };
                Repeater2.DataBind();
            }
            catch { }
        }
    }
}