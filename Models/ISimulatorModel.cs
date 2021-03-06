﻿using System;
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
        void Disconnect();

        string Error { set; get; }
        void SetAileron(string s);
        void SetElevator(string s);
        void SetRudder(string s);
        void SetThrottle(string s);
    }
}
