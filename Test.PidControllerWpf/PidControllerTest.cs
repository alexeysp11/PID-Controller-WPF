using NUnit.Framework;
using PidControllerWpf.Model; 

namespace Test.PidControllerWpf.Model
{
    public class PidControllerTest
    {
        private PidController _PidController = null; 

        private float MinValue { get; set; } = 0; 
        private float MaxValue { get; set; } = 0; 
        
        private float ProcessVariable { get; set; } = 0; 
        private float Setpoint { get; set; } = 32.0f; 

        private System.TimeSpan DeltaTime { get; set; }  

        [SetUp]
        public void Setup()
        {
            MinValue = -10; 
            MinValue = 50; 
            _PidController = new PidController(MinValue, MaxValue); 
        }

        [Test]
        public void ControlPv_ZeroDeltaTime_SetpointDidNotChanged()
        {
            // Arrange 
            DeltaTime = System.TimeSpan.Zero; 

            // Act 
            float processVariableExpected = ProcessVariable;
            ProcessVariable = _PidController.ControlPv(ProcessVariable, Setpoint, DeltaTime); 

            // Assert 
            Assert.AreEqual(processVariableExpected, ProcessVariable, 0.0f); 
        }
    }
}