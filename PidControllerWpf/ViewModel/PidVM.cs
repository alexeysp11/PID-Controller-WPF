using System.Windows.Input;
using System.Windows.Threading; 
using PidControllerWpf.View; 
using PidControllerWpf.Model; 
using PidControllerWpf.Commands;

namespace PidControllerWpf.ViewModel
{
    /// <summary>
    /// Allows to connect UI and PidController 
    /// </summary>
    public class PidVM
    {
        public DispatcherTimer TimerGraph { get; private set; } = null; 

        public TextBlockVM _TextBlockVM { get; private set; }
        public GraphCanvasVM _GraphCanvasVM { get; private set; }
        
        private PidController _PidController { get; set; } = null; 
        
        public ICommand ChangeSetpointCommand { get; private set; }
        public ICommand RegulatePvCommand { get; private set; }
        public ICommand StartTimerCommand { get; private set; }
        public ICommand StopTimerCommand { get; private set; }
        public ICommand RestartTimerCommand { get; private set; }

        public static double DelaySeconds = 0.1; 

        #region Constructors
        public PidVM(ref TextBlockVM TextBlockVM, ref GraphCanvasVM GraphCanvasVM)
        {
            try
            {
                InitializeCommands(); 
                InitializeVM(ref TextBlockVM, ref GraphCanvasVM); 
                InitializeModels(); 

                TimerGraph = new DispatcherTimer(); 
                TimerGraph.Tick += new System.EventHandler((o, e) => 
                {
                    _GraphCanvasVM.Time += DelaySeconds; 
                    _TextBlockVM.TimeTextBlock = $"{System.Math.Round(_GraphCanvasVM.Time, 3)}"; 

                    AdjustPv(); 
                    UpdatePidParams(); 
                }); 
                TimerGraph.Interval = System.TimeSpan.FromSeconds(DelaySeconds);
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception: {e}");
            }
        }
        #endregion  // Constructors

        #region Initialize instances 
        private void InitializeCommands()
        {
            this.ChangeSetpointCommand = new ChangeSetpointCommand(this);
            this.RegulatePvCommand = new RegulatePvCommand(this);
            this.StartTimerCommand = new StartTimerCommand(this);
            this.StopTimerCommand = new StopTimerCommand(this);
            this.RestartTimerCommand = new RestartTimerCommand(this);
        }

        private void InitializeVM(ref TextBlockVM TextBlockVM, ref GraphCanvasVM GraphCanvasVM)
        {
            this._TextBlockVM = TextBlockVM;
            this._GraphCanvasVM = GraphCanvasVM;
        }

        private void InitializeModels()
        {
            float minPv = (float)MainWindow.MinPvGraph; 
            float maxPv = (float)MainWindow.MaxPvGraph; 
            _PidController = new PidController(minPv, maxPv); 
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
                System.Windows.MessageBox.Show($"Exception: {e}");
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
                System.Windows.MessageBox.Show($"Exception: {e}");
            }
        }
        #endregion  // Change variables 

        #region Methods
        private void GetSpFromTextBox(ref double value)
        {
            value = System.Convert.ToSingle(_TextBlockVM.SetPointTextBlock);
        }

        private void GetPvFromTextBox(ref double value)
        {
            value = System.Convert.ToSingle(_TextBlockVM.ProcessVariableTextBlock);
        }

        private void SetBoundsForValue(ref double value)
        {
            if (value > MainWindow.MaxPvGraph)
            {
                value = MainWindow.MaxPvGraph;
            }
            else if (value < MainWindow.MinPvGraph)
            {
                value = MainWindow.MinPvGraph;
            }
        }

        private void AdjustPv()
        {
            float pv = (float)_GraphCanvasVM.ProcessVariable; 
            float sp = (float)_GraphCanvasVM.Setpoint; 

            _PidController.ControlPv(ref pv, sp, TimerGraph.Interval); 

            _TextBlockVM.ProcessVariableTextBlock = $"{pv}"; 
            _GraphCanvasVM.ProcessVariable = (double)pv;
        }
        #endregion  // Methods

        #region Updating 
        private void UpdateSpOnGraph(double value)
        {
            _TextBlockVM.SetPointTextBlock = $"{value}"; 
            _GraphCanvasVM.Setpoint = value;
        }

        private void UpdatePvOnGraph(double value)
        {
            _TextBlockVM.ProcessVariableTextBlock = $"{value}"; 
            _GraphCanvasVM.ProcessVariable = value;
        }

        private void UpdatePidParams()
        {
            _TextBlockVM.IntegralErrorTextBlock = $"{_PidController.IntegralTerm}"; 
            _TextBlockVM.ProptionalGainTextBlock = $"{_PidController.ProportionalGain}"; 
            _TextBlockVM.IntegralGainTextBlock = $"{_PidController.IntegralGain}"; 
            _TextBlockVM.DerivativeGainTextBlock = $"{_PidController.DerivativeGain}"; 
        }
        #endregion  // Updating 
    }
}