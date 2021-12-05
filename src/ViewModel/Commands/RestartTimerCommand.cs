using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.View; 
using PidControllerWpf.UserControls; 
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    class RestartTimerCommand : ICommand
    {
        private PidVM PidVM { get; set; } 

        public RestartTimerCommand(PidVM PidVM)
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
    }
}