using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace LaunchVehicleService
{
    public class Service1 : IService1
    {
        // Maintain the references of launchVehicles
        public static Dictionary<int, LaunchVehicle> launchVehicles = new Dictionary<int, LaunchVehicle>();
        public DSNService.Service1Client dsnService = new DSNService.Service1Client();
        
        /**
         * Receices command from DSN and takes appropriate action
         */
        public void receiveCommandFromDSN(int launchVehicleId, int orbit, string command)
        {
            if (!launchVehicles.ContainsKey(launchVehicleId)) {
                XElement vehicleData = dsnService.GetLaunchVehicleData(launchVehicleId.ToString());
                int launchVehicleOrbit = Int32.Parse(vehicleData.Element("orbit").Value);
                launchVehicles[launchVehicleId] = new LaunchVehicle(launchVehicleId, launchVehicleOrbit);
            }
            switch (command) {
                case "DEPLOY_PAYLOAD":
                    XElement launchVehicle = dsnService.GetLaunchVehicleData(launchVehicleId.ToString());
                    if (launchVehicle.Element("status").Value == "active") {
                        launchVehicles[launchVehicleId].DeployPayload();
                    }
                    break;
                case "DEORBIT":
                    launchVehicles[launchVehicleId].StopTelemetry();
                    launchVehicles[launchVehicleId].Deorbit();
                    launchVehicles.Remove(launchVehicleId);
                    break;
                case "START_TELEMETRY":
                    launchVehicles[launchVehicleId].StartTelemetry();
                    break;
                case "STOP_TELEMETRY":
                    launchVehicles[launchVehicleId].StopTelemetry();
                    break;
                default:
                    break;
            }
        }
    }
}
