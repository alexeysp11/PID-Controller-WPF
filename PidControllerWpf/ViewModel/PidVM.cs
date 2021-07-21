using System.Windows.Input;
using System.Windows.Threading; 
using PidControllerWpf.View; 
using PidControllerWpf.Model; 
using PidControllerWpf.Commands;
using PidControllerWpf.Exceptions; 

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
        
        #region Commands
        public ICommand ChangeSetpointCommand { get; set; }
        public ICommand RegulatePvCommand { get; set; }

        public ICommand StartTimerCommand { get; set; }
        public ICommand StopTimerCommand { get; set; }
        public ICommand RestartTimerCommand { get; set; }
        #endregion  // Commands

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
            this.ChangeVar(delta, true, false); 
        }

        public void ChangeProcessVariable(double delta=1.0f)
        {
            this.ChangeVar(delta, false, true); 
        }

        /// <summary>
        /// Changes a variable on the screen 
        /// </summary>
        private void ChangeVar(double delta, bool isSetpoint, bool isPv)
        {
            double value = 0; 
            try
            {
                GetValueFromTextBox(isSetpoint, isPv, ref value); 
                value += delta;
                SetBoundsForValue(ref value); 
                UpdateGraph(isSetpoint, isPv, value); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception: {e}");
            }
        }
        #endregion  // Change variables 

        #region Methods
        private void GetValueFromTextBox(bool isSetpoint, bool isPv, ref double value)
        {
            if (isSetpoint)
            {
                value = System.Convert.ToSingle(_TextBlockVM.SetPointTextBlock);
            }
            else if (isPv)
            {
                value = System.Convert.ToSingle(_TextBlockVM.ProcessVariableTextBlock);
            }
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
        private void UpdateGraph(bool isSetpoint, bool isPv, double value)
        {
            if (isSetpoint)
            {
                _TextBlockVM.SetPointTextBlock = $"{value}"; 
                _GraphCanvasVM.Setpoint = value;
            }
            else if (isPv)
            {
                _TextBlockVM.ProcessVariableTextBlock = $"{value}"; 
                _GraphCanvasVM.ProcessVariable = value;
            }
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