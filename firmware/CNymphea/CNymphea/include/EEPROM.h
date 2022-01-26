#ifndef EEPROM_H_
	#define EEPROM_H_
	#include <avr/eeprom.h>

	enum IPMode{
		Factory = 1, //use 192.168.2.<100+number from id>
		StartOctets = 2,
		WholeAddress = 3
	};
	enum ServiceIPMode{
		TalkBack = 1, //use ip from first artnet control packet
		StaticAddress = 2
	};

	enum service_commands{
		get_all_settings	= 0,
		set_device_ip_mode	= 1,
		set_device_ip		= 2,
		set_dns				= 3,
		set_subnet			= 4,
		set_gw				= 5,
		set_service_ip_mode = 6,
		set_service_ip		= 7,
		set_servcie_port	= 8,
	
		set_motor_current	= 9,
		set_motor_steps_divider = 10,
		set_motor_acceleration	= 11,
		set_motor_acceleration_eeprom = 12,
		set_motor_speed			= 13,
		set_motor_speed_eeprom	= 14,
		set_motor_bottom_margin = 15,
		set_motor_poll_timer	= 16,
		set_motor_poll_timer_eeprom = 17,
	
		set_temperature_treshold	= 18,
		reset_to_factory			= 254,
		reboot						= 255
	};

	typedef struct{
		//Ethernet section
		uint8_t		ethernet_ip_mode;
		uint8_t		ethernet_device_ip[4];
		uint8_t		ethernet_dns[4];
		uint8_t		ethernet_subnet[4];
		uint8_t		ethernet_gw[4];
		uint8_t		ethernet_service_ip_mode;
		uint8_t		ethernet_service_ip[4];
		uint16_t	ethernet_servicePort;
	
		//Motor section
		uint16_t	motor_current;
		uint8_t		motor_steps_divider;
		uint16_t	motor_acceleration;
		uint16_t	motor_speed;
		uint16_t	motor_bottom_margin;
		uint16_t	motor_poll_timer;
	
		//Temperature section
		uint8_t		temperature_treshold;
	}settings;

	//extern settings NympheaSettingsAddresses;
	extern settings NympheaSettings;

	void ReadEEPROMSettings();
	uint8_t HandleServiceCommand(void* pCommand, uint8_t commandLength, uint8_t** pRes);
	void FactoryReset();

#endif /* EEPROM_H_ */