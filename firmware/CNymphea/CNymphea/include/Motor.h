#ifndef MOTOR_H_
#define MOTOR_H_

	#define MOTOR_SELF_STEPS 200
	#define MOTOR_STEPS_PER_REVOLUTION(steps_divider) ((long)MOTOR_SELF_STEPS * steps_divider)
	#define MOTOR_WORKZONE_REVOLUTIONS_COUNT 6
	#define MOTOR_WORKZONE_STEPS_COUNT(steps_divider) ((long)MOTOR_WORKZONE_REVOLUTIONS_COUNT * MOTOR_STEPS_PER_REVOLUTION(steps_divider))

	void Motor_init();
	void Motor_SetPosition(uint8_t * pPosition);

#endif /* MOTOR_H_ */