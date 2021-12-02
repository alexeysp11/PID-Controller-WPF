namespace PidControllerWpf.Model
{
    public class PidController
    {
        public float ProportionalGain { get; private set; } = 0; 
        public float IntegralGain { get; private set; } = 0; 
        public float DerivativeGain { get; private set; } = 0; 
        public float IntegralTerm { get; private set; } = 0; 

        private float MaxPv = 0; 
        private float MinPv = 0; 

        public PidController(float minValue, float maxValue)
        {
            this.MaxPv = maxValue; 
            this.MinPv = minValue; 

            this.ProportionalGain = -0.8f; 
            this.IntegralGain = 1.0f; 
            this.DerivativeGain = 0.1f; 
        }
        
        public void ControlPv(ref float pv, float setpoint, System.TimeSpan deltaTime)
        {
            float error = setpoint - pv; 

            float proportionalTerm = this.ProportionalGain * error;  
            this.IntegralTerm += this.IntegralGain * error * (float)deltaTime.TotalSeconds; 
            float derivativeTerm = this.DerivativeGain * error / (float)deltaTime.TotalSeconds; 
            
            float output = proportionalTerm + this.IntegralTerm + derivativeTerm; 
            
            if (output >= this.MaxPv)
            {
                output = this.MaxPv; 
            }
            else if (output <= this.MinPv)
            {
                output = this.MinPv; 
            }
            pv = output; 
        }
    }
}