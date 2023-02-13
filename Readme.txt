Instructions for Starting the Project:

1. Visual Studio is required to run the project
2. Set the properties of the solution to start all four projects.
3. Use the start button in Visual Studio to launch the project.
4. The web application will automatically open in the default browser at the following address: https://localhost:44334/.
5. Use the following route to create a new spacecraft by attaching the relevant CSV files for different payload types: https://localhost:44334/LaunchSpacecraft.
6. Use the following route to send commands to the launch vehicle and payload: https://localhost:44334/SendCommand.
7. The following route lists all active and waiting spacecrafts: https://localhost:44334/.


Project Description:

This project is built using ASP.NET WCF Service and ASP.NET Web Application. It contains three web services and one ASP.NET web application. The three web services are DSN Service, Launch Vehicle Service, and Payload Service. The DSN Web Application serves as the project dashboard, providing control over all the functionalities.

Web Application Pages:

The Web Application consists of four pages:

1. Main Page: This page contains two tabs displaying the active and waiting launch vehicles.
2. Send Command Page: This page displays all the spacecrafts and allows users to send commands.
3. Spacecraft Info Page: Users can select a spacecraft to display launch vehicle and payload details. This page has three tabs: Launch Vehicle Telemetry Data, Payload Telemetry Data, and Payload Data.

Sequence of Actions:

1. Click the button on the top right to open a new spacecraft page.
2. Input the CSV file containing payload and launch vehicle data to the form.
3. After launch, the telemetry for launch vehicle starts automatically. Users can stop or start the telemetry as desired.
4. Once the launch vehicle status turns active, users can deploy the payload.
5. Deploy Payload allows users to retrieve data from the payload and start telemetry for the payload.
6. Users can start and stop payload data using Start Data and Stop Data actions respectively.
7. There are options to start and stop telemetry data for the payload.

Data Storage and Communication:

Data storage of launch vehicles, payloads, launch vehicle telemetry, payload telemetry, and payload transmission data is done using XML files. Communication between services occurs via SOAP requests.

Future Enhancements:

- In the event that the launch vehicle is in the waiting state and the service is abruptly shut down, the launch vehicle will remain in that state indefinitely. 
- To avoid this situation, a more sophisticated process can be implemented to enable a graceful start for the launch vehicle.