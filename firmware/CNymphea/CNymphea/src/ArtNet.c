#include "ArtNet.h"
#include "Light.h"
#include "Motor.h"
#include "DeviceId.h"

//uint16_t opcode(uint8_t* pBuffer)
//{
	//return (pBuffer[9] << 8 | pBuffer[8]);
//}

void ArtNet_Parse(uint8_t *pArtNetBuffer)
{
	if ((pArtNetBuffer[9] << 8 | pArtNetBuffer[8] )!= 0x5000) //check op code
		return;
		
	uint8_t * p = &pArtNetBuffer[ARTNET_HEADER_SIZE];
	Light_SetLights(p+((uint16_t)7*deviceId)+1);
	Motor_SetPosition(p+((uint16_t)7*deviceId));
}