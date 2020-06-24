using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// Model for flight Simulator.
    /// </summary>
    public class MySimulatorModel : ISimulatorModel
    {
        private Mutex mutex;
        public event PropertyChangedEventHandler PropertyChanged;
        public MyTelnetClient telnetClient;

        // Constructor.
        public MySimulatorModel(IConfiguration config)
        {
            mutex = new Mutex();
            telnetClient = new MyTelnetClient();
            string ip = config.GetConnectionString("IP");
            string socket_port = config.GetConnectionString("socket_port");
            this.Connect(ip, socket_port);
            telnetClient.Write("data\n");
        }

        // Connect to the server.
        public void Connect(string ip, string port)
        {
            try
            {
                if (!telnetClient.isConnect)
                {
                    //mutex = new Mutex();
                    telnetClient.Connect(ip, port);
                }
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }
        
        public void SetThrottle(string s)
        {
            try
            {
                mutex.WaitOne();
                string toSend = "set " + "/controls/engines/current-engine/throttle " + s + "\n";
                telnetClient.Write(toSend);
                ValidSet(s, "Throttle");
                mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }

        public void SetRudder(string s)
        {
            try
            {
                mutex.WaitOne();
                string toSend = "set " + "/controls/flight/rudder " + s + "\n";
                telnetClient.Write(toSend);
                ValidSet(s, "Rudder");
                mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }

        public void SetElevator(string s)
        {
            try
            {
                mutex.WaitOne();
                string toSend = "set " + "/controls/flight/elevator " + s + "\n";
                telnetClient.Write(toSend);
                ValidSet(s, "Elevator");
                mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }

        public void SetAileron(string s)
        {
            try
            {
                mutex.WaitOne();
                string toSend = "set " + "/controls/flight/aileron " + s + "\n";
                telnetClient.Write(toSend);
                ValidSet(s, "Aileron");
                mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }

        public void ValidSet(string source, string variable)
        {
            string var = "";
            switch (variable)
            {
                case "Throttle":
                    var = "/controls/engines/engine[0]/throttle";
                    break;
                case "Aileron":
                    var = "/controls/flight/aileron";
                    break;
                case "Elevator":
                    var = "/controls/flight/elevator";
                    break;
                case "Rudder":
                    var = "/controls/flight/rudder";
                    break;
                default:
                    Console.WriteLine("json file invalid");
                    return;
            }
            mutex.WaitOne();
            telnetClient.Write("get " + var + "\n");
            string current = telnetClient.Read();
            double d1, d2;
            //double d1 = Double.Parse(source);
            //double d2 = Double.Parse(current);
            if (!Double.TryParse(source, out d1))
            {
                Console.WriteLine("json file invalid");
                return;
            }
            if (!Double.TryParse(current, out d2))
            {
                Console.WriteLine("get invalid value from the simulator");
                return;
            }
            string s1 = String.Format("{0:0.00}", d1);
            string s2 = String.Format("{0:0.00}", d2);
            if (!s1.Equals(s2))
            {
                Console.WriteLine("need to be " + source + " but get: " + current);
                // Error
                Console.WriteLine("error post " + variable);
            }
            mutex.ReleaseMutex();
        }

        // Error message property
        private String error = "";
        public String Error
        {
            get { return this.error; }
            set
            {
                
            }
        }

    }
}
