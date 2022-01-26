#ifndef ETHERNET_H_
#define ETHERNET_H_
	#include "avr/io.h"

	#define ARTNET_UDPS 0
	

	void Ethernet_CheckArtnetUDP();
	void Ethernet_InitEthernet();

#endif /* ETHERNET_H_ */