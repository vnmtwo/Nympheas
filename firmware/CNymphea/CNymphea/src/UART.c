#include <avr/io.h>
#include <stdio.h>

#define BAUD 9600
#include <util/setbaud.h>

void uart_init() {
	UBRR1L = UBRRH_VALUE;
	UBRR1L = UBRRL_VALUE;

	#if USE_2X
	UCSR1A |= _BV(U2X1);
	#else
	UCSR1A &= ~(_BV(U2X1));
	#endif

	UCSR1C = _BV(UCSZ11) | _BV(UCSZ10); /* 8-bit data */
	UCSR1B = _BV(RXEN1) | _BV(TXEN1);   /* Enable RX and TX */
}

void uart_write(uint8_t data) {
	while(!(UCSR1A & (1<<UDRE1)));
	UDR1 = data;
}