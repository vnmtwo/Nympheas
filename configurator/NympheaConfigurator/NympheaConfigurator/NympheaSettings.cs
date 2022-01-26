using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NympheaConfigurator
{
	//public class NympheaSettings
	//{
	//	//Ethernet section
	//	byte ethernet_ip_mode = (byte)IPMode.Factory;
	//	byte[] ethernet_device_ip = new byte[4];
	//	byte[] ethernet_dns = new byte[4];
	//	byte[] ethernet_subnet = new byte[4];
	//	byte[] ethernet_gw = new byte[4];
	//	byte ethernet_service_ip_mode = (byte)ServiceIPMode.TalkBack;
	//	byte[] ethernet_service_ip = new byte[4];
	//	ushort ethernet_servicePort = 8889;

	//	////Motor section
	//	ushort motor_current = 800;
	//	byte motor_steps_divider = 32;
	//	ushort motor_acceleration = 3200;
	//	ushort motor_speed = 3200;
	//	ushort motor_bottom_margin = 400;
	//	ushort motor_poll_imer = 1500;

	//	////Temperature section
	//	byte temperature_treshold = 50;
	//}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class NympheaSettings
	{
		public byte ethernet_ip_mode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] ethernet_device_ip;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] ethernet_dns;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] ethernet_subnet;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] ethernet_gw;
		public byte ethernet_service_ip_mode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] ethernet_service_ip;
		public ushort ethernet_servicePort;

		////Motor section
		public ushort motor_current;
		public byte motor_steps_divider;
		public ushort motor_acceleration;
		public ushort motor_speed;
		public ushort motor_bottom_margin;
		public ushort motor_poll_timer;

		////Temperature section
		public byte temperature_treshold;
		public NympheaSettings()
		{

		}
		public NympheaSettings(bool defaultValues)
		{
			if (defaultValues)
			{
				ethernet_ip_mode = (byte)IPMode.Factory;
				ethernet_device_ip = new byte[] { 192, 168, 2, 2 };
				ethernet_dns = new byte[] { 192, 168, 2, 1 };
				ethernet_subnet = new byte[] { 255, 255, 255, 0};
				ethernet_gw = new byte[] { 192, 168, 2, 1 };
				ethernet_service_ip_mode = (byte)ServiceIPMode.TalkBack;
				ethernet_service_ip = new byte[]{ 192, 168, 2, 1 };
				ethernet_servicePort = 8889;

				////Motor section
				motor_current = 800;
				motor_steps_divider = 32;
				motor_acceleration = 3200;
				motor_speed = 3200;
				motor_bottom_margin = 400;
				motor_poll_timer = 1500;

				//Temperature section
				temperature_treshold = 50;
			}
			else
			{
				ethernet_ip_mode = 0;
				ethernet_device_ip = new byte[] { 0, 0, 0, 0 };
				ethernet_dns = new byte[] { 0, 0, 0, 0 };
				ethernet_subnet = new byte[] { 0, 0, 0, 0 };
				ethernet_gw = new byte[] { 0, 0, 0, 0 };
				ethernet_service_ip_mode = 0;
				ethernet_service_ip = new byte[] { 0, 0, 0, 0 };
				ethernet_servicePort = 0;

				////Motor section
				motor_current = 0;
				motor_steps_divider = 0;
				motor_acceleration = 0;
				motor_speed = 0;
				motor_bottom_margin = 0;
				motor_poll_timer = 0;

				//Temperature section
				temperature_treshold = 0;
			}
		}
		public static byte[] StringToIP(string ipstring)
		{
			try
			{
				IPAddress ipa = IPAddress.Parse(ipstring);
				return ipa.GetAddressBytes();
			}
			catch
			{

			}
			return new byte[] { 0, 0, 0, 0 };
		}
	}
}
