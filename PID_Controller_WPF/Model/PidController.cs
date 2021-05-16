namespace PID_Controller_WPF.Model
{
    class PidController
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public float ProcessVariable { get; private set; } = 0; 
        /// <summary>
        /// 
        /// </summary>
        public float SetPoint { get; private set; } = 0; 
        /// <summary>
        /// 
        /// </summary>
        public float ProportionalGain { get; private set; } = 0; 
        /// <summary>
        /// 
        /// </summary>
        public float IntegralGain { get; private set; } = 0; 
        /// <summary>
        /// 
        /// </summary>
        public float DerivativeGain { get; private set; } = 0; 
        /// <summary>
        /// 
        /// </summary>
        public float IntegralTerm { get; private set; } = 0; 
        /// <summary>
        /// 
        /// </summary>
        public float MaxValue { get; private set; } = 0; 
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="dt"></param>
        private float Control(float dt)
        {
            // An error of PID controller. 
            float error = this.SetPoint - this.ProcessVariable; 

            // Calculate terms of PID controller. 
            float proportionalTerm = 0;  
            this.IntegralTerm += this.IntegralGain * error * dt; 
            float derivativeTerm = this.DerivativeGain * error / dt; 
            
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