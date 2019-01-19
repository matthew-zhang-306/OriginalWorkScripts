
public class InputHolder {

	public bool Accelerate;
	public bool Brake;
	public float Turn;

	public InputHolder() : this(false, false, 0f) {
	}

	public InputHolder(bool accel, bool brake, float turn) {
		Accelerate = accel;
		Brake = brake;
		Turn = turn;
	}

}
