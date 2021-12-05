using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.Views; 
using PidControllerWpf.UserControls; 
using PidControllerWpf.ViewModels; 

namespace PidControllerWpf.Commands
{
    class TimerCommand : ICommand
    {
        private PidVM PidVM { get; set; } 

        public TimerCommand(PidVM PidVM)
        {
            this.PidVM = PidVM; 
        }

        public event System.EventHandler CanExecuteChanged; 
        
        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            string parameterString = parameter as string; 
            if (parameterString == "Start")
            {
                Start(); 
            }
            else if (parameterString == "Restart")
            {
                Restart(); 
            }
            else if (parameterString == "Stop")
            {
                Stop(); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect parameter: {parameterString}", "Error"); 
            }
        }

        private void Start()
        {
            try
            {
                this.PidVM.TimerGraph.Start(); 
                this.PidVM.GraphCanvasVM.IsTimerEnabled = true; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }

        private void Restart()
        {
            try
            {
                this.PidVM.TimerGraph.Stop(); 
                
                GraphCanvasVM gcvm = this.PidVM.GraphCanvasVM; 
                gcvm.IsTimerEnabled = false; 

                Graph2D.MinTimeGraph = Graph2D.InitMinTimeGraph; 
                Graph2D.MaxTimeGraph = Graph2D.InitMaxTimeGraph; 

                gcvm.Setpoint = 0; 
                PidVM.TextBlockVM.SetPointTextBlock = gcvm.Setpoint.ToString(); 
                
                gcvm.ProcessVariable = 0; 
                PidVM.TextBlockVM.ProcessVariableTextBlock = gcvm.ProcessVariable.ToString(); 

                gcvm.Time = 0; 
                PidVM.TextBlockVM.TimeTextBlock = gcvm.Time.ToString();

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.SpRefPoint = refpoint; 

                gcvm.ClearListOfLines(); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }

        private void Stop()
        {
            try
            {
                this.PidVM.TimerGraph.Stop(); 
                GraphCanvasVM gcvm = this.PidVM.GraphCanvasVM; 
                gcvm.IsTimerEnabled = false; 

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.SpRefPoint = refpoint; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }
    }
}