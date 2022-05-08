namespace Common.Helpers; 

public static class ActivationFunctions {
	// relu activation function
	public static float Relu(float x) {
		return x > 0 ? x : 0;
	}
	
	// tanh activation function
	public static float Tanh(float x) {
		return (float)Math.Tanh(x);
	}
	
	// sigmoid activation function
	public static float Sigmoid(float x) {
		return (float)(1 / (1 + Math.Exp(-x)));
	}

	// relu derivative
	public static float ReluDerivative(float x) {
		return x > 0 ? 1 : 0;
	}
	
	// tanh derivative
	public static float TanhDerivative(float x) {
		return 1 - (float)Math.Pow(x, 2);
	}
	
	// sigmoid derivative
	public static float SigmoidDerivative(float x) {
		return (float)(Sigmoid(x) * (1 - Sigmoid(x)));
	}
	
	
}
