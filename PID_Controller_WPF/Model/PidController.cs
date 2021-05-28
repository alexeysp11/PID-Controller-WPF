namespace PID_Controller_WPF.Model
{
    class PidController
    {
        #region PID controller parameters
        /// <summary>
        /// Proportional gain of PID controller 
        /// </summary>
        public float ProportionalGain { get; private set; } = 0; 
        /// <summary>
        /// Integral gain of PID controller 
        /// </summary>
        public float IntegralGain { get; private set; } = 0; 
        /// <summary>
        /// Derivative gain of PID controller 
        /// </summary>
        public float DerivativeGain { get; private set; } = 0; 
        /// <summary>
        /// Integral term of PID controller 
        /// </summary>
        public float IntegralTerm { get; private set; } = 0; 
        #endregion  // PID controller parameters

        #region PV properties
        /// <summary>
        /// Max value of process variables 
        /// </summary>
        private float MaxValue { get; set; } = 0; 
        /// <summary>
        /// Min value of process variables 
        /// </summary>
        private float MinValue { get; set; } = 0; 
        #endregion  // PV properties

        #region Constructor
        /// <summary>
        /// Constructor of a class `PidController`
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public PidController(float minValue, float maxValue)
        {
            // Assign min and max values that PV can be equal
            this.MaxValue = maxValue; 
            this.MinValue = minValue; 

            // Set PID parameters
            this.ProportionalGain = -0.8f; 
            this.IntegralGain = 1.0f; 
            this.DerivativeGain = 0.1f; 
        }
        #endregion  // Constructor

        #region Methods
        /// <summary>
        /// Allows to control process variable 
        /// </summary>
        /// <param name="dt"></param>
        public float ControlPv(float processVariable, float setpoint, System.TimeSpan dt)
        {
            // An error of PID controller. 
            float error = setpoint - processVariable; 

            // Calculate terms of PID controller. 
            float proportionalTerm = this.ProportionalGain * error;  
            this.IntegralTerm += this.IntegralGain * error * (float)dt.TotalSeconds; 
            float derivativeTerm = this.DerivativeGain * error / (float)dt.TotalSeconds; 
            
            // Calculate output. 
            float output = proportionalTerm + this.IntegralTerm + derivativeTerm; 
            
            // Adjust output variable. 
            if (output >= this.MaxValue)
            {
                output = this.MaxValue; 
            }
            else if (output <= this.MinValue)
            {
                output = this.MinValue; 
            }

            return output; 
        }
        #endregion  // Methods
    }
}