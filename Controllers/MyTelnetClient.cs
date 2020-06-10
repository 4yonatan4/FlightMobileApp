using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// Telnet Client Class
    /// </summary>
    public class MyTelnetClient
    {
        TcpClient clientSocket;
        StreamReader sr;
        StreamWriter sw;
        public Boolean isConnect = false;

        // Make the connection with the server.
        public void Connect(string ip, string port)
        {
            try
            {
                if (!isConnect)
                {
                    clientSocket = new TcpClient();
                    // Try to connect.
                    var result = clientSocket.BeginConnect(ip, Int32.Parse(port), null, null);
                    // Wait 1 seconds if the server is not responding.
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                    // If failed, throw exception.
                    if (!success)
                    {
                        throw new Exception("Failed to connect.");
                    }

                    // We have connected
                    clientSocket.EndConnect(result);
                    sr = new StreamReader(clientSocket.GetStream());
                    sw = new StreamWriter(clientSocket.GetStream());
                    isConnect = true;
                    // (Application.Current as App).MainViewModel.model.Error = "Connection Established!\n";
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Write to the server.
        public void Write(string command)
        {
            try
            {
                if (isConnect)
                {
                    sw.WriteLine(command);
                    sw.Flush();
                    // Console.WriteLine("Sent: " + command);
                }
            }
            catch(IOException)
            {
                // (Application.Current as App).MainViewModel.model.Error = "Error in Connection. Press \"Connect\" to reconnect.\n";
                Disconnect();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Read from the server.
        public string Read()
        {
            try
            {
                if (isConnect)
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    string data = sr.ReadLine();
                    // Check if the server is not responsing for 10 seconds.
                    if (stopWatch.ElapsedMilliseconds > 10000)
                    {
                        // (Application.Current as App).MainViewModel.model.Error = "Server slows down\n";
                        return data;
                    }
                    if (double.TryParse(data, out _))
                    {
                        return data;
                    }
                    else
                    {
                        // (Application.Current as App).MainViewModel.model.Error = "Invalid values!\n";
                    }
                }
                return null;
            }
            catch (IOException)
            {
                // (Application.Current as App).MainViewModel.model.Error = "Error in Connection. Press \"Connect\" to reconnect.\n";
                Disconnect();
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Disconnect from the server.
        public void Disconnect()
        {
            if (isConnect)
            {
                if (sr != null)
                    sr.Close();
                if (sw != null)
                    sw.Close();
                if (clientSocket != null)
                    clientSocket.Close();
                isConnect = false;
            }
            
        }
    }
}
