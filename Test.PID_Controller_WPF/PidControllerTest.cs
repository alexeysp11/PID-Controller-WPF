using NUnit.Framework;
using PID_Controller_WPF.Model; 

namespace Test.PID_Controller_WPF.Model
{
    public class PidControllerTest
    {
        #region Members
        /// <summary>
        /// Instance of PidController class 
        /// </summary>
        private PidController _PidController = null; 
        #endregion  // Members

        #region Properties
        /// <summary>
        /// Minimal value of process variable and setpoint
        /// </summary>
        /// <value>Private get and set</value>
        private float MinValue { get; set; } = 0;  
        /// <summary>
        /// Maximal value of process variable and setpoint
        /// </summary>
        /// <value>Private get and set</value>
        private float MaxValue { get; set; } = 0; 
        /// <summary>
        /// Value of PV at current time
        /// </summary>
        /// <value>Private get and set</value>
        private float ProcessVariable { get; set; } = 0; 
        /// <summary>
        /// Desired setpoint
        /// </summary>
        /// <value>Private get and set</value>
        private float Setpoint { get; set; } = 32.0f; 
        /// <summary>
        /// Delta time
        /// </summary>
        /// <value>Private get and set</value>
        private System.TimeSpan DeltaTime { get; set; }  
        #endregion  // Properties

        [SetUp]
        public void Setup()
        {
            // Initialize PID controller 
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