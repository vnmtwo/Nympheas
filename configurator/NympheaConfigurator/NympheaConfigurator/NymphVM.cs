using LiveCharts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NympheaConfigurator
{

    class NymphVM : ViewModelBase
    {
        private NymphModel Model { get; }
        public NymphLogger Logger { get; }

        public string RemoteAddress
        {
            get
            {
                return Model.RemoteDeviceIP.ToString();
            }
            set
            {
                Model.RemoteDeviceIP = IPAddress.Parse(value);
                OnProperyChanged();
            }
        }
        public string RemotePort
        {
            get
            {
                return Model.RemoteDevicePort.ToString();
            }
            set
            {
                Model.RemoteDevicePort = int.Parse(value);
                OnProperyChanged();
            }
        }
        public string LocalPort
        {
            get
            {
                return Model.LocalUdpPort.ToString();
            }
            set
            {
                Model.LocalUdpPort = int.Parse(value);
            }
        }
        public string RemoteID
        {
            get
            {
                return Model.RemoteDeviceID.ToString();
            }
            set
            {
                Model.RemoteDeviceID = byte.Parse(value);
                OnProperyChanged();
            }
        }
        public string Log
        {
            get
            {
                return Logger.GetLog();
            }
        }
        
        public TemperatureControl TC
        {
            get
            {
                return Model.TemperatureControl;
            }
        }


        public SettingsViewModel Settings
        {
            get; set;
        }

        #region commands
        private RelayCommand getSettingsCmd;
        public RelayCommand GetSettingsCmd
        {
            get
            {
                return getSettingsCmd ??
                  (getSettingsCmd = new RelayCommand(obj =>
                  {
                      Model.GetSettings();
                  }));
            }
        }

        private RelayCommand rebootDeviceCmd;
        public RelayCommand RebootDeviceCmd
        {
            get
            {
                return rebootDeviceCmd ??
                  (rebootDeviceCmd = new RelayCommand(obj =>
                  {
                      Model.RebootDevice();
                  }));
            }
        }

        private RelayCommand defaultCmd;
        public RelayCommand DefaultCmd
        {
            get
            {
                return defaultCmd ??
                  (defaultCmd = new RelayCommand(obj =>
                  {
                      Model.SetDefaults();
                  }));
            }
        }

        private RelayCommand setIPModeCmd;
        public RelayCommand SetIPModeCmd
        {
            get
            {
                return setIPModeCmd ??
                  (setIPModeCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_device_ip_mode);
                      c.Add(Model.NympheaSettings.ethernet_ip_mode);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setEthernetDeviceIPCmd;
        public RelayCommand SetEthernetDeviceIPCmd
        {
            get
            {
                return setEthernetDeviceIPCmd ??
                  (setEthernetDeviceIPCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_device_ip);
                      c.AddRange(Model.NympheaSettings.ethernet_device_ip);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setEthernetDeviceDNSCmd;
        public RelayCommand SetEthernetDeviceDNSCmd
        {
            get
            {
                return setEthernetDeviceDNSCmd ??
                  (setEthernetDeviceDNSCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_dns);
                      c.AddRange(Model.NympheaSettings.ethernet_dns);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setEthernetDeviceSubnetCmd;
        public RelayCommand SetEthernetDeviceSubnetCmd
        {
            get
            {
                return setEthernetDeviceSubnetCmd ??
                  (setEthernetDeviceSubnetCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_subnet);
                      c.AddRange(Model.NympheaSettings.ethernet_subnet);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setEthernetDeviceGWCmd;
        public RelayCommand SetEthernetDeviceGWCmd
        {
            get
            {
                return setEthernetDeviceGWCmd ??
                  (setEthernetDeviceGWCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_gw);
                      c.AddRange(Model.NympheaSettings.ethernet_gw);
                      Model.SendCommand(c);
                  }));
            }
        }

        private RelayCommand setServiceIPModeCmd;
        public RelayCommand SetServiceIPModeCmd
        {
            get
            {
                return setServiceIPModeCmd ??
                  (setServiceIPModeCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_service_ip_mode);
                      c.Add(Model.NympheaSettings.ethernet_service_ip_mode);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setServiceIPCmd;
        public RelayCommand SetServiceIPCmd
        {
            get
            {
                return setServiceIPCmd ??
                  (setServiceIPCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_service_ip);
                      c.AddRange(Model.NympheaSettings.ethernet_service_ip);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setServicePortCmd;
        public RelayCommand SetServicePortCmd
        {
            get
            {
                return setServicePortCmd ??
                  (setServicePortCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_servcie_port);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.ethernet_servicePort));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setTemperatureTresholdCmd;
        public RelayCommand SetTemperatureTresholdCmd
        {
            get
            {
                return setTemperatureTresholdCmd ??
                  (setTemperatureTresholdCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_temperature_treshold);
                      c.Add(Model.NympheaSettings.temperature_treshold);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorCurrentCmd;
        public RelayCommand SetMotorCurrentCmd
        {
            get
            {
                return setMotorCurrentCmd ??
                  (setMotorCurrentCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_current);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_current));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorStepsDividerCmd;
        public RelayCommand SetMotorStepsDividerCmd
        {
            get
            {
                return setMotorStepsDividerCmd ??
                  (setMotorStepsDividerCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_steps_divider);
                      c.Add(Model.NympheaSettings.motor_steps_divider);
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorPollTimerCmd;
        public RelayCommand SetMotorPollTimerCmd
        {
            get
            {
                return setMotorPollTimerCmd ??
                  (setMotorPollTimerCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_poll_timer);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_poll_timer));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorPollTimerEEPROMCmd;
        public RelayCommand SetMotorPollTimerEEPROMCmd
        {
            get
            {
                return setMotorPollTimerEEPROMCmd ??
                  (setMotorPollTimerEEPROMCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_poll_timer_eeprom);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_poll_timer));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorSpeedCmd;
        public RelayCommand SetMotorSpeedCmd
        {
            get
            {
                return setMotorSpeedCmd ??
                  (setMotorSpeedCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_speed);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_speed));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorSpeedEEPROMCmd;
        public RelayCommand SetMotorSpeedEEPROMCmd
        {
            get
            {
                return setMotorSpeedEEPROMCmd ??
                  (setMotorSpeedEEPROMCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_speed_eeprom);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_speed));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorAccelerationCmd;
        public RelayCommand SetMotorAccelerationCmd
        {
            get
            {
                return setMotorAccelerationCmd ??
                  (setMotorAccelerationCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_acceleration);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_acceleration));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorAccelerationEEPROMCmd;
        public RelayCommand SetMotorAccelerationEEPROMCmd
        {
            get
            {
                return setMotorAccelerationEEPROMCmd ??
                  (setMotorAccelerationEEPROMCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_acceleration_eeprom);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_acceleration));
                      Model.SendCommand(c);
                  }));
            }
        }
        private RelayCommand setMotorBottomMarginCmd;
        public RelayCommand SetMotorBottomMarginCmd
        {
            get
            {
                return setMotorBottomMarginCmd ??
                  (setMotorBottomMarginCmd = new RelayCommand(obj =>
                  {
                      List<byte> c = new List<byte>();
                      c.Add((byte)ServiceCommands.set_motor_bottom_margin);
                      c.AddRange(BitConverter.GetBytes(Model.NympheaSettings.motor_bottom_margin));
                      Model.SendCommand(c);
                  }));
            }
        }
        #endregion

        public NymphVM(NymphModel model)
        {
            Model = model;
            Logger = NymphLogger.getInstance(model);

            Settings = new SettingsViewModel(Model);

            Logger.PropertyChanged += Logger_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

            var task = Task.Run(async () => {
                for (; ; )
                {
                    await Task.Delay(1 * 1000);
                    OnProperyChanged("Log");
                }
            });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Settings.RasieAllPropertiesChanged();
        }
        private void Logger_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //OnProperyChanged("Log");
        }
    }
}
