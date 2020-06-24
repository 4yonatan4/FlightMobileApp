using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// Model Interface
    /// </summary>
    public interface ISimulatorModel : INotifyPropertyChanged
    {
        // Telnet Clinet methods.
        void Connect(string ip, string port);

        string Error { set; get; }
        // Set methods.
        void SetAileron(string s);
        void SetElevator(string s);
        void SetRudder(string s);
        void SetThrottle(string s);
    }
}
