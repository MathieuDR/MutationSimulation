namespace Common.Helpers; 

public static class Oscillator {
	public static float Oscillate(float phase, float frequency = 5f, float amplitude = 1f) => amplitude * (float)Math.Sin(frequency * phase);
	public static float OscillateAnalog(float phase, float frequency = 5f) => (Oscillate(phase, frequency, 0.5f) + 0.5f) ; 
}
