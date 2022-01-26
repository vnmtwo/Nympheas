#include "Pins.h"
#include "Stepper.h"
#include <util/delay.h>
#include <stdlib.h>

volatile long position = 0;
volatile long goto_position = 0;
uint8_t stop = 0;

void Stepper_stop(){
	stop = 1;
}
void Stepper_goto_position(long _goto_position)
{
	goto_position = _goto_position;
}
void Stepper_set_current_position(long _position)
{
	position = _position;
}
long Stepper_distance_to_go()
{
	return labs(goto_position-position);
}

void step(){
	DRIVER_STEP_PORT |= (1<<DRIVER_STEP_PIN);
	_delay_us(1);
	DRIVER_STEP_PORT &= ~(1<<DRIVER_STEP_PIN);
}

void Stepper_run()
{
	if (stop)
		return;
		
	if (position<goto_position)
	{
		DRIVER_DIR_PORT |= (1<<DRIVER_DIR_PIN); //1 - up
		step();
		position++;
	}
	else if(position>goto_position){
		DRIVER_DIR_PORT &= ~(1<<DRIVER_DIR_PIN);
		step();
		position--;
	}
}

