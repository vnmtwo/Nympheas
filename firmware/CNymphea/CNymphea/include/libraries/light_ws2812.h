#ifndef LIGHT_WS2812_H_
	#define LIGHT_WS2812_H_
	#include <avr/io.h>

	typedef struct {
		uint8_t count_led;
		uint8_t pin_mask;
		volatile uint8_t * port;
		volatile uint8_t * port_register;
	}ws2812_stripe;
	
	ws2812_stripe ws2812_init_stripe(uint8_t led_count, volatile uint8_t* port, volatile uint8_t* reg, uint8_t pin);
	void ws2812_show_single_color(ws2812_stripe *pStripe,  uint8_t* pColor_data_3);
#endif /* LIGHT_WS2812_H_ */