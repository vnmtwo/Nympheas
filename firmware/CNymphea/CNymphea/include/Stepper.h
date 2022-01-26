#ifndef STEPPER_H_
#define STEPPER_H_
	void Stepper_run();
	void Stepper_stop();
	void Stepper_goto_position(long goto_position);
	void Stepper_set_current_position(long _position);
	long Stepper_distance_to_go();
#endif /* STEPPER_H_ */