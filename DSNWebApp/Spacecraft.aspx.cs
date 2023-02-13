using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DSNWebApp
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        public string launchVehicleId, launchVehicleOrbit, launchVehicleStatus, launchVehicleName, payloadName, payloadType, payloadId;
        public DSNService.Service1Client dsnService = new DSNService.Service1Client();
        public LaunchVehicleService.Service1Client launchVehicleService = new LaunchVehicleService.Service1Client();
        public PayloadService.Service1Client payloadService = new PayloadService.Service1Client();

        /**
         * Sends the selected command to payload
         */
        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Select1.Value != "")
                {
                    payloadService.receiveCommandFromDSN(Int32.Parse(payloadId), "", Select1.Value);
                    Label3.Text = Select1.Value + " command is successfully sent";
                    Label4.Text = "";
                }
                else {
                    Label4.Text = "Please select the command before sending request";
                    Label3.Text = "";
                }
            }
            catch (Exception ex)
            {
                Label4.Text = "Error in sending the command";
                Label3.Text = "";
            }
        }

        /**
         * Sends the selected command to launchVehicle
         */
        protected void Button1_Click(object sender, EventArgs e)
        {
            try {
                if (Select2.Value != "")
                {
                    launchVehicleService.receiveCommandFromDSN(Int32.Parse(launchVehicleId), 0, Select2.Value);
                    Label1.Text = Select2.Value + " command is successfully sent";
                    Label2.Text = "";
                    if (Select2.Value == "DEORBIT")
                    {
                        Response.Redirect("/");
                        return;
                    }
                   
                }
                else
                {
                    Label2.Text = "Please select the command before sending request";
                    Label1.Text = "";
                }
            }
            catch (Exception ex) {
                Label2.Text = "Error in sending the command";
                Label1.Text = "";
            }
        }

        /**
         * Get launchvehicle, payload and telemetry data for the spacecraft
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                launchVehicleId = Request.QueryString["id"];
                XElement launchVehicleData = dsnService.GetLaunchVehicleData(launchVehicleId);
                if (launchVehicleData == null)
                {
                    Response.Redirect("/");
                    return;
                }
                payloadId = launchVehicleData.Element("payloadId").Value;
                XElement payload = dsnService.GetPayloadData(payloadId);
                IEnumerable<XElement> launchVehicleTelemetryData = dsnService.GetLaunchVehicleTelemetryData(launchVehicleId);
                IEnumerable<XElement> payloadTelemetryData = dsnService.GetPayloadTelemetryData(payloadId);
                IEnumerable<XElement> payloadProcessedData = dsnService.GetPayloadProcessedData(payloadId);
                PopulateLVTeleData(launchVehicleTelemetryData);
                PopulatePLTeleData(payloadTelemetryData);
                if (payloadProcessedData.Count() != 0)
                {
                    PopulatePLProcessData(payloadProcessedData, payloadProcessedData.First().Element("type").Value);
                }
                PopulateLVData(launchVehicleData);
                PopulatePLData(payload);
            }
            catch { 
            
            }
        }

        /**
         * Populating payload data
         */
        private void PopulatePLData(XElement payload)
        {
            if (payload != null) {
                payloadName = payload.Element("name").Value;
                payloadType = payload.Element("type").Value;
            }
        }

        /**
         * Populating launchvehicle data
         */
        private void PopulateLVData(XElement launchVehicleData)
        {
            launchVehicleName = launchVehicleData.Element("name").Value;
            launchVehicleOrbit = launchVehicleData.Element("orbit").Value;
            launchVehicleStatus = launchVehicleData.Element("status").Value;
        }

        /**
         * Populating launch vehicle telemetry data
         */
        private void PopulateLVTeleData(IEnumerable<XElement> launchVehicleTelemetryData)
        {
            Repeater1.DataSource = from x in launchVehicleTelemetryData
                                   select new
                                   {
                                       altitude = x.Element("altitude").Value,
                                       longitude = x.Element("longitude").Value,
                                       latitude = x.Element("latitude").Value,
                                       temperature = x.Element("temperature").Value,
                                       timeToOrbit = x.Element("timeToOrbit").Value,
                                       createdDate = x.Element("createdDate").Value
                                   };
            Repeater1.DataBind();
        }

        /**
         * Populating payload telemetry data
         */
        private void PopulatePLTeleData(IEnumerable<XElement> payloadTelemetryData)
        {
            Repeater2.DataSource = from x in payloadTelemetryData
                                   select new
                                   {
                                       altitude = x.Element("altitude").Value,
                                       longitude = x.Element("longitude").Value,
                                       latitude = x.Element("latitude").Value,
                                       temperature = x.Element("temperature").Value,
                                       createdDate = x.Element("createdDate").Value
                                   };
            Repeater2.DataBind();
        }

        /**
         * Populating payload data
         */
        private void PopulatePLProcessData(IEnumerable<XElement> payloadProcessedData, string type)
        {
            switch (type)
            {
                case "Scientific":
                    Repeater3.DataSource = from x in payloadProcessedData
                                           select new
                                           {
                                               rainfall = x.Element("rainfall").Value,
                                               humidity = x.Element("humidity").Value,
                                               snow = x.Element("snow").Value,
                                               createdDate = x.Element("createdDate").Value
                                           };
                    Repeater3.DataBind();
                    break;
                case "Communication":
                    Repeater4.DataSource = from x in payloadProcessedData
                                           select new
                                           {
                                               uplink = x.Element("uplink").Value,
                                               downlink = x.Element("downlink").Value,
                                               transponders = x.Element("activeTransponders").Value,
                                               createdDate = x.Element("createdDate").Value
                                           };
                    Repeater4.DataBind();
                    break;
                case "Spy":
                    Repeater5.DataSource = from x in payloadProcessedData
                                           select new
                                           {
                                               imageData = x.Element("imageData").Value,
                                               createdDate = x.Element("createdDate").Value
                                           };
                    Repeater5.DataBind();
                    break;
            }
        }
    }
}