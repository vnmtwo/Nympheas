#include "Pins.h"
#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include "Stepper.h"
#include "EEPROM.h"
#include "Motor.h"
#include "Light.h"
#include "UART.h"
#include "tmc2209.h"

void InitMotorTimer(){
	// инициализация Timer1
	cli(); // отключить глобальные прерывания
	TCCR1A = 0; // установить регистры в 0
	TCCR1B = 0;

	OCR1A = 1500; // установка регистра совпадения
	TCCR1B |= (1 << WGM12); // включение в CTC режим

	// Установка битов CS10 и CS12 на коэффициент деления 1
	TCCR1B |= (1 << CS10);
	//TCCR1B |= (1 << CS12);

	TIMSK1 |= (1 << OCIE1A);  // включение прерываний по совпадению
	sei(); // включить глобальные прерывания
}

ISR(TIMER1_COMPA_vect)
{
	Stepper_run();
}

	  union {
		  uint32_t sr;
		  struct {
			  uint8_t toff : 4,
			  hstrt : 3,
			  hend : 4,
			  : 4,
			  tbl : 2;
			  uint8_t    vsense : 1;
			  uint8_t : 6,
			  mres : 4;
			  uint8_t    intpol : 1,
			  dedge : 1,
			  diss2g : 1,
			  diss2vs : 1;
		  };
	  }chopconf;
	  
void InitDriver()
{
	uart_init();
	_delay_ms(100);
	tmc2209_write(0x80, 0x80);
	chopconf.sr = 0x10000053; 
	chopconf.mres=4;
	_delay_ms(1);
	tmc2209_write(0x6c+0x80, chopconf.sr);
}

uint8_t FindZero()
{
	uint8_t w = WORKZONE_OPTICAL_PINREG & (1<<WORKZONE_OPTICAL_PIN);
	uint8_t b = BOTTOM_OPTICAL_PINREG & (1<<BOTTOM_OPTICAL_PIN);
	
	//самое нижнее положение
	if (!w && b)
	{
		Stepper_goto_position(4l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));	
		while(!(WORKZONE_OPTICAL_PINREG & (1<<WORKZONE_OPTICAL_PIN))){
			if (Stepper_distance_to_go()==0)
			return 1;
		}
		Stepper_set_current_position(-(long)NympheaSettings.motor_bottom_margin);
		Stepper_goto_position(0);
		while (Stepper_distance_to_go()>0){
			_delay_ms(1);
		}	
	}

	//между триггерами, или выше верхнего положения
	if (!w && !b){
		Stepper_goto_position(-4l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));
		while(!(BOTTOM_OPTICAL_PINREG & (1<<BOTTOM_OPTICAL_PIN))){
			if (Stepper_distance_to_go()==0)
				break;
		}
		if (Stepper_distance_to_go()==0){
			if (WORKZONE_OPTICAL_PINREG & (1<<WORKZONE_OPTICAL_PIN)){
				Stepper_goto_position(-10l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));
				while (WORKZONE_OPTICAL_PINREG & (1<<WORKZONE_OPTICAL_PIN)){
					if (Stepper_distance_to_go()==0)
						return 4;
				}
				Stepper_set_current_position(-(long)NympheaSettings.motor_bottom_margin);
				Stepper_goto_position(-2l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));
				
				while(!(BOTTOM_OPTICAL_PINREG & (1<<BOTTOM_OPTICAL_PIN))){
					if (Stepper_distance_to_go()==0)
						return 5;
				}
				Stepper_goto_position(0);
				while(Stepper_distance_to_go()>0){
					_delay_ms(1);
				}
			}
			else{
				return 6;
			}
		}
		else{
			Stepper_goto_position(4l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));
			while(!(WORKZONE_OPTICAL_PINREG & (1<<WORKZONE_OPTICAL_PIN))){
				if (Stepper_distance_to_go()==0)
					return 7;
			}
			Stepper_set_current_position(-(long)NympheaSettings.motor_bottom_margin);
			Stepper_goto_position(0);
			while( Stepper_distance_to_go()>0){
				_delay_ms(1);
			}
		}
	}
	
	//в рабочей зоне
	if (w && !b){
		Stepper_goto_position(-10l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));
		while ( WORKZONE_OPTICAL_PINREG & (1<<WORKZONE_OPTICAL_PIN))
		{
			if (Stepper_distance_to_go()==0)
				return 8;
		}
		Stepper_set_current_position(-(long)NympheaSettings.motor_bottom_margin);
		Stepper_goto_position(-(long)NympheaSettings.motor_bottom_margin + -2l*MOTOR_STEPS_PER_REVOLUTION(NympheaSettings.motor_steps_divider));
			//
		while(!(BOTTOM_OPTICAL_PINREG & (1<<BOTTOM_OPTICAL_PIN))){
			if (Stepper_distance_to_go()==0)
				return 9;
		}
		Stepper_goto_position(0);
		while (Stepper_distance_to_go()>0){
			_delay_ms(1);
		}
	}

	return 0;	
}

void Motor_init(){
	InitDriver();
	
	DRIVER_DIR_REGISTER |= (1<<DRIVER_DIR_PIN);
	DRIVER_EN_REGISTER |=(1<<DRIVER_EN_PIN);
	DRIVER_STEP_REGISTER |=(1<<DRIVER_STEP_PIN);
	
	DRIVER_EN_PORT &= ~(1<<DRIVER_EN_PIN);
	
	WORKZONE_OPTICAL_REGISTER &= ~(1<<WORKZONE_OPTICAL_PIN);
	BOTTOM_OPTICAL_REGISTER &= ~(1<<BOTTOM_OPTICAL_PIN);
	
	InitMotorTimer();
	
	uint8_t error = FindZero();

	if(error>0){
		Stepper_stop();
		while(1){
			Light_BlinkMotorInitError(error);
		}
	}
	Light_BlinkSuccess(3);
}

void Motor_SetPosition(uint8_t * pPosition){
	Stepper_goto_position((*pPosition)*(MOTOR_WORKZONE_STEPS_COUNT(NympheaSettings.motor_steps_divider)-NympheaSettings.motor_bottom_margin)/255l);
}