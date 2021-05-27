using System.ComponentModel;

namespace PID_Controller_WPF.ViewModel
{
    public class TextBlockViewModel : INotifyPropertyChanged
    {
        #region Properties
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
        #endregion  // Properties

        #region Constructor
        public TextBlockViewModel()
        {
            this.SetPointTextBlock = "0"; 
            this.ProcessVariableTextBlock = "0"; 
            this.TimeTextBlock = "0"; 
        }
        #endregion  // Constructor

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