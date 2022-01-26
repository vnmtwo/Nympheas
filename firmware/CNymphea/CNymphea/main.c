#include <avr/io.h>
#include "Light.h"
#include "Ethernet.h"
#include "Motor.h"
#include "EEPROM.h"

FUSES ={.low = 0xff, .high = 0xd9, .extended = 0xfb,};

int main(void)
{
	ReadEEPROMSettings();
	Ethernet_InitEthernet();
	Light_InitLight();	
	Motor_init();
	
    while (1) 
    {
		Ethernet_CheckArtnetUDP();
    }
}

