using System.Collections.Generic; 
using System.ComponentModel;
using System.Windows;
using System.Windows.Shapes;
using PID_Controller_WPF.View; 
using PID_Controller_WPF.Exceptions; 

namespace PID_Controller_WPF.ViewModel
{
    public class GraphCanvasViewModel : INotifyPropertyChanged
    {
        #region Members
        public Point SpRefPoint { get; set; }
        public Point PvRefPoint { get; set; }
        #endregion  // Members

        #region Properties
        /// <summary>
        /// List of lines that are currently on the canvas 
        /// </summary>
        private List<Line> SetpointLines = new List<Line>(); 
        private List<Line> ProcessVarLines = new List<Line>(); 
        /// <summary>
        /// Boolean variable that shows if timer for graph drawing is enabled
        /// </summary>
        public bool IsTimerEnabled = false; 
        
        private bool isSpMovedToInitPoint = false; 
        /// <summary>
        /// Boolean variable that shows if SP moved to (0,0) point on coordinates
        /// </summary>
        public bool IsSpMovedToInitPoint
        {
            get { return isSpMovedToInitPoint; }
            set { isSpMovedToInitPoint = (!value && !isSpMovedToInitPoint) ? false : true; }
        }

        /// <summary>
        /// Boolean variable that shows if PV moved to (0,0) point on coordinates
        /// </summary>        
        private bool isPvMovedToInitPoint = false; 
        public bool IsPvMovedToInitPoint
        {
            get { return isPvMovedToInitPoint; }
            set { isPvMovedToInitPoint = (!value && !isPvMovedToInitPoint) ? false : true; }
        }
        
        public bool IsEverStarted
        {
            get { return IsSpMovedToInitPoint && IsPvMovedToInitPoint; }
        }

        private double processVariable;
        public double ProcessVariable
        {
            get { return processVariable; }
            set 
            { 
                this.DrawVariable(ref value, false, true); 

                // Draw all lines 
                MainWindow.DrawCoordinates(); 
                MainWindow.DrawLine(SetpointLines);
                MainWindow.DrawLine(ProcessVarLines);
                
                processVariable = value; 
            }
        }
                
        private double setpoint; 
        /// <summary>
        /// Allows to move setpoint on the graph according to 
        /// the scale of a graph 
        /// </summary>
        public double Setpoint
        {
            get { return setpoint; } 
            set
            {
                this.DrawVariable(ref value, true, false); 

                // Draw all lines 
                MainWindow.DrawCoordinates(); 
                MainWindow.DrawLine(SetpointLines);
                MainWindow.DrawLine(ProcessVarLines);
                
                setpoint = value; 
            }
        }
        
