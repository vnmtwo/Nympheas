#include <util/delay.h>

#include "light_ws2812.h"
#include "Light.h"
#include "Pins.h"

ws2812_stripe pistil_stripe, lb_stripe;

uint8_t black[3] = {0,0,0};
uint8_t red[3] = {255,0,0};
uint8_t green[3] = {0,0,255};
uint8_t yellow[3] ={255,0,69};

void Blink(ws2812_stripe *stripe, uint8_t code, uint8_t* c){
	ws2812_show_single_color(stripe, c);
	for (uint8_t i=0; i<code; i++){
		ws2812_show_single_color(stripe, c);
		_delay_ms(500);
		ws2812_show_single_color(stripe, &black[0]);
		_delay_ms(500);
	}
	_delay_ms(3000);
}

void Light_BlinkSuccess(uint8_t code){
	Blink(&pistil_stripe, code, &green[0]);
}
	
void Light_InitLight(){
	pistil_stripe = ws2812_init_stripe(PISTIL_LED_COUNT, &PISTIL_PORT, &PISTIL_REGISTER, PISTIL_PIN);	
	lb_stripe = ws2812_init_stripe(LIGHTBOX_LED_COUNT, &LIGHTBOX_PORT, &LIGHTBOX_REGISTER, LIGHTBOX_PIN);

	ws2812_show_single_color(&pistil_stripe, &black[0]);
	ws2812_show_single_color(&lb_stripe, &black[0]);
}

void Light_SetLights(uint8_t *data){
	ws2812_show_single_color(&pistil_stripe, data);
	ws2812_show_single_color(&lb_stripe, data+3);
}

void Light_BlinkMotorInitError(uint8_t code){
	ws2812_show_single_color(&lb_stripe, &yellow[0]);
	Blink(&pistil_stripe, code, &red[0]);
}