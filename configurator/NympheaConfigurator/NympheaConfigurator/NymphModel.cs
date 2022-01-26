using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NympheaConfigurator
{
    public class NymphModel:ViewModelBase
    {
        private NymphLogger Logger;
        
        private IPAddress remoteDeviceIP = IPAddress.Parse(Properties.Settings.Default.DeviceIP);
        public IPAddress RemoteDeviceIP

        {
            get
            {
                return remoteDeviceIP;
            }
            set
            {
                remoteDeviceIP = value;
                Properties.Settings.Default.DeviceIP = remoteDeviceIP.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private int localUdpPort = Properties.Settings.Default.LocalPort;
        private bool changePort = false;
        public int LocalUdpPort
        {
            get
            {
                return localUdpPort;
            }
            set
            {
                if (localUdpPort != value)
                {
                    localUdpPort = value;
                    if (tokenSource2 != null)
                    {
                        tokenSource2.Cancel();
                        changePort = true;
                        
                    }
                    else
                    {
                        ParseUDP();
                    }
                }
                Properties.Settings.Default.LocalPort = value;
                Properties.Settings.Default.Save();
            }
        }

        private int remoteDevicePort = Properties.Settings.Default.DevicePort;
        public int RemoteDevicePort

        {
            get
            {
                return remoteDevicePort;
            }
            set
            {
                remoteDevicePort = value;
                Properties.Settings.Default.DevicePort = remoteDevicePort;
                Properties.Settings.Default.Save();
            }
        }

        private byte remoteDeviceID = (byte)Properties.Settings.Default.DeviceID;
        public byte RemoteDeviceID
        {
            get
            {
                return remoteDeviceID;
            }
            set
            {
                remoteDeviceID = value;
                Properties.Settings.Default.DeviceID = remoteDeviceID;
                Properties.Settings.Default.Save();
            }
        }

        private NympheaSettings nympheaSettings;
        public NympheaSettings NympheaSettings
        {
            get
            {
                return nympheaSettings;
            }
            set
            {
                nympheaSettings = value;
                OnProperyChanged();
            }
        }
        public TemperatureControl TemperatureControl;


        public NymphModel()
        {
            NympheaSettings = new NympheaSettings(false);
            Logger = NymphLogger.getInstance(this);
            TemperatureControl = new TemperatureControl();
            StartUDPReceiver();
        }

        private void StartUDPReceiver()
        {

            ParseUDP();
        }

        CancellationTokenSource tokenSource2;
        CancellationToken ct;
        async private void ParseUDP()
        {

            UdpClient UdpReceiver;
            try
            {
                UdpReceiver = new UdpClient(new IPEndPoint(IPAddress.Any, LocalUdpPort));
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Port alredy used",
                                          "Port is busy",
                                          MessageBoxButton.OK);
                return;
            }
            while (!changePort)
            {
                tokenSource2 = new CancellationTokenSource();
                ct = tokenSource2.Token;

                Task<UdpReceiveResult> t = UdpReceiver.ReceiveAsync();
                try
                {
                    UdpReceiveResult r = await t.ContinueWith(x => x.Result, ct);

                    if (r.Buffer.Length > 0)
                    {
                        Logger.Add(r.Buffer);
                        if (r.Buffer[0] == 's')
                        {
                            if (r.Buffer[1] < 255 && r.Buffer.Length > 10)
                            {
                                if (r.Buffer[2] == (byte)ServiceCommands.get_all_settings)
                                {
                                    byte[] buffer = r.Buffer.Skip(3).ToArray();
                                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                                    NympheaSettings = (NympheaSettings)Marshal.PtrToStructure(
                                        handle.AddrOfPinnedObject(),
                                        typeof(NympheaSettings));
                                    handle.Free();
                                }
                            }
                        }
                        if (r.Buffer[0] == 't')
                        {
                            TemperatureControl.ParsePacket(r.Buffer);
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    tokenSource2.Dispose();
                }
            }

            changePort = false;
            UdpReceiver.Close();
            StartUDPReceiver();
        }

        internal void RebootDevice()
        {
            SendCommand(new byte[] { (byte)ServiceCommands.reboot });
        }

        public void GetSettings()
        {
            SendCommand(new byte[] { (byte)ServiceCommands.get_all_settings });
        }

        internal void SetDefaults()
        {
            NympheaSettings = new NympheaSettings(true);
        }

        internal void SendCommand(IEnumerable<byte> command)
        {
            UdpClient UdpSender = new UdpClient();
            UdpSender.Connect(new IPEndPoint(RemoteDeviceIP, RemoteDevicePort));
            List<byte> data = new List<byte>() { (byte)'s', RemoteDeviceID};
            data.AddRange(command);
            UdpSender.Send(data.ToArray(), data.Count);
        }


    }
}
