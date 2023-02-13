using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Linq;

namespace DSNService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        IEnumerable<XElement> GetPayloadTelemetryData(string payloadId);

        [OperationContract]
        IEnumerable<XElement> GetPayloadProcessedData(string payloadId);

        [OperationContract]
        IEnumerable<XElement> GetLaunchVehicleTelemetryData(string launchVehicleId);

        [OperationContract]
        XElement GetLaunchVehicleData(string launchVehicleId);

        [OperationContract]
        XElement GetPayloadData(string payloadId);

        [OperationContract]
        IEnumerable<XElement> GetAllLaunchVehicleData();

        [OperationContract]
        Boolean launchSpacecraft(string name, int orbit, string payloadName, string payloadType);

        [OperationContract]
        void receiveCommandFromLaunchVehicle(int launchVehicleId, string command, XElement payloadData);
        [OperationContract]
        void receiveCommandFromPayload(int payloadId, string command, XElement payloadData);
    }
}
