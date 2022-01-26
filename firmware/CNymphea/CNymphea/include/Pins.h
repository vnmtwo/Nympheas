#ifndef PINS_H_
#define PINS_H_
	#include <Avr/io.h>
	//DEVICE ID
	#define ID_BIT_0 6
	#define ID_BIT_1 8
	#define ID_BIT_2 9
	#define ID_BIT_3 10
	#define ID_BIT_4 5
	#define ID_BIT_5 13
	
	//LIGHT
	#define PISTIL_PIN PORTD6
	#define PISTIL_PORT PORTD
	#define PISTIL_REGISTER DDRD
	
	#define LIGHTBOX_PIN PORTD4
	#define LIGHTBOX_PORT PORTD
	#define LIGHTBOX_REGISTER DDRD
	
	//ETHERNET
	#define ETHERNET_RESET_PIN 4
	#define ETHERNET_RESET_PORT PORTF
	#define ETHERNET_RESET_REGISTER DDRF
	
	#define ETHERNET_CS_PIN 5
	#define ETHERNET_CS_PORT PORTF
	#define ETHERNET_CS_REGISTER DDRF
	
	//SPI
	#define SCK            1  /* - Output: SPI Serial Clock (SCLK) - ATMEGA644/1284 PORTB, PIN7 */
	#define MOSI           2  /* - Output: SPI Master out - slave in (MOSI) -  ATMEGA644/1284 PORTB, PIN5 */
	#define MISO           3  /* - Input:  SPI Master in - slave out (MISO) -  ATMEGA644/1284 PORTB, PIN6 */
	#define CSN            0  /*SPI - SS*/
	

	//MOTORS
	#define MOTOR_TEMPERATURE_PIN A4

	#define BOTTOM_OPTICAL_PIN 0
	#define BOTTOM_OPTICAL_PORT PORTF
	#define BOTTOM_OPTICAL_REGISTER DDRF
	#define BOTTOM_OPTICAL_PINREG PINF
	
	#define WORKZONE_OPTICAL_PIN 7
	#define WORKZONE_OPTICAL_PORT PORTF
	#define WORKZONE_OPTICAL_REGISTER DDRF
	#define WORKZONE_OPTICAL_PINREG PINF
	
	#define DRIVER_EN_PIN 1
	#define DRIVER_EN_PORT PORTD
	#define DRIVER_EN_REGISTER DDRD
	
	#define DRIVER_STEP_PIN 7
	#define DRIVER_STEP_PORT PORTB
	#define DRIVER_STEP_REGISTER DDRB
	
	#define DRIVER_DIR_PIN 0
	#define DRIVER_DIR_PORT PORTD
	#define DRIVER_DIR_REGISTER DDRD
	
	//OTHERS
	#define RESET_BUTTON_PIN A1
#endif /* PINS_H_ */