using System.Windows.Input;
using System.Windows.Threading; 
using PidControllerWpf.Views; 
using PidControllerWpf.UserControls; 
using PidControllerWpf.Models; 
using PidControllerWpf.Commands;

namespace PidControllerWpf.ViewModels
{
    /// <summary>
    /// Allows to connect UI and PidController 
    /// </summary>
    public class PidVM
    {
        public DispatcherTimer TimerGraph { get; private set; } = null; 

        public TextBlockVM TextBlockVM { get; private set; }
        public GraphCanvasVM GraphCanvasVM { get; private set; }
        
        private PidController PidController { get; set; } = null; 
        
        public ICommand VariablesCommand { get; private set; }
        public ICommand TimerCommand { get; private set; }

        public static double DelaySeconds = 0.1; 

        public PidVM(ref TextBlockVM textBlockVM, ref GraphCanvasVM graphCanvasVM)
        {
            try
            {
                InitializeCommands(); 
                InitializeVM(ref textBlockVM, ref graphCanvasVM); 
                InitializeModelss(); 

                TimerGraph = new DispatcherTimer(); 
                TimerGraph.Tick += new System.EventHandler((o, e) => 
                {
                    GraphCanvasVM.Time += DelaySeconds; 
                    TextBlockVM.TimeTextBlock = $"{System.Math.Round(GraphCanvasVM.Time, 3)}"; 

                    AdjustPv(); 
                    UpdatePidParams(); 
                }); 
                TimerGraph.Interval = System.TimeSpan.FromSeconds(DelaySeconds);
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }

        #region Initialize instances 
        private void InitializeCommands()
        {
            this.VariablesCommand = new VariablesCommand(this);
            this.VariablesCommand = new VariablesCommand(this);
            this.TimerCommand = new TimerCommand(this);
            this.TimerCommand = new TimerCommand(this);
            this.TimerCommand = new TimerCommand(this);
        }

        private void InitializeVM(ref TextBlockVM textBlockVM, ref GraphCanvasVM graphCanvasVM)
        {
            this.TextBlockVM = textBlockVM;
            this.GraphCanvasVM = graphCanvasVM;
        }

        private void InitializeModelss()
        {
            float minPv = (float)(Graph2D.MinPvGraph); 
            float maxPv = (float)(Graph2D.MaxPvGraph); 
            PidController = new PidController(minPv, maxPv); 
        }
        #endregion  // Initialize instances 

        #region Change variables 
        public void ChangeSetpoint(double delta=1.0f)
        {
            double value = 0; 
            try
            {
                GetSpFromTextBox(ref value); 
                value += delta;
                SetBoundsForValue(ref value); 
                UpdateSpOnGraph(value); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }

        public void ChangeProcessVariable(double delta=1.0f)
        {
            double value = 0; 
            try
            {
                GetPvFromTextBox(ref value); 
                value += delta;
                SetBoundsForValue(ref value); 
                UpdatePvOnGraph(value); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }
        #endregion  // Change variables 

        #region Methods
        private void GetSpFromTextBox(ref double value)
        {
            value = System.Convert.ToSingle(TextBlockVM.SetPointTextBlock);
        }

        private void GetPvFromTextBox(ref double value)
        {
            value = System.Convert.ToSingle(TextBlockVM.ProcessVariableTextBlock);
        }

        private void SetBoundsForValue(ref double value)
        {
            if (value > Graph2D.MaxPvGraph)
            {
                value = Graph2D.MaxPvGraph;
            }
            else if (value < Graph2D.MinPvGraph)
            {
                value = Graph2D.MinPvGraph;
            }
        }

        private void AdjustPv()
        {
            float pv = (float)GraphCanvasVM.ProcessVariable; 
            float sp = (float)GraphCanvasVM.Setpoint; 

            PidController.ControlPv(ref pv, sp, TimerGraph.Interval); 

            TextBlockVM.ProcessVariableTextBlock = pv.ToString(); 
            GraphCanvasVM.ProcessVariable = (double)pv;
        }
        #endregion  // Methods

        #region Updating 
        private void UpdateSpOnGraph(double value)
        {
            TextBlockVM.SetPointTextBlock = value.ToString(); 
            GraphCanvasVM.Setpoint = value;
        }

        private void UpdatePvOnGraph(double value)
        {
            TextBlockVM.ProcessVariableTextBlock = value.ToString(); 
            GraphCanvasVM.ProcessVariable = value;
        }

        private void UpdatePidParams()
        {
            TextBlockVM.IntegralErrorTextBlock = PidController.IntegralTerm.ToString(); 
            TextBlockVM.ProptionalGainTextBlock = PidController.ProportionalGain.ToString(); 
            TextBlockVM.IntegralGainTextBlock = PidController.IntegralGain.ToString(); 
            TextBlockVM.DerivativeGainTextBlock = PidController.DerivativeGain.ToString(); 
        }
        #endregion  // Updating 
    }
}