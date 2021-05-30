using System.Windows.Input;
using System.Windows.Threading; 
using PID_Controller_WPF.View; 
using PID_Controller_WPF.Model; 
using PID_Controller_WPF.Commands;
using PID_Controller_WPF.Exceptions; 

namespace PID_Controller_WPF.ViewModel
{
    /// <summary>
    /// ViewModel for changing elements related to PidController
    /// </summary>
    public class PidViewModel
    {
        #region Members
        public DispatcherTimer TimerGraph { get; private set; } = null; 
        #endregion  // Members
        
        #region Commands
        /// <summary>
        /// Command for changing setpoint
        /// </summary>
        public ICommand ChangeSetpointCommand { get; set; }
        /// <summary>
        /// Command for regulating process variable
        /// </summary>
        public ICommand RegulatePvCommand { get; set; }
        /// <summary>
        /// Command used to start a timer 
        /// </summary>
        public ICommand StartTimerCommand { get; set; }
        /// <summary>
        /// Command used to stop a timer 
        /// </summary>
        public ICommand StopTimerCommand { get; set; }
        /// <summary>
        /// Command used to restart a timer 
        /// </summary>
        public ICommand RestartTimerCommand { get; set; }
        #endregion  // Commands

        #region ViewModels
        /// <summary>
        /// ViewModel for displaying setpoint
        /// </summary>
        public TextBlockViewModel _TextBlockViewModel { get; private set; }
        /// <summary>
        /// ViewModel for drawing a graph
        /// </summary>
        public GraphCanvasViewModel _GraphCanvasViewModel { get; private set; }
        #endregion  // ViewModels

        #region Models
        private PidController _PidController { get; set; } = null; 
        #endregion  // Models

        #region Properties 
        public static double DelaySeconds = 0.1; 
        #endregion  // Properties 

        #region Constructor
        /// <summary>
        /// Constructor of PidViewModel class
        /// </summary>
        public PidViewModel(ref TextBlockViewModel textBlockViewModel, ref GraphCanvasViewModel graphCanvasViewModel)
        {
            try
            {
                // Commands 
                this.ChangeSetpointCommand = new ChangeSetpointCommand(this);
                this.RegulatePvCommand = new RegulatePvCommand(this);
                this.StartTimerCommand = new StartTimerCommand(this);
                this.StopTimerCommand = new StopTimerCommand(this);
                this.RestartTimerCommand = new RestartTimerCommand(this);

                // ViewModels
                this._TextBlockViewModel = textBlockViewModel;
                this._GraphCanvasViewModel = graphCanvasViewModel;

                // Models
                _PidController = new PidController(
                    (float)MainWindow.MinPvGraph, 
                    (float)MainWindow.MaxPvGraph
                ); 

                // Add timer for updating graph and visual elements 
                TimerGraph = new DispatcherTimer(); 
                TimerGraph.Tick += new System.EventHandler((o, e) => 
                {
                    // Increase time and change text block for time
                    _GraphCanvasViewModel.Time += DelaySeconds; 
                    _TextBlockViewModel.TimeTextBlock = $"{System.Math.Round(_GraphCanvasViewModel.Time, 3)}"; 

                    // Call method of PID controller to adjust PV
                    float processVariable = _PidController.ControlPv(
                        (float)_GraphCanvasViewModel.ProcessVariable, 
                        (float)_GraphCanvasViewModel.Setpoint, 
                        TimerGraph.Interval
                    ); 
                    _TextBlockViewModel.ProcessVariableTextBlock = $"{processVariable}"; 
                    _GraphCanvasViewModel.ProcessVariable = (double)processVariable;

                    // Update parameters of PID controller on the canvas
                    _TextBlockViewModel.IntegralErrorTextBlock = $"{_PidController.IntegralTerm}"; 
                    _TextBlockViewModel.ProptionalGainTextBlock = $"{_PidController.ProportionalGain}"; 
                    _TextBlockViewModel.IntegralGainTextBlock = $"{_PidController.IntegralGain}"; 
                    _TextBlockViewModel.DerivativeGainTextBlock = $"{_PidController.DerivativeGain}"; 
                }); 
                TimerGraph.Interval = System.TimeSpan.FromSeconds(DelaySeconds);
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
        #endregion  // Constructor

        #region Methods
        /// <summary>
        /// Changes setpoint value on the screen 
        /// </summary>
        public void ChangeSetpoint(double delta=1.0f)
        {
            this.ChangeVar(delta, true, false); 
        }

        /// <summary>
        /// Changes process variable on the screen 
        /// </summary>
        public void ChangeProcessVariable(double delta=1.0f)
        {
            this.ChangeVar(delta, false, true); 
        }

        /// <summary>
        /// Changes a variable on the screen 
        /// </summary>
        private void ChangeVar(double delta, bool isSetpoint, bool isPv)
        {
            // Assign value 
            double value = 0; 

            // Get value from textblock 
            try
            {
                if (isSetpoint)
                {
                    value = System.Convert.ToSingle(_TextBlockViewModel.SetPointTextBlock);
                }
                else if (isPv)
                {
                    value = System.Convert.ToSingle(_TextBlockViewModel.ProcessVariableTextBlock);
                }
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }

            // Correct value 
            value += delta;
            if (value > MainWindow.MaxPvGraph)
            {
                value = MainWindow.MaxPvGraph;
            }
            else if (value < MainWindow.MinPvGraph)
            {
                value = MainWindow.MinPvGraph;
            }
            
            // Update graph
            try
            {
                if (isSetpoint)
                {
                    _TextBlockViewModel.SetPointTextBlock = $"{value}"; 
                    _GraphCanvasViewModel.Setpoint = value;
                }
                else if (isPv)
                {
                    _TextBlockViewModel.ProcessVariableTextBlock = $"{value}"; 
                    _GraphCanvasViewModel.ProcessVariable = value;
                }
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
        #endregion  // Methods
    }
}