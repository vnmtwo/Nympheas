#include "Ethernet.h"
#include "spi.h"
#include "wizchip_conf.h"
#include <util/delay.h>
#include "socket.h"
#include "ArtNet.h"

uint8_t ArtNetBuffer[ARTNET_PACKET_SIZE];

void cs_sel() {
	SPI_WIZNET_ENABLE();
}

void cs_desel() {
	SPI_WIZNET_DISABLE();
}

uint8_t spi_rb(void) {
	uint8_t rbuf;
	//HAL_SPI_Receive(&hspi1, &rbuf, 1, HAL_MAX_DELAY);
	SPI_READ(rbuf);
	return rbuf;
}

void spi_wb(uint8_t b) {
	//HAL_SPI_Transmit(&hspi1, &b, 1, HAL_MAX_DELAY);
	SPI_WRITE(b);
}

void spi_rb_burst(uint8_t *buf, uint16_t len) {
	//HAL_SPI_Receive_DMA(&hspi1, buf, len);
	//while(HAL_SPI_GetState(&hspi1) == HAL_SPI_STATE_BUSY_RX);
	for (uint16_t var = 0; var < len; var++) {
		SPI_READ(*buf++);
	}
}

void spi_wb_burst(uint8_t *buf, uint16_t len) {
	//HAL_SPI_Transmit_DMA(&hspi1, buf, len);
	//while(HAL_SPI_GetState(&hspi1) == HAL_SPI_STATE_BUSY_TX);
	for (uint16_t var = 0; var < len; var++) {
		SPI_WRITE(*buf++);
	}
}

wiz_NetInfo netInfo = { .mac  = {0x00, 0x08, 0xdc, 0xab, 0xcd, 0xef}, // Mac address
						.ip   = {192, 168, 2, 100},         // IP address
						.sn   = {255, 255, 255, 0},         // Subnet mask
						.dns =  {8,8,8,8},			  // DNS address (google dns)
						.gw   = {192, 168, 2, 1}, // Gateway address
						.dhcp = NETINFO_STATIC};    //Static IP configuration

void ResetEthernet(){
	ETHERNET_RESET_REGISTER |= (1<<ETHERNET_RESET_PIN);
	ETHERNET_RESET_PORT &= ~(1<<ETHERNET_RESET_PIN);
	_delay_ms(5);
	ETHERNET_RESET_PORT |= (1<<ETHERNET_RESET_PIN);
	_delay_ms(10);
		
}

void IO_LIBRARY_Init(void) {
	uint8_t bufSize[] = {2, 2, 2, 2, 2, 2, 2, 2};

	reg_wizchip_cs_cbfunc(cs_sel, cs_desel);
	reg_wizchip_spi_cbfunc(spi_rb, spi_wb);
	reg_wizchip_spiburst_cbfunc(spi_rb_burst, spi_wb_burst);

	wizchip_init(bufSize, bufSize);
	wizchip_setnetinfo(&netInfo);
	//wizchip_setinterruptmask(IK_SOCK_0);
}

void Ethernet_InitEthernet(){
	ResetEthernet();
	SPI_Init();
	IO_LIBRARY_Init();
}

void Ethernet_CheckArtnetUDP(){
	uint16_t size;
	uint8_t  remoteIP[4];
	uint16_t remotePort;
	
	switch(getSn_SR(ARTNET_UDPS))
	{
		case SOCK_UDP :
			if((size = getSn_RX_RSR(ARTNET_UDPS)) > 0)
			{
				if(size > ARTNET_PACKET_SIZE) size = ARTNET_PACKET_SIZE;
				recvfrom(ARTNET_UDPS, ArtNetBuffer, size, remoteIP, &remotePort);
				ArtNet_Parse(ArtNetBuffer);
			}
			break;
			
		case SOCK_CLOSED:
			socket(ARTNET_UDPS, Sn_MR_UDP, ARTNET_PORT, 0x00);
			break;
			
		default :
		break;
	}
}