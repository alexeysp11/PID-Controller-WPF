using System.Windows.Input;
using System.Windows.Threading; 
using PID_Controller_WPF.View; 
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
        public DispatcherTimer TimerGraph = null; 
        #endregion  // Members
        
        #region Commands
        /// <summary>
        /// Command for increasing setpoint
        /// </summary>
        public ICommand SetpointUpCommand { get; set; }
        /// <summary>
        /// Command for decreasing setpoint
        /// </summary>
        public ICommand SetpointDownCommand { get; set; }
        #endregion  // Commands

        #region ViewModels
        /// <summary>
        /// ViewModel for displaying setpoint
        /// </summary>
        private TextBlockViewModel _TextBlockViewModel { get; set; }
        /// <summary>
        /// ViewModel for drawing a graph
        /// </summary>
        private GraphCanvasViewModel _GraphCanvasViewModel { get; set; }
        #endregion  // ViewModels

        #region Properties 
        public static double DelaySeconds = 0.1; 
        #endregion  // Properties 

        #region Constructor
        /// <summary>
        /// Constructor of PidViewModel class
        /// </summary>
        public PidViewModel(ref TextBlockViewModel textBlockViewModel, ref GraphCanvasViewModel graphCanvasViewModel)
        {
            // Commands 
            this.SetpointUpCommand = new SetpointUpCommand(this);
            this.SetpointDownCommand = new SetpointDownCommand(this);

            // ViewModels
            this._TextBlockViewModel = textBlockViewModel;
            this._GraphCanvasViewModel = graphCanvasViewModel;

            // Add timer for updating graph and visual elements 
            TimerGraph = new DispatcherTimer(); 
            TimerGraph.Tick += new System.EventHandler((o, e) => 
            {
                _GraphCanvasViewModel.Time += DelaySeconds; 
                _TextBlockViewModel.TimeTextBlock = $"{System.Math.Round(_GraphCanvasViewModel.Time, 3)}"; 
            }); 
            TimerGraph.Interval = System.TimeSpan.FromSeconds(DelaySeconds);
        }
        #endregion  // Constructor

        #region Methods
        /// <summary>
        /// Changes setpoint value on the screen 
        /// </summary>
        public void ChangeSetpoint(double delta=1.0f)
        {
            // Restart timer for drawing a graph 
            if (TimerGraph.IsEnabled)
            {
                TimerGraph.Stop(); 
            }
            TimerGraph.Start(); 
            
            // Set default value of setpoint 
            double setpoint = 0; 

            // Update textblock 
            try
            {
                setpoint = System.Convert.ToSingle(_TextBlockViewModel.SetPointTextBlock);
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show("Unable to convert string to float");
                ExceptionViewer.WatchExceptionMessageBox(e);
            }

            // Correct setpoint value 
            setpoint += delta;
            if (setpoint > MainWindow.MaxPvGraph)
            {
                setpoint = MainWindow.MaxPvGraph;
            }
            else if (setpoint < MainWindow.MinPvGraph)
            {
                setpoint = MainWindow.MinPvGraph;
            }
            _TextBlockViewModel.SetPointTextBlock = $"{setpoint}"; 

            // Update graph
            try
            {
                _GraphCanvasViewModel.Setpoint = setpoint;
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
        #endregion  // Methods
    }
}