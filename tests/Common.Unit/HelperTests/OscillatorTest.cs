using Common.Helpers;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Common.Unit.HelperTests; 

public class OscillatorTest {
	private readonly ITestOutputHelper _testOutputHelper;

	public OscillatorTest(ITestOutputHelper testOutputHelper) {
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public void Oscillate_ShouldBeBetweenAmpl_WhenGivenValues() {
		//Arrange
		var phaseOffsett = 0f;
		var frequency = 1f;
		var ampl = 1f;
		
		//Act
		for (int i = 0; i < 1000; i++) {
			var currentPhase = phaseOffsett + (float)i / 1000;
			var result = Oscillator.Oscillate(currentPhase, frequency, ampl);

			// assert
			result.Should().BeLessOrEqualTo(ampl);
			result.Should().BeGreaterOrEqualTo(-ampl);
		}
	}
	
	[Fact]
	public void OscillateAnalog_ShouldBeBetween0And1_WhenGiven1Ampl() {
		//Arrange
		var phaseOffsett = 0f;
		var frequency = 50f;
		
		//Act
		for (int i = 0; i < 1000; i++) {
			var currentPhase = phaseOffsett + (float)i / 1000;
			var result = Oscillator.OscillateAnalog(currentPhase, frequency);
			
			// assert
			result.Should().BeLessOrEqualTo(1);
			result.Should().BeGreaterOrEqualTo(0);
		}
	}
}
