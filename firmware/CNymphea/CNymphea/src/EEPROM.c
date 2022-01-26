#include <avr/io.h>
#include <avr/eeprom.h>
#include <avr/wdt.h>
#include <avr/interrupt.h>

#include "EEPROM.h"
#include "motor.h"

settings EEMEM NympheaSettingsAddresses = {
	.ethernet_ip_mode			= Factory,
	.ethernet_device_ip			= {192, 168, 2, 2},
	.ethernet_dns				= {192, 168, 2, 1},
	.ethernet_subnet			= {255,255,255,0},
	.ethernet_gw				= {192, 168, 2, 1},
	.ethernet_service_ip_mode	= TalkBack,
	.ethernet_service_ip		= {192, 168, 2, 1},
	.ethernet_servicePort		= 8889,
		
	//Motor section
	.motor_current				= 800,
	.motor_steps_divider		= 32,
	.motor_acceleration			= 3200,
	.motor_speed				= 3200,
	.motor_bottom_margin		= 400,
	.motor_poll_timer			= 1500,
			
	//Temperature section
	.temperature_treshold		= 50,
};
settings NympheaSettings;

void ReadEEPROMSettings(){
	eeprom_read_block(&NympheaSettings, &NympheaSettingsAddresses, sizeof(settings));
}

uint8_t SetByte(uint8_t* pCommand, uint8_t commandLength, uint8_t* pRes, uint8_t* pEEPROMaddr){
	eeprom_update_byte(pEEPROMaddr, *(pCommand+1));
	*pRes = eeprom_read_byte(pEEPROMaddr);
	if (*pRes == *(pCommand+1)){
		return 1;
	}
	else{
		return 0;
	}
	return 0;
}
uint8_t SetWord(uint16_t* pCommand, uint8_t commandLength, void* pRes, uint16_t* pEEPROMaddr){
	eeprom_update_word(pEEPROMaddr, *(pCommand));
	*((uint16_t *)pRes)=eeprom_read_word(pEEPROMaddr);
	if (*((uint16_t *)pRes) == *(pCommand)){
		return 2;
	}
	else{
		return 0;
	}
	return 0;
}
uint8_t SetIP(uint8_t* pCommand, uint8_t commandLength, uint8_t* pRes, uint8_t* pEEPROMaddr){
	eeprom_update_block(pCommand+1, pEEPROMaddr, 4);
	eeprom_read_block(pRes,pEEPROMaddr, 4);
	for (uint8_t i=0; i<4; i++){
		if (*(pCommand+1+i)!=*(pRes+i)){
			return 0;
		}
		else{
			return 4;
		}
	}
	return 0;
}
uint8_t HandleServiceCommand(void* pCommand, uint8_t commandLength, uint8_t** pRes){
	switch(*((uint8_t *)pCommand))
	{

		case set_device_ip_mode:
		{
			return SetByte((uint8_t *)pCommand, commandLength, *pRes, &NympheaSettingsAddresses.ethernet_ip_mode);
			break;
		}
		case set_device_ip:
		{
			return SetIP((uint8_t *)pCommand, commandLength, *pRes, (uint8_t*)&NympheaSettingsAddresses.ethernet_device_ip);
			break;
		}
		case set_dns:
		{
			return SetIP((uint8_t *)pCommand, commandLength, *pRes, (uint8_t *)&NympheaSettingsAddresses.ethernet_dns);
			break;
		}
		case set_subnet:
		{
			return SetIP((uint8_t *)pCommand, commandLength, *pRes, (uint8_t *)&NympheaSettingsAddresses.ethernet_subnet);
			break;
		}
		case set_gw:
		{
			return SetIP((uint8_t *)pCommand, commandLength, *pRes, (uint8_t *)&NympheaSettingsAddresses.ethernet_gw);
			break;
		}
		case set_service_ip_mode:
		{
			return SetByte((uint8_t *)pCommand, commandLength, *pRes, &NympheaSettingsAddresses.ethernet_service_ip_mode);
			break;
		}
		case set_service_ip:
		{
			return SetIP((uint8_t *)pCommand, commandLength, *pRes, (uint8_t *)&NympheaSettingsAddresses.ethernet_service_ip);
			break;
		}
		case set_servcie_port:
		{
			return SetWord((uint16_t *)(pCommand+1), commandLength, *pRes, &NympheaSettingsAddresses.ethernet_servicePort);
			break;
		}
		case set_motor_current:
		{
			return SetWord((uint16_t *)(pCommand+1), commandLength, *pRes, &NympheaSettingsAddresses.motor_current);
			break;
		}
		case set_motor_steps_divider:
		{
			return SetByte((uint8_t *)pCommand, commandLength, *pRes, &NympheaSettingsAddresses.motor_steps_divider);
			break;
		}
		case set_motor_acceleration:
		{
			uint16_t acceleration = *((uint16_t *)(pCommand+1));
			//stepper.setAcceleration(acceleration);
			void * vp = (void *)*pRes;
			uint16_t * p = (uint16_t *)vp;
			*p = acceleration;
			
			return 2;
			break;
		}
		case set_motor_acceleration_eeprom:
		{
			return SetWord((uint16_t *)(pCommand+1), commandLength, *pRes, &NympheaSettingsAddresses.motor_acceleration);
			break;
		}
		case set_motor_speed:
		{
			uint16_t speed = *((uint16_t *)(pCommand+1));
			//stepper.setMaxSpeed(speed);
			void * vp = (void *)*pRes;
			uint16_t * p = (uint16_t *)vp;
			*p = speed;
			return 2;
			break;
		}
		case set_motor_speed_eeprom:
		{
			return SetWord((uint16_t *)(pCommand+1), commandLength, *pRes, &NympheaSettingsAddresses.motor_speed);
			break;
		}
		case set_motor_bottom_margin:{
			return SetWord((uint16_t *)(pCommand+1), commandLength, *pRes, &NympheaSettingsAddresses.motor_bottom_margin);
			break;
		}
		case set_motor_poll_timer:{
			uint16_t timer = *((uint16_t *)(pCommand+1));
			OCR1A = timer;
			void * vp = (void *)*pRes;
			uint16_t * p = (uint16_t *)vp;
			*p = timer;
			return 2;
			break;
		}
		case set_motor_poll_timer_eeprom:{
			return SetWord((uint16_t *)(pCommand+1), commandLength, *pRes, &NympheaSettingsAddresses.motor_poll_timer);
			break;
		}
		case set_temperature_treshold:
		{
			return SetByte((uint8_t *)pCommand, commandLength, *pRes, &NympheaSettingsAddresses.temperature_treshold);
			break;
		}
		case get_all_settings:{
			*pRes = (uint8_t *)&NympheaSettings;
			return sizeof(NympheaSettings);
		}
		case reboot:
		{
			cli();
			wdt_enable(WDTO_120MS);
			while(1); // wait to die and be reborn....
			break;
		}
	}
	return 0;
}
void FactoryReset(){
	settings s;
	eeprom_update_block(&s, &NympheaSettingsAddresses, sizeof(settings));
}