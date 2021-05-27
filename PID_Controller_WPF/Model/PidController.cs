namespace PID_Controller_WPF.Model
{
    class PidController
    {
        #region Properties
        /// <summary>
        /// Value of the process variable 
        /// </summary>
        public float ProcessVariable { get; private set; } = 0; 
        /// <summary>
        /// Set point of PID controller 
        /// </summary>
        public float SetPoint { get; private set; } = 0; 
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
        /// <summary>
        /// Max value of process variables 
        /// </summary>
        public float MaxValue { get; private set; } = 0; 
        /// <summary>
        /// Min value of process variables 
        /// </summary>
        public float MinValue { get; private set; } = 0; 
        #endregion  // Properties

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public PidController(float minValue, float maxValue)
        {
            this.MaxValue = maxValue; 
            this.MinValue = minValue; 
        }
        #endregion  // Constructor

        #region Methods
        /// <summary>
        /// Allows to control process variable 
        /// </summary>
        /// <param name="dt"></param>
        private float Control(System.TimeSpan dt)
        {
            // An error of PID controller. 
            float error = this.SetPoint - this.ProcessVariable; 

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