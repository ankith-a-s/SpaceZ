using System.ServiceModel;

namespace LaunchVehicleService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void receiveCommandFromDSN(int launchVehicleId, int orbit, string command);
    }
}
