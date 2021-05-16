using System.ComponentModel;

namespace PID_Controller_WPF.ViewModel
{
    public class TextBlockViewModel : INotifyPropertyChanged
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

        public TextBlockViewModel()
        {
            this.SetPointTextBlock = "0"; 
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