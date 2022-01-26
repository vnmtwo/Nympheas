using System;
using System.Collections.Generic;

namespace NympheaConfigurator
{
    internal class LogItem
    {
        internal PacketType Type;
        internal byte DeviceId;
        internal string Message;
        internal DateTime Time;
    }
    internal class NymphLogger:ViewModelBase
    {
        private List<LogItem> Log { get; set; }
        private static NymphLogger instance;

        public bool ShowTemperature
        {
            get;
            set;
        } = false;
        public bool ShowServiceCommands
        {
            get;
            set;
        } = true;
        public bool ShowOnlySelectedDevice
        {
            get;
            set;
        } = false;

        private NymphModel Model;
        private NymphLogger(NymphModel model)
        {
            Log = new List<LogItem>();
            Model = model;
        }
        public static NymphLogger getInstance(NymphModel model)
        {
            if (instance == null)
                instance = new NymphLogger(model);
            return instance;
        }
        public void Add(byte[] buffer)
        {
            Log.Add(new LogItem
            {
                Type = (PacketType)Convert.ToChar(buffer[0]),
                DeviceId = buffer[1],
                Message = ArrayToHexString(buffer),
                Time = DateTime.Now
            });
            if (Log.Count > 300) Log.RemoveAt(0);
        }
        public string GetLog()
        {
            List<string> sl = new List<string>();
            for (int i = 0; i < Log.Count; i++)
            {
                if (ShowOnlySelectedDevice)
                {
                    if (Log[i].DeviceId != Model.RemoteDeviceID)
                        continue;
                }
                if (ShowTemperature)
                {
                    if (Log[i].Type == PacketType.Teperature)
                    {
                        sl.Add(Log[i].Time.ToString() + " " + Log[i].Message);
                    }
                }
                if (ShowServiceCommands)
                {
                    if (Log[i].Type == PacketType.CommandResponse)
                    {
                        sl.Add(Log[i].Time.ToString() + " " + Log[i].Message);
                    }
                }
            }
            return string.Join(System.Environment.NewLine, sl);
        }
        static string ArrayToHexString(byte[] a)
        {
            return string.Join(" ", Array.ConvertAll(a, b => b.ToString("X2")));
        }
    }
}