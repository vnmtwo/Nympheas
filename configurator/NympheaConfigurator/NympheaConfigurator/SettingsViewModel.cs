using System;
using System.Reflection;

namespace NympheaConfigurator
{
    public class SettingsViewModel:ViewModelBase
    {
        public string[] IPModeItems
        {
            get;
        } = (string[])Enum.GetNames(typeof(IPMode));
        public string EthernetIPMode
        {
            get
            {
                return Enum.GetName(typeof(IPMode), Model.NympheaSettings.ethernet_ip_mode);
            }
            set
            {
                byte v = (byte)(IPMode)Enum.Parse(typeof(IPMode), value);
                Model.NympheaSettings.ethernet_ip_mode = v;
                OnProperyChanged();
            }
        }
        public string EthernetDeviceIP
        {
            get
            {
                return string.Join(".", Model.NympheaSettings.ethernet_device_ip);
            }
            set
            {
                Model.NympheaSettings.ethernet_device_ip = NympheaSettings.StringToIP(value);
                OnProperyChanged();

            }
        }
        public string EthernetDeviceDNS
        {
            get
            {
                return string.Join(".", Model.NympheaSettings.ethernet_dns);
            }
            set
            {
                Model.NympheaSettings.ethernet_dns = NympheaSettings.StringToIP(value);
                OnProperyChanged();
            }
        }
        public string EthernetDeviceSubnet
        {
            get
            {
                return string.Join(".", Model.NympheaSettings.ethernet_subnet);
            }
            set
            {
                Model.NympheaSettings.ethernet_subnet = NympheaSettings.StringToIP(value);
                OnProperyChanged();
            }
        }
        public string EthernetDeviceGW
        {
            get
            {
                return string.Join(".", Model.NympheaSettings.ethernet_gw);
            }
            set
            {
                Model.NympheaSettings.ethernet_gw = NympheaSettings.StringToIP(value);
                OnProperyChanged();
            }
        }
        public string[] ServiceIPModeItems 
        {
            get;
        } = (string[])Enum.GetNames(typeof(ServiceIPMode));
        public string ServiceIPMode
        {
            get
            {
                return Enum.GetName(typeof(ServiceIPMode), Model.NympheaSettings.ethernet_service_ip_mode);
            }
            set
            {
                byte v = (byte)(ServiceIPMode)Enum.Parse(typeof(ServiceIPMode), value);
                Model.NympheaSettings.ethernet_service_ip_mode = v;
                OnProperyChanged();
            }
        }
        public string ServiceIP
        {
            get
            {
                return string.Join(".", Model.NympheaSettings.ethernet_service_ip);
            }
            set
            {
                Model.NympheaSettings.ethernet_service_ip = NympheaSettings.StringToIP(value);
                OnProperyChanged();
            }
        }
        public string ServicePort
        {
            get
            {
                return Model.NympheaSettings.ethernet_servicePort.ToString();
            }
            set
            {
                Model.NympheaSettings.ethernet_servicePort = ushort.Parse(value);
                OnProperyChanged();
            }
        }
        public string TemperatureTreshold
        {
            get
            {
                return Model.NympheaSettings.temperature_treshold.ToString();
            }
            set
            {
                Model.NympheaSettings.temperature_treshold = byte.Parse(value);
                OnProperyChanged();
            }
        }
        public string MotorCurrent
        {
            get
            {
                return Model.NympheaSettings.motor_current.ToString();
            }
            set
            {
                Model.NympheaSettings.motor_current = ushort.Parse(value);
                OnProperyChanged();
            }
        }
        public string MotorStepsDivider
        {
            get
            {
                return Model.NympheaSettings.motor_steps_divider.ToString();
            }
            set
            {
                Model.NympheaSettings.motor_steps_divider = byte.Parse(value);
                OnProperyChanged();
            }
        }
        public string MotorPollTimer
        {
            get
            {
                return Model.NympheaSettings.motor_poll_timer.ToString();
            }
            set
            {
                Model.NympheaSettings.motor_poll_timer = ushort.Parse(value);
                OnProperyChanged();
            }
        }
        public string MotorSpeed
        {
            get
            {
                return Model.NympheaSettings.motor_speed.ToString();
            }
            set
            {
                Model.NympheaSettings.motor_speed = ushort.Parse(value);
                OnProperyChanged();
            }
        }
        public string MotorAcceleration
        {
            get
            {
                return Model.NympheaSettings.motor_acceleration.ToString();
            }
            set
            {
                Model.NympheaSettings.motor_acceleration = ushort.Parse(value);
                OnProperyChanged();
            }
        }
        public string MotorBottomMargin
        {
            get
            {
                return Model.NympheaSettings.motor_bottom_margin.ToString();
            }
            set
            {
                Model.NympheaSettings.motor_bottom_margin = ushort.Parse(value);
                OnProperyChanged();
            }
        }

        private NymphModel Model;

        public SettingsViewModel(NymphModel model)
        {
            Model = model;
        }

        public void RasieAllPropertiesChanged()
        {
            PropertyInfo[] myPropertyInfo;
            // Get the properties of 'Type' class object.
            myPropertyInfo = this.GetType().GetProperties();
                //Type.GetType("System.Type").GetProperties();
            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                OnProperyChanged(myPropertyInfo[i].Name);
            }
        }
    }
}