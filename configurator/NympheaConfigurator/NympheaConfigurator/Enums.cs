using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NympheaConfigurator
{
	public enum IPMode
	{
		Factory = 1, //use 192.168.2.<100+number from id>
		StartOctets = 2,
		WholeAddress = 3
	};
	public enum ServiceIPMode
	{
		TalkBack = 1, //use ip from first artnet control packet
		StaticAddress = 2
	};

	public enum PacketType
	{
		Teperature = 't',
		CommandResponse = 's'
	}

	public enum ServiceCommands
	{
		get_all_settings = 0,
		set_device_ip_mode = 1,
		set_device_ip = 2,
		set_dns = 3,
		set_subnet = 4,
		set_gw = 5,
		set_service_ip_mode = 6,
		set_service_ip = 7,
		set_servcie_port = 8,

		set_motor_current = 9,
		set_motor_steps_divider = 10,
		set_motor_acceleration = 11,
		set_motor_acceleration_eeprom = 12,
		set_motor_speed = 13,
		set_motor_speed_eeprom = 14,
		set_motor_bottom_margin = 15,
		set_motor_poll_timer = 16,
		set_motor_poll_timer_eeprom = 17,

		set_temperature_treshold = 18,
		reset_to_factory = 254,
		reboot = 255
	};
}
