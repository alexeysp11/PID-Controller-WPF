using System.ComponentModel;

namespace PidControllerWpf.ViewModel
{
    public class TextBlockVM : INotifyPropertyChanged
    {
        private string setpoint;
        public string SetPointTextBlock
        {
            get { return setpoint; }
            set 
            {
                setpoint = value;
                OnPropertyChanged("SetPointTextBlock");
            }
        }

        private string processVariable;
        public string ProcessVariableTextBlock
        {
            get { return processVariable; }
            set 
            {
                processVariable = value;
                OnPropertyChanged("ProcessVariableTextBlock");
            }
        }

        private string time;
        public string TimeTextBlock
        {
            get { return time; }
            set 
            {
                time = value;
                OnPropertyChanged("TimeTextBlock");
            }
        }

        #region PID controller's parameters 
        private string proptionalGain;
        public string ProptionalGainTextBlock
        {
            get { return proptionalGain; }
            set 
            {
                proptionalGain = value;
                OnPropertyChanged("ProptionalGainTextBlock");
            }
        }

        private string integralGain;
        public string IntegralGainTextBlock
        {
            get { return integralGain; }
            set 
            {
                integralGain = value;
                OnPropertyChanged("IntegralGainTextBlock");
            }
        }

        private string derivativeGain;
        public string DerivativeGainTextBlock
        {
            get { return derivativeGain; }
            set 
            {
                derivativeGain = value;
                OnPropertyChanged("DerivativeGainTextBlock");
            }
        }

        private string integralError;
        public string IntegralErrorTextBlock
        {
            get { return integralError; }
            set 
            {
                integralError = value;
                OnPropertyChanged("IntegralErrorTextBlock");
            }
        }
        #endregion  // PID controller's parameters 

        public TextBlockVM()
        {
            this.SetPointTextBlock = "0"; 
            this.ProcessVariableTextBlock = "0"; 
            this.TimeTextBlock = "0"; 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(PropertyName);
                handler(this, e);
            }
        }
    }
}