using NUnit.Framework;
using PidControllerWpf.Model; 

namespace Test.PidControllerWpf.Model
{
    public class PidControllerTest
    {
        private PidController PidController = null; 

        private float MinValue { get; set; } = 0; 
        private float MaxValue { get; set; } = 0; 

        private System.TimeSpan DeltaTime { get; set; }  

        [SetUp]
        public void Setup()
        {
            MinValue = -10; 
            MinValue = 50; 
            PidController = new PidController(MinValue, MaxValue); 
        }

        [Test]
        public void ControlPv_ZeroDeltaTime_SetpointDidNotChanged()
        {
            // Arrange 
            DeltaTime = System.TimeSpan.Zero; 
            float pvExpected = 0;
            float pv = 0; 
            float setpoint = 32.0f; 

            // Act 
            PidController.ControlPv(ref pv, setpoint, DeltaTime); 

            // Assert 
            Assert.AreEqual(pvExpected, pv, 0.0f); 
        }
    }
}