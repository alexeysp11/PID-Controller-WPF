using System; 
using System.Windows.Input;
using PidControllerWpf.ViewModels; 

namespace PidControllerWpf.Commands
{
    public class VariablesCommand : ICommand
    {
        public PidVM PidVM { get; set; }

        public VariablesCommand(PidVM pidVM)
        {
            this.PidVM = pidVM; 
        }

        public event EventHandler CanExecuteChanged; 

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            double delta = 1;      // Delta for process variable
            string parameterString = parameter as string;
            if (parameterString == "IncreaseSP")
            {
                this.PidVM.ChangeSetpoint(delta);
            }
            else if (parameterString == "DecreaseSP")
            {
                this.PidVM.ChangeSetpoint(-delta);
            }
            else if (parameterString == "IncreasePV")
            {
                this.PidVM.ChangeProcessVariable(delta);
            }
            else if (parameterString == "DecreasePV")
            {
                this.PidVM.ChangeProcessVariable(-delta);
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect parameter: {parameterString}", "Error"); 
            }
        }
    }
}