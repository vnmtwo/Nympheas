#ifndef LIGHT_H_
#define LIGHT_H_

	#define PISTIL_LED_COUNT 24
	#define LIGHTBOX_LED_COUNT 125
	
	void Light_InitLight();
	void Light_SetLights(uint8_t *data);
	void Light_BlinkSuccess(uint8_t code);
	void Light_BlinkMotorInitError(uint8_t code);
	
#endif /* LIGHT_H_ */