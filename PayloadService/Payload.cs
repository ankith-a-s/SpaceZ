using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Xml.Linq;

namespace PayloadService
{
    public class Payload
    {
        private int payloadId, orbit;
        private string type;
        private static System.Timers.Timer payloadTeleTimer;
        private static System.Timers.Timer payloadDataTimer;
        private bool sendTelemetry = false;
        private bool sendTransmissionData = false;
        DSNService.Service1Client dsnService = new DSNService.Service1Client();

        public Payload(int payloadId, string type, int orbit)
        {
            this.payloadId = payloadId;
            this.type = type;
            this.orbit = orbit;
            SetTimers();
        }

        private void SetTimers() {
            SetPayloadTimer();
            SetPayloadDataTimer();
        }

        private void SetPayloadTimer() {
            int timer = 0;
            switch (type)
            {
                case "Communication":
                    timer = 60000;
                    break;
                case "Scientific":
                    timer = 5000;
                    break;
                case "Spy":
                    timer = 10000;
                    break;
            }
            payloadDataTimer = new System.Timers.Timer(timer);
            payloadDataTimer.Elapsed += OnPayloadDataEvent;
            payloadDataTimer.AutoReset = true;
            payloadDataTimer.Enabled = true;
        }

        private void SetPayloadDataTimer() {
            payloadTeleTimer = new System.Timers.Timer(1000);
            payloadTeleTimer.Elapsed += OnPayloadTeleEvent;
            payloadTeleTimer.AutoReset = true;
            payloadTeleTimer.Enabled = true;
        }

        public void StartTelemetry()
        {
            this.sendTelemetry = true;
        }

        public void StopTelemetry()
        {
            this.sendTelemetry = false;
        }

        public void StartData()
        {
            this.sendTransmissionData = true;
        }

        public void StopData()
        {
            this.sendTransmissionData = false;
        }

        public void Decommission() {
            dsnService.receiveCommandFromPayload(this.payloadId, "DECOMMISSION", new XElement("test"));
        }

        private void OnPayloadDataEvent(Object source, ElapsedEventArgs e)
        {
            if (!this.sendTransmissionData) {
                return;
            }
            XElement payloadData = new XElement("payloadData");
            switch (this.type) {
                case "Communication":
                    payloadData.Add(new XElement("uplink", new Random().Next(50, 5000) + "Mbps"));
                    payloadData.Add(new XElement("downlink", new Random().Next(50, 5000) + "Mbps"));
                    payloadData.Add(new XElement("activeTransponders", new Random().Next(50, 500)));
                    payloadData.Add(new XElement("type", this.type));
                    break;
                case "Scientific":
                    payloadData.Add(new XElement("rainfall", new Random().Next(30, 60) + "mm"));
                    payloadData.Add(new XElement("humidity", new Random().Next(1, 100) + "%"));
                    payloadData.Add(new XElement("snow", new Random().Next(2, 20) + "in"));
                    payloadData.Add(new XElement("type", this.type));
                    break;
                case "Spy":
                    // Using the random image api for spy payload type
                    payloadData.Add(new XElement("imageData", "https://loremflickr.com/320/240?random=" + new Random().Next(1, 1000)));
                    payloadData.Add(new XElement("type", this.type));
                    break;
            }
            payloadData.Add(new XElement("createdDate", GetTimestamp(DateTime.Now)));
            payloadData.Add(new XElement("payloadId", this.payloadId));
            dsnService.receiveCommandFromPayload(this.payloadId, "DATA", payloadData);
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, 2);
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm:ss");
        }

        private void OnPayloadTeleEvent(Object source, ElapsedEventArgs e)
        {
            if (!this.sendTelemetry)
            {
                return;
            }
            XElement payloadTelemetry = CreatePayloadTelemetryElement();
            dsnService.receiveCommandFromPayload(this.payloadId, "TELEMETRY_DATA", payloadTelemetry);
        }

        private XElement CreatePayloadTelemetryElement() {
            XElement payloadTelemetry = new XElement("payloadTelemetry");
            payloadTelemetry.Add(new XElement("altitude", this.orbit));
            payloadTelemetry.Add(new XElement("latitude", GetRandomNumber(-90, 90)));
            payloadTelemetry.Add(new XElement("longitude", GetRandomNumber(-180, 180)));
            payloadTelemetry.Add(new XElement("temperature", new Random().Next(600, 900)));
            payloadTelemetry.Add(new XElement("payloadId", this.payloadId));
            payloadTelemetry.Add(new XElement("createdDate", GetTimestamp(DateTime.Now)));
            return payloadTelemetry;
        }
    }
}