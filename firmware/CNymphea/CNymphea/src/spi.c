#include <avr/io.h>
#include "spi.h"
#include "Pins.h"

//https://github.com/maxxir/m1284p_wiz5500/blob/master/03_m1284p_WIZNET_loopback_STATIC_IP/spi.c

void SPI_Init()
{
	// CS PIN for FLASH
	ETHERNET_CS_REGISTER |= (1<<(ETHERNET_CS_PIN)); // CS to OUT && Disable
	SPI_WIZNET_DISABLE();
  
	/* Initalize ports for communication with SPI units. */
	/* CSN=SS and must be output when master! */
	DDRB  |= _BV(MOSI) | _BV(SCK) | _BV(CSN);
	PORTB |= _BV(MOSI) | _BV(SCK);
 
	/* Enables SPI, selects "master", clock rate FCK / 4 - 4Mhz, and SPI mode 0 */
	SPCR = _BV(SPE) | _BV(MSTR);
	#if defined(SPI_8_MHZ)
		SPSR = _BV(SPI2X); //FCK / 2 - 8Mhz
	#elif defined (SPI_4_MHZ)
		SPSR = 0x0; //FCK / 4 - 4Mhz
	#else
		SPSR = 0x0; //FCK / 4 - 4Mhz
	#endif


}