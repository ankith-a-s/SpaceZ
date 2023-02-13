using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Timers;
using System.Xml.Linq;

namespace LaunchVehicleService
{
    public class LaunchVehicle
    {
        private int orbit, launchVehicleId, timeToOrbit;
        private bool sendTelemetry = true;
        private static System.Timers.Timer launchVehTimer;
        private static System.Timers.Timer simulLaunchTimer;
        private static System.Timers.Timer telemetryTimer;
        private DSNService.Service1Client dsnService = new DSNService.Service1Client();
        private PayloadService.Service1Client payloadService = new PayloadService.Service1Client();


        public LaunchVehicle(int launchVehicleId, int orbit)
        {
            this.orbit = orbit;
            this.timeToOrbit = ((orbit / 3600)) + 10;
            this.launchVehicleId = launchVehicleId;

            SetTimers();
        }

        /**
         * Method to start telemetry data
         */
        public void StartTelemetry()
        {
            this.sendTelemetry = true;
        }

        /**
         * Method to stop telemetry data
         */
        public void StopTelemetry()
        {
            this.sendTelemetry = false;
        }

        /**
         * Method to deply payload
         */
        public void DeployPayload()
        {
            XElement vehicleData = dsnService.GetLaunchVehicleData(this.launchVehicleId.ToString());
            payloadService.receiveCommandFromLaunchVehicle(Int32.Parse(vehicleData.Element("payloadId").Value));
        }

        public void Deorbit()
        {
            launchVehTimer.Stop();
            telemetryTimer.Stop();
            simulLaunchTimer.Stop();
            dsnService.receiveCommandFromLaunchVehicle(this.launchVehicleId, "DEORBIT", new XElement("default"));
        }

        /**
         * Set launchvehicle, simulateLaunch and telemetry timers
         */
        private void SetTimers()
        {
            SetLaunchVehTimer();
            SetSimulLaunchTimer();
            SetTelemetryTimer();
        }

        /**
         * Set launchvehicle timer
         */
        private void SetLaunchVehTimer()
        {
            launchVehTimer = new Timer(((this.orbit / 3600) + 10) * 1000);
            launchVehTimer.Elapsed += OnLVIntoOrbitEvent;
            launchVehTimer.AutoReset = false;
            launchVehTimer.Enabled = true;
        }

        /**
         * Set simulatelaunch timer
         */
        private void SetSimulLaunchTimer()
        {
            simulLaunchTimer = new Timer(1000);
            simulLaunchTimer.Elapsed += OnSimulLaunchEvent;
            simulLaunchTimer.AutoReset = true;
            simulLaunchTimer.Enabled = true;
        }

        /**
         * Set Telemetry timer
         */
        private void SetTelemetryTimer()
        {
            telemetryTimer = new System.Timers.Timer(1000);
            telemetryTimer.Elapsed += OnSendTelemetryEvent;
            telemetryTimer.AutoReset = true;
            telemetryTimer.Enabled = true;
        }

        /**
         * Handles the simulate launch timer event
         */
        private void OnSimulLaunchEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (this.timeToOrbit > 0)
                {
                    timeToOrbit -= 1;
                }
            }
            catch
            {

            }
        }

        /**
         * Handles the send telemetry timer event
         */
        private void OnSendTelemetryEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (!this.sendTelemetry)
                {
                    return;
                }

                int time = (orbit / 3600) + 10;
                XElement launchVehicleTelemetry = CreateLVTeleEle(time);
                dsnService.receiveCommandFromLaunchVehicle(this.launchVehicleId, "TELEMETRY_DATA", launchVehicleTelemetry);
            }
            catch { }
        }

        /**
         * Create launch vehicle telemetry element using the data
         */
        private XElement CreateLVTeleEle(int time)
        {
            XElement launchVehicleTelemetry = new XElement("launchVehicleTelemetry");
            launchVehicleTelemetry.Add(new XElement("altitude", orbit - ((orbit * timeToOrbit) / time)));
            launchVehicleTelemetry.Add(new XElement("latitude", GetRandomNumber(-90, 90)));
            launchVehicleTelemetry.Add(new XElement("longitude", GetRandomNumber(-180, 180)));
            launchVehicleTelemetry.Add(new XElement("temperature", new Random().Next(600, 900)));
            launchVehicleTelemetry.Add(new XElement("timeToOrbit", timeToOrbit));
            launchVehicleTelemetry.Add(new XElement("launchVehicleId", this.launchVehicleId));
            launchVehicleTelemetry.Add(new XElement("createdDate", GetTimestamp(DateTime.Now)));
            return launchVehicleTelemetry;
        }

        /**
         * Handles the launch vehicle into orbit event
         */
        private void OnLVIntoOrbitEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                timeToOrbit = 0;
                launchVehTimer.Stop();
                simulLaunchTimer.Stop();
                dsnService.receiveCommandFromLaunchVehicle(this.launchVehicleId, "REACHED_ORBIT", new XElement("default"));
            }
            catch
            {

            }
        }

        /**
         * Gives the random number based on the range provided
         */
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, 2);
        }

        /**
         * Get timestamp string based on the datetime value provided
         */
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
}