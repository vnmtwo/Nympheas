#ifndef SPI_H_
#define SPI_H_
	#include "Pins.h"
	
	#define SPI_TXBUF SPDR
	#define SPI_RXBUF SPDR
	
	#define BV(bitno) _BV(bitno)
	
	#define SPI_WAITFOREOTx() do { while (!(SPSR & BV(SPIF))); } while (0)
	#define SPI_WAITFOREORx() do { while (!(SPSR & BV(SPIF))); } while (0)

	#define SPI_WIZNET_ENABLE()  ( ETHERNET_CS_PORT &= ~BV(ETHERNET_CS_PIN) )
	#define SPI_WIZNET_DISABLE() ( ETHERNET_CS_PORT |=  BV(ETHERNET_CS_PIN) )

	#define SPI_8_MHZ
	
	#ifdef SPI_WAITFORTxREADY
	#define SPI_WAITFORTx_BEFORE() SPI_WAITFORTxREADY()
	#define SPI_WAITFORTx_AFTER()
	#define SPI_WAITFORTx_ENDED() SPI_WAITFOREOTx()
	#else /* SPI_WAITFORTxREADY */
	#define SPI_WAITFORTx_BEFORE()
	#define SPI_WAITFORTx_AFTER() SPI_WAITFOREOTx()
	#define SPI_WAITFORTx_ENDED()
	#endif /* SPI_WAITFORTxREADY */
	
	void SPI_Init();
	
	#define SPI_READ(data)  \
	do {					\
		SPI_TXBUF = 0;      \
		SPI_WAITFOREORx();  \
		data = SPI_RXBUF;   \
	} while(0)
	
	/* Write one character to SPI */
	#define SPI_WRITE(data)                       \
	do {                                          \
		SPI_WAITFORTx_BEFORE();                   \
		SPI_TXBUF = data;                         \
		SPI_WAITFOREOTx();                        \
	} while(0)
	
#endif /* SPI_H_ */