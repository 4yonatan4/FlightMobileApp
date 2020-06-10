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
                    mutex = new Mutex();
                    telnetClient.Connect(ip, port);
                }
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }

        // Disconnect from the server.
        public void Disconnect()
        {
            try
            {
                telnetClient.Disconnect();
            }
            catch (Exception e)
            {
                Error = e.Message + "\n";
            }
        }


        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
            }
            mutex.WaitOne();
            telnetClient.Write("get " + var + "\n");
            string current = telnetClient.Read();
            if (source != current)
            {
                // Error
                Console.WriteLine("error post" + variable);
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
                this.error += DateTime.Now.ToString("H:mm:ss : ") + value;
                NotifyPropertyChanged("Error");
            }
        }

        
    }
}
