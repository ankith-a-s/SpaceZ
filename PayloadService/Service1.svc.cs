using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace PayloadService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public static Dictionary<int, Payload> payloads = new Dictionary<int, Payload>();
        public DSNService.Service1Client dsnService = new DSNService.Service1Client();

        /**
         * Receive the command from DSN and perform appropriate action for the command
         */
        public string receiveCommandFromDSN(int payloadId, string type, string command)
        {
            XElement payloadData = dsnService.GetPayloadData(payloadId.ToString());
            string deployed = payloadData.Element("deployed").Value;
            if (deployed == "false")
            {
                return "Payload is not deployed";
            }
            if (!payloads.ContainsKey(payloadId))
            {
                string payloadType = payloadData.Element("type").Value;
                int orbit = Int32.Parse(payloadData.Element("orbit").Value);
                payloads[payloadId] = new Payload(payloadId, payloadType, orbit);
            }
            switch (command)
            {
                case "START_DATA":
                    payloads[payloadId].StartData();
                    break;
                case "STOP_DATA":
                    payloads[payloadId].StopData();
                    break;
                case "DECOMMISSION":
                    payloads[payloadId].StopData();
                    payloads[payloadId].StopTelemetry();
                    payloads[payloadId].Decommission();
                    payloads.Remove(payloadId);
                    break;
                case "START_TELEMETRY":
                    payloads[payloadId].StartTelemetry();
                    break;
                case "STOP_TELEMETRY":
                    payloads[payloadId].StopTelemetry();
                    break;
            }
            return "Success";
        }

        /**
         * Receive command from launch vehicle for the give payloadId
         */
        public void receiveCommandFromLaunchVehicle(int payloadId)
        {
            if (!payloads.ContainsKey(payloadId))
            {
                XElement payloadData = dsnService.GetPayloadData(payloadId.ToString());
                string type = payloadData.Element("type").Value;
                int orbit = Int32.Parse(payloadData.Element("orbit").Value);
                payloads[payloadId] = new Payload(payloadId, type, orbit);
                dsnService.receiveCommandFromPayload(payloadId, "PAYLOAD_DEPLOYED", new XElement("test"));
            }
        }
    }
}