        private double time; 
        public double Time
        {
            get { return time; } 
            set
            {
                // Set min and max of a time  
                double tmin = MainWindow.MinTimeGraph; 
                double tmax = MainWindow.MaxTimeGraph; 

                if (value < tmin)
                {
                    /* Time could not be less than minimum time, so 
                    you just set time to the min in this case */ 
                    value = tmin; 
                }
                else if (value > tmax - 1)  
                {
                    /* In this block you can move all elements 
                    of a canvas to the left */ 

                    // Increase by delta minimum and maximum time values  
                    MainWindow.MinTimeGraph += 1;
                    MainWindow.MaxTimeGraph += 1;

                    // Assign variables to redefine list of lines
                    try
                    {
                        // Redefine list of lines according to the FIFO principle 
                        foreach (var line in SetpointLines)
                        {
                            line.X1 -= MainWindow.GraphWidth / (tmax - tmin); 
                            line.X2 -= MainWindow.GraphWidth / (tmax - tmin); 
                        }
                        foreach (var line in ProcessVarLines)
                        {
                            line.X1 -= MainWindow.GraphWidth / (tmax - tmin); 
                            line.X2 -= MainWindow.GraphWidth / (tmax - tmin); 
                        }

                        // Set new value for left position of the setpoint 
                        this.SetpointLeft -= MainWindow.GraphWidth / (tmax - tmin); 
                        this.ProcessVariableLeft -= MainWindow.GraphWidth / (tmax - tmin); 

                        // Remove lines that are out of range of a canvas
                        for (int i = SetpointLines.Count - 1; i >= 0 ; i--)
                        {
                            if (SetpointLines[i].X1 < 0 || SetpointLines[i].X2 < 0)
                            {
                                SetpointLines.RemoveAt(i); 
                            }
                        }
                        for (int i = ProcessVarLines.Count - 1; i >= 0 ; i--)
                        {
                            if (ProcessVarLines[i].X1 < 0 || ProcessVarLines[i].X2 < 0)
                            {
                                ProcessVarLines.RemoveAt(i); 
                            }
                        }
                        
                        // Draw coordinates again 
                        MainWindow.DrawCoordinates(); 
                        MainWindow.DrawLine(SetpointLines); 
                        MainWindow.DrawLine(ProcessVarLines); 
                    }
                    catch (System.Exception e)
                    {
                        ExceptionViewer.WatchExceptionMessageBox(e);
                    }
                }
                else 
                {
                    try
                    {
                        // A line of SP that needs to be added to the canvas 
                        Line lineSp = new Line(); 
                        lineSp.Stroke = System.Windows.Media.Brushes.Red;
                        lineSp.StrokeThickness = 1.5;

                        // A line of PV that needs to be added to the canvas 
                        Line linePv = new Line(); 
                        linePv.Stroke = System.Windows.Media.Brushes.Blue;
                        linePv.StrokeThickness = 1.5;

                        // Get the first point of lines for SP and PV
                        lineSp.X1 = this.SetpointLeft; 
                        lineSp.Y1 = this.SetpointTop + 2.5; 
                        linePv.X1 = this.ProcessVariableLeft; 
                        linePv.Y1 = this.ProcessVariableTop + 2.5; 

                        // Set new value for left position of the setpoint 
                        this.SetpointLeft = ((value - tmin) * MainWindow.GraphWidth / (tmax - tmin)) - 2.5; 
                        this.ProcessVariableLeft = ((value - tmin) * MainWindow.GraphWidth / (tmax - tmin)) - 2.5; 
                        
                        // Get the second point of a lines for SP and PV
                        lineSp.X2 = this.SetpointLeft; 
                        lineSp.Y2 = this.SetpointTop + 2.5; 
                        linePv.X2 = this.ProcessVariableLeft; 
                        linePv.Y2 = this.ProcessVariableTop + 2.5; 

                        // Add line to the list and canvas
                        SetpointLines.Add(lineSp);          // Add line to the lines array 
                        SetpointLines.Add(linePv);          // Add line to the lines array 
                        MainWindow.DrawLine(lineSp);        // Draw line 
                        MainWindow.DrawLine(linePv);        // Draw line 
                    }
                    catch (System.Exception e)
                    {
                        ExceptionViewer.WatchExceptionMessageBox(e); 
                    }
                }
                
                // Redefine field `time`
                time = value; 
            }
        }

        private double processVariableTop;
        public double ProcessVariableTop
        {
            get { return processVariableTop; }
            set 
            {
                processVariableTop = value;
                if (processVariableTop < 0)
                {
                    processVariableTop = 0; 
                }
                OnPropertyChanged("ProcessVariableTop");
            }
        }

        private double processVariableLeft;
        public double ProcessVariableLeft
        {
            get { return processVariableLeft; }
            set 
            {
                processVariableLeft = value;
                if (processVariableLeft < 0)
                {
                    processVariableLeft = 0; 
                }
                OnPropertyChanged("ProcessVariableLeft");
            }
        }
        
        private double setpointTop;
        public double SetpointTop
        {
            get { return setpointTop; }
            set 
            {
                setpointTop = value;
                if (setpointTop < 0)
                {
                    setpointTop = 0; 
                }
                OnPropertyChanged("SetpointTop");
            }
        }

        private double setpointLeft;
        public double SetpointLeft
        {
            get { return setpointLeft; }
            set 
            {
                setpointLeft = value;
                if (setpointLeft < 0)
                {
                    setpointLeft = 0; 
                }
                OnPropertyChanged("SetpointLeft");
            }
        }
        #endregion  // Properties

