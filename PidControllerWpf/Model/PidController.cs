namespace PidControllerWpf.Model
{
    /// <summary>
    /// Adjusts PV  
    /// </summary>
    public class PidController
    {
        #region Properties 
        public float ProportionalGain { get; private set; } = 0; 
        public float IntegralGain { get; private set; } = 0; 
        public float DerivativeGain { get; private set; } = 0; 
        public float IntegralTerm { get; private set; } = 0; 
        #endregion  // Properties 

        #region Private fields
        private float MaxPv = 0; 
        private float MinPv = 0; 
        #endregion  // Private fields

        #region Constructors
        /// <summary>
        /// Parameterized constructor of a class `PidController`
        /// </summary>
        /// <param name="minValue">Minimal value of process variable and setpoint</param>
        /// <param name="maxValue">Maximal value of process variable and setpoint</param>
        public PidController(float minValue, float maxValue)
        {
            this.MaxPv = maxValue; 
            this.MinPv = minValue; 

            this.ProportionalGain = -0.8f; 
            this.IntegralGain = 1.0f; 
            this.DerivativeGain = 0.1f; 
        }
        #endregion  // Constructors

        #region Methods
        /// <summary>
        /// Allows to control process variable 
        /// </summary>
        /// <param name="pv">Value of PV at current time</param>
        /// <param name="setpoint">Desired setpoint</param>
        /// <param name="dt">Delta time</param>
        public float ControlPv(float pv, float setpoint, System.TimeSpan dt)
        {
            float error = setpoint - pv; 

            float proportionalTerm = this.ProportionalGain * error;  
            this.IntegralTerm += this.IntegralGain * error * (float)dt.TotalSeconds; 
            float derivativeTerm = this.DerivativeGain * error / (float)dt.TotalSeconds; 
            
            float output = proportionalTerm + this.IntegralTerm + derivativeTerm; 
            
            if (output >= this.MaxPv)
            {
                output = this.MaxPv; 
            }
            else if (output <= this.MinPv)
            {
                output = this.MinPv; 
            }

            return output; 
        }
        #endregion  // Methods
    }
}