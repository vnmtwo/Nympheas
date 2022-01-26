#ifndef ARTNET_H_
#define ARTNET_H_

	#include "avr/io.h"
	#define ARTNET_PORT 6454
	#define ARTNET_HEADER_SIZE 18
	#define ARTNET_DEVICES_COUNT 37
	#define ARTNET_CHANNELS_PER_DEVICE 7
	#define ARTNET_PACKET_SIZE (ARTNET_HEADER_SIZE+ARTNET_DEVICES_COUNT*ARTNET_CHANNELS_PER_DEVICE)

	void ArtNet_Parse(uint8_t * pArtNetBuffer);

#endif /* ARTNET_H_ */