        #region Constructor
        public GraphCanvasViewModel()
        {
            // Set initial values of setpoint 
            this.SetpointTop = 0.0; 
            this.SetpointLeft = 0.0; 

            // Set initial values of process variable  
            this.ProcessVariableTop = 0.0; 
            this.ProcessVariableLeft = 0.0; 
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

        #region Methods
        /// <summary>
        /// Clears all lists of lines used for drawing SP and PV
        /// </summary>
        public void ClearListOfLines()
        {
            SetpointLines.Clear();          // Clear list of lines
            ProcessVarLines.Clear();        // Clear list of lines
            MainWindow.DrawCoordinates();   // Draw line 
        }

        /// <summary>
        /// Allows to draw SP and PV
        /// </summary>
        private void DrawVariable(ref double value, bool isSetpoint, bool isPv)
        {
            double min = MainWindow.MinPvGraph; 
            double max = MainWindow.MaxPvGraph; 
            double VarLeft = 0; 
            double VarTop = 0; 
            Point ReferencePoint = new Point(); 
            List<Line> lines = null; 
            System.Windows.Media.Brush color = null; 

            // Assign variables for SP and PV setting
            if (isSetpoint)
            {
                VarLeft = this.SetpointLeft; 
                VarTop = this.SetpointTop; 
                ReferencePoint = this.SpRefPoint; 
                lines = this.SetpointLines; 
                color = System.Windows.Media.Brushes.Red; 
            }
            else if (isPv)
            {
                VarLeft = this.ProcessVariableLeft; 
                VarTop = this.ProcessVariableTop; 
                ReferencePoint = this.PvRefPoint; 
                lines = this.ProcessVarLines; 
                color = System.Windows.Media.Brushes.Blue; 
            }

            // Correct value
            if (value < min)
            {
                value = min; 
            }
            else if (value > max)
            {
                value = max; 
            }

            // Create an instance of a line 
            Line line = new Line(); 
            line.Stroke = color;
            line.StrokeThickness = 1.5;

            // Set first point of a line before changing position 
            line.X1 = VarLeft; 
            line.Y1 = VarTop + 2.5; 

            // Move setpoint to the left
            VarTop = MainWindow.GraphHeight - (value * MainWindow.GraphHeight / max) - 2.5; 
            
            // Set first point of a line after changing position 
            line.X2 = VarLeft; 
            line.Y2 = VarTop + 2.5; 

            try
            {
                // Correct a list of lines 
                if (!IsTimerEnabled)
                {
                    // Assign ReferencePoint for the first time 
                    if (IsEverStarted)
                    {
                        // Define logical variables 
                        bool isAtPoint = (line.Y1 == ReferencePoint.Y) ? true : false; 
                        bool isUpper = (line.Y1 < ReferencePoint.Y) ? true : false; 
                        bool isLower = (line.Y1 > ReferencePoint.Y) ? true : false; 
                        bool isGoingUp = (line.Y1 > line.Y2) ? true : false; 
                        bool isGoingDown = (line.Y1 < line.Y2) ? true : false; 

                        // Conditions for adding/removing lines from a list 
                        if (isAtPoint)
                        {
                            lines.Add(line); 
                        }
                        else if (isUpper)
                        {
                            if (isGoingUp)
                            {
                                lines.Add(line); 
                            }
                            else if (isGoingDown)
                            {
                                lines.RemoveAt(lines.Count - 1); 
                            }
                        }
                        else if (isLower)
                        {
                            if (isGoingUp)
                            {
                                lines.RemoveAt(lines.Count - 1); 
                            }
                            else if (isGoingDown)
                            {
                                lines.Add(line); 
                            }
                        }
                    }
                    else
                    {
                        ReferencePoint = new Point(line.X2, line.Y2);

                        if (isSetpoint)
                        {
                            IsSpMovedToInitPoint = true; 
                        }
                        else if (isPv)
                        {
                            IsPvMovedToInitPoint = true; 
                        }
                    }
                }
                else
                {
                    lines.Add(line); 
                }
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e); 
            }

            // Reassign variables for SP and PV setting
            if (isSetpoint)
            {
                this.SetpointLeft = VarLeft; 
                this.SetpointTop = VarTop; 
                this.SpRefPoint = ReferencePoint; 
                this.SetpointLines = lines; 
            }
            else if (isPv)
            {
                this.ProcessVariableLeft = VarLeft; 
                this.ProcessVariableTop = VarTop; 
                this.PvRefPoint = ReferencePoint; 
                this.ProcessVarLines = lines; 
            }
        }
        #endregion  // Methods
    }
}