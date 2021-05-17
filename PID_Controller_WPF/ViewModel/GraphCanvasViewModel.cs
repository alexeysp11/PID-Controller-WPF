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
        public Point ReferencePoint { get; set; }
        #endregion  // Members

        #region Properties
        /// <summary>
        /// List of lines that are currently on the canvas 
        /// </summary>
        private List<Line> lines = new List<Line>(); 
        /// <summary>
        /// Boolean variable that shows if timer for graph drawing is enabled
        /// </summary>
        public bool IsTimerEnabled = false; 
        
        private bool isEverStarted = false;
        public bool IsEverStarted
        {
            get { return isEverStarted; }
            set { isEverStarted = (!value && !isEverStarted) ? false : true; }
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
                double min = MainWindow.MinPvGraph; 
                double max = MainWindow.MaxPvGraph; 
                
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
                line.Stroke = System.Windows.Media.Brushes.Red;
                line.StrokeThickness = 1.5;

                // Set first point of a line before changing position 
                line.X1 = this.SetpointLeft; 
                line.Y1 = this.SetpointTop + 2.5; 

                // Move setpoint to the left
                this.SetpointTop = MainWindow.GraphHeight - (value * MainWindow.GraphHeight / max) - 2.5; 
                
                // Set first point of a line after changing position 
                line.X2 = this.SetpointLeft; 
                line.Y2 = this.SetpointTop + 2.5; 

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
                            IsEverStarted = true; 
                        }
                    }
                    else
                    {
                        lines.Add(line); 
                    }
                    
                    // Draw line 
                    MainWindow.DrawCoordinates(); 
                    MainWindow.DrawLine(lines);
                }
                catch (System.Exception e)
                {
                    ExceptionViewer.WatchExceptionMessageBox(e); 
                }
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
                        foreach (var line in lines)
                        {
                            line.X1 -= MainWindow.GraphWidth / (tmax - tmin); 
                            line.X2 -= MainWindow.GraphWidth / (tmax - tmin); 
                        }

                        // Set new value for left position of the setpoint 
                        this.SetpointLeft -= MainWindow.GraphWidth / (tmax - tmin); 

                        // Remove lines that are out of range of a canvas
                        for (int i = lines.Count - 1; i >= 0 ; i--)
                        {
                            if (lines[i].X1 < 0 || lines[i].X2 < 0)
                            {
                                lines.RemoveAt(i); 
                            }
                        }
                        
                        // Draw coordinates again 
                        MainWindow.DrawCoordinates(); 
                        MainWindow.DrawLine(lines); 
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
                        // A line that needs to be added to the canvas 
                        Line line = new Line(); 
                        line.Stroke = System.Windows.Media.Brushes.Red;
                        line.StrokeThickness = 1.5;

                        // Get coordinates of the first point of a line 
                        line.X1 = this.SetpointLeft; 
                        line.Y1 = this.SetpointTop + 2.5; 

                        // Set new value for left position of the setpoint 
                        this.SetpointLeft = ((value - tmin) * MainWindow.GraphWidth / (tmax - tmin)) - 2.5; 
                        
                        // Get coordinates of the second point of a line
                        line.X2 = this.SetpointLeft; 
                        line.Y2 = this.SetpointTop + 2.5; 

                        // Add line to the list and canvas
                        lines.Add(line);            // Add line to the lines array 
                        MainWindow.DrawLine(line);  // Draw line 
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
        public void ClearListOfLines()
        {
            lines.Clear();                  // Clear list of lines
            MainWindow.DrawCoordinates();   // Draw line 
        }
        #endregion  // Methods
    }
}