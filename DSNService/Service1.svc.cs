using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DSNService
{
    public class Service1 : IService1
    {
        LaunchVehicleService.Service1Client launchVehicleService = new LaunchVehicleService.Service1Client();
        /**
         * Retrieve launch vehicle data based on the launchVehicleId
         */
        public XElement GetLaunchVehicleData(string launchVehicleId)
        {
            try
            {
                string launchVehiclePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "launchVehicleData.xml");
                XElement root = XElement.Load(launchVehiclePath);
                IEnumerable<XElement> launchVehEles = root.Descendants("launchVehicle")
                                                   .Where(m => m.Element("id").Value == launchVehicleId);
                return launchVehEles.FirstOrDefault();
            }
            catch (Exception e)
            {
                return new XElement("error");
            }

        }

        /**
         * Retrieve payload data based on payloadId 
         */
        public XElement GetPayloadData(string payloadId)
        {
            try {
                string payloadPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadData.xml");
                XElement payloadXEle = XElement.Load(payloadPath);
                IEnumerable<XElement> payloads = payloadXEle.Descendants("payload")
                                                   .Where(m => m.Element("id").Value == payloadId);
                return payloads.FirstOrDefault();
            }
            catch (Exception e) {
                return new XElement("error");
            }
        }

        /**
         * Retrieve all launch vehicle data
         */
        public IEnumerable<XElement> GetAllLaunchVehicleData()
        {
            try {
                string launchVehiclePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "launchVehicleData.xml");
                XElement launchVehicleXEle = XElement.Load(launchVehiclePath);
                IEnumerable<XElement> launchVehicles = launchVehicleXEle.Descendants("launchVehicle");
                return launchVehicles;
            }
            catch(Exception e) {
                return new List<XElement>();
            }
        }

        /**
         * Retreive launch vehicle telemetry data based on the launchVehicleId
         */
        public IEnumerable<XElement> GetLaunchVehicleTelemetryData(string launchVehicleId)
        {
            try {
                string launchVehicleTelePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "launchVehicleTelemetryData.xml");
                XElement root = XElement.Load(launchVehicleTelePath);
                IEnumerable<XElement> launchVehicleTeleData = root.Descendants("launchVehicleTelemetry")
                                                .Where(m => m.Element("launchVehicleId").Value == launchVehicleId);
                return launchVehicleTeleData;

            }
            catch (Exception e) {
                return new List<XElement>();            
            }
        }

        /**
         * Retrieve payload telemetry data based on the payloadId
         */
        public IEnumerable<XElement> GetPayloadTelemetryData(string payloadId)
        {
            try {
                string payloadTelePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadTelemetryData.xml");
                XElement root = XElement.Load(payloadTelePath);
                IEnumerable<XElement> payloadTeleData = root.Descendants("payloadTelemetry")
                                                   .Where(m => m.Element("payloadId").Value == payloadId);
                return payloadTeleData;
            }
            catch (Exception e) {
                return new List<XElement>();
            }
        }

        /**
         * Retrieve payload sent data based on the payloadId
         */
        public IEnumerable<XElement> GetPayloadProcessedData(string payloadId)
        {
            try {
                string payloadProcessPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadTransmissionData.xml");
                XElement root = XElement.Load(payloadProcessPath);
                IEnumerable<XElement> payloadProcesData = root.Descendants("payloadData")
                                                    .Where(m => m.Element("payloadId").Value == payloadId);
                return payloadProcesData;
            }
            catch (Exception e) {
                return new List<XElement>();
            }
        }

        /**
         * Launch spacecraft based on the launch vehicle and payload details provided
         */
        public Boolean launchSpacecraft(string name, int orbit, string payloadName, string payloadType)
        {
            try
            {
                string launchVehiclePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "launchVehicleData.xml");
                XElement launchVehicleData = XElement.Load(launchVehiclePath);

                string payloadPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadData.xml");
                XElement payloadData = XElement.Load(payloadPath);

                // Get Launch vehicle and Payload count
                int timeToOrbit = ((orbit / 3600)) + 10;

                int launchVehicleId = new Random().Next(1000, 100000);
                int payloadId = new Random().Next(1000, 100000);

                // Add Launch vehicle
                XElement launchVehicles = launchVehicleData.Descendants("launchVehicles").FirstOrDefault();
                XElement launchVehicle = createLaunchVehicleEle(launchVehicleId, name, orbit, timeToOrbit, payloadId);
                startLaunchVehicle(launchVehiclePath, launchVehicles, launchVehicleData, launchVehicle);

                // Add Payload
                XElement payloads = payloadData.Descendants("payloads").FirstOrDefault();
                XElement payload = createPayload(payloadId, payloadName, payloadType, orbit);
                startPayload(payloadPath, payloads, payloadData, payload);

                launchVehicleService.receiveCommandFromDSN(launchVehicleId, orbit, "START");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        /**
         * Add the launch vehicle and save it
         */
        private void startLaunchVehicle(string launchVehiclePath, XElement launchVehicles, XElement launchVehicleData, XElement launchVehicle)
        {
            try {
                launchVehicles.Add(launchVehicle);
                launchVehicleData.Save(launchVehiclePath);
            }
            catch (Exception e) { }
        }

        /**
         * Add the payload and save
         */
        private void startPayload(string payloadPath, XElement payloads, XElement payloadData, XElement payload)
        {
            try {
                payloads.Add(payload);
                payloadData.Save(payloadPath);
            }
            catch (Exception e) { }
        }

        /**
         * Create launch vehicle xml element 
         */
        private XElement createLaunchVehicleEle(int launchVehicleId, string name, int orbit, int timeToOrbit, int payloadCount)
        {
            try {
                XElement launchVehicle = new XElement("launchVehicle");
                launchVehicle.Add(new XElement("id", launchVehicleId));
                launchVehicle.Add(new XElement("name", name));
                launchVehicle.Add(new XElement("orbit", orbit));
                launchVehicle.Add(new XElement("status", "waiting"));
                launchVehicle.Add(new XElement("payloadId", payloadCount));
                launchVehicle.Add(new XElement("createdDate", GetTimestamp(DateTime.Now)));
                return launchVehicle;
            }
            catch(Exception e) {
                return new XElement("");
            }
        }

        /**
         * Create the payload xml element
         */
        private XElement createPayload(int payloadId, string payloadName, string payloadType, int orbit)
        {
            try {
                XElement payload = new XElement("payload");
                payload.Add(new XElement("id", payloadId));
                payload.Add(new XElement("name", payloadName));
                payload.Add(new XElement("type", payloadType));
                payload.Add(new XElement("orbit", orbit));
                payload.Add(new XElement("deployed", false));
                payload.Add(new XElement("createdDate", GetTimestamp(DateTime.Now)));
                return payload;
            }
            catch (Exception e) {
                return new XElement("");
            }
        }

        /**
         * Get timestamp string based on timestamp provided
         */
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm:ss");
        }

        /**
         *  Accepts command from launch vehicle and performs appropriate action
         */
        public void receiveCommandFromLaunchVehicle(int launchVehicleId, string command, XElement payloadData)
        {
            try
            {
                string launchVehiclePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "launchVehicleData.xml");
                XElement launchVehicleData = XElement.Load(launchVehiclePath);

                string launchVehicleTelePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "launchVehicleTelemetryData.xml");
                XElement launchVehicleTeleData = XElement.Load(launchVehicleTelePath);
                switch (command)
                {
                    case "REACHED_ORBIT":
                        IEnumerable<XElement> launchVeh = launchVehicleData.Descendants("launchVehicle")
                                                           .Where(m => m.Element("id").Value == launchVehicleId.ToString());
                        if (launchVeh.Count() == 0)
                        {
                            break;
                        }
                        // Update the status of launchVehicle
                        if (launchVeh.FirstOrDefault().Element("status").Value != "active")
                        {
                            launchVeh.FirstOrDefault().Element("status").Value = "active";
                            launchVehicleData.Save(launchVehiclePath);
                        }
                        break;
                    case "TELEMETRY_DATA":
                        IEnumerable<XElement> launchVehTele = launchVehicleTeleData.Descendants("launchVehicleTelemetries");
                        launchVehTele.FirstOrDefault().Add(payloadData);
                        launchVehicleTeleData.Save(launchVehicleTelePath);
                        break;
                    case "DEORBIT":
                        IEnumerable<XElement> launchVehicles = launchVehicleData.Descendants("launchVehicle")
                                                           .Where(m => m.Element("id").Value == launchVehicleId.ToString());
                        int payloadId = Int32.Parse(launchVehicles.FirstOrDefault().Element("payloadId").Value);
                        launchVehicleData.Descendants("launchVehicle")
                                                           .Where(m => m.Element("id").Value == launchVehicleId.ToString())
                                                           .Remove();
                        launchVehicleTeleData.Descendants("launchVehicleTelemetry")
                                                           .Where(m => m.Element("launchVehicleId").Value == launchVehicleId.ToString())
                                                           .Remove();
                        launchVehicleData.Save(launchVehiclePath);
                        launchVehicleTeleData.Save(launchVehicleTelePath);
                        receiveCommandFromPayload(payloadId, "DECOMMISSION", new XElement("test"));
                        break;
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        /**
        *  Accepts command from payload and performs appropriate action
        */
        public void receiveCommandFromPayload(int payloadId, string command, XElement data)
        {
            try
            {
                string payloadTelePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadTelemetryData.xml");
                string payloadPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadData.xml");
                string payloadTransPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "App_Data", "payloadTransmissionData.xml");
                XElement payloadXEle = XElement.Load(payloadPath);
                XElement payloadTeleXEle = XElement.Load(payloadTelePath);
                XElement payloadTransXEle = XElement.Load(payloadTransPath);
                switch (command)
                {
                    case "PAYLOAD_DEPLOYED":
                        IEnumerable<XElement> payloads = payloadXEle.Descendants("payload")
                                                           .Where(m => m.Element("id").Value == payloadId.ToString());
                        if (payloads.FirstOrDefault().Element("deployed").Value == "false")
                        {
                            // Update the value of deployed for payload
                            payloads.FirstOrDefault().Element("deployed").Value = "true";
                            payloadXEle.Save(payloadPath);
                        }
                        break;
                    case "TELEMETRY_DATA":
                        IEnumerable<XElement> payloadTelemetries = payloadTeleXEle.Descendants("payloadTelemetries");
                        payloadTelemetries.FirstOrDefault().Add(data);
                        payloadTeleXEle.Save(payloadTelePath);
                        break;
                    case "DATA":

                        IEnumerable<XElement> payloadDatas = payloadTransXEle.Descendants("payloadDatas");
                        payloadDatas.FirstOrDefault().Add(data);
                        payloadTransXEle.Save(payloadTransPath);
                        break;
                    case "DECOMMISSION":
                        payloadXEle.Descendants("payload")
                                                           .Where(m => m.Element("id").Value == payloadId.ToString())
                                                           .Remove();
                        payloadTeleXEle.Descendants("payloadTelemetry")
                                                           .Where(m => m.Element("payloadId").Value == payloadId.ToString())
                                                           .Remove();
                        payloadTransXEle.Descendants("payloadData")
                                                           .Where(m => m.Element("payloadId").Value == payloadId.ToString())
                                                           .Remove();
                        payloadXEle.Save(payloadPath);
                        payloadTeleXEle.Save(payloadTelePath);
                        payloadTransXEle.Save(payloadTransPath);
                        break;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
