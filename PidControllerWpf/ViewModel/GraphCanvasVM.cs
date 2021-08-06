using System.Collections.Generic; 
using System.ComponentModel;
using System.Windows;
using System.Windows.Shapes;
using PidControllerWpf.View; 

namespace PidControllerWpf.ViewModel
{
    public class GraphCanvasVM : INotifyPropertyChanged
    {
        #region Reference points 
        public Point SpRefPoint { get; set; }
        public Point PvRefPoint { get; set; }
        #endregion  // Reference points 

        #region Properties
        private List<Line> SetpointLines = new List<Line>(); 
        private List<Line> ProcessVarLines = new List<Line>(); 

        public bool IsTimerEnabled = false; 
        
        private bool isSpMovedToInitPoint = false; 
        public bool IsSpMovedToInitPoint
        {
            get { return isSpMovedToInitPoint; }
            set { isSpMovedToInitPoint = (!value && !isSpMovedToInitPoint) ? false : true; }
        }
     
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

        private double time; 
        public double Time
        {
            get { return time; } 
            set
            {
                SetTime(ref value); 
                time = value; 
            }
        }
        #endregion  // Properties

        #region Process variable 
        private double processVariable;
        public double ProcessVariable
        {
            get { return processVariable; }
            set 
            { 
                this.DrawVariable(ref value, false, true); 

                MainWindow.DrawCoordinates(); 
                MainWindow.DrawLine(SetpointLines);
                MainWindow.DrawLine(ProcessVarLines);
                
                processVariable = value; 
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
        #endregion  // Process variable 
                
        #region Setpoint
        private double setpoint; 
        public double Setpoint
        {
            get { return setpoint; } 
            set
            {
                this.DrawVariable(ref value, true, false); 

                MainWindow.DrawCoordinates(); 
                MainWindow.DrawLine(SetpointLines);
                MainWindow.DrawLine(ProcessVarLines);
                
                setpoint = value; 
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
        #endregion  // Setpoint
        
        #region Constructors 
        public GraphCanvasVM()
        {
            this.SetpointTop = 0.0; 
            this.SetpointLeft = 0.0; 

            this.ProcessVariableTop = 0.0; 
            this.ProcessVariableLeft = 0.0; 
        }
        #endregion  // Constructors 

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
            SetpointLines.Clear(); 
            ProcessVarLines.Clear(); 
            MainWindow.DrawCoordinates(); 
        }

        private void DrawVariable(ref double value, bool isSetpoint, bool isPv)
        {
            double min = MainWindow.MinPvGraph; 
            double max = MainWindow.MaxPvGraph; 

            double varLeft = 0; 
            double varTop = 0; 

            Point referencePoint = new Point(); 
            List<Line> lines = null; 
            System.Windows.Media.Brush color = null; 

            AssignVariable(isSetpoint, isPv, ref varLeft, ref varTop, 
                ref referencePoint, ref lines, ref color); 
            SetBounds(ref value, min, max); 

            Line line = new Line(); 
            DrawLineMovingPoint(varLeft, ref varTop, value, max, color, ref line); 

            try
            {
                CorrectLinesList(isSetpoint, isPv, line, ref referencePoint, ref lines); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception: {e}"); 
            }

            ReassignVariable(isSetpoint, isPv, varLeft, varTop, referencePoint, lines); 
        }

        private void DrawLineMovingPoint(double varLeft, ref double varTop, double value,
            double max, System.Windows.Media.Brush color, ref Line line)
        {
            line.Stroke = color;
            line.StrokeThickness = 1.5;

            line.X1 = varLeft; 
            line.Y1 = varTop + 2.5;     // Little shift to bottom, radius of a point is 5. 

            varTop = MainWindow.GraphHeight - (value * MainWindow.GraphHeight / max) - 2.5; 
            
            line.X2 = varLeft; 
            line.Y2 = varTop + 2.5;     // Little shift to bottom, radius of a point is 5. 
        }

        private void SetBounds(ref double value, double min, double max)
        {
            if (value < min)
            {
                value = min; 
            }
            else if (value > max)
            {
                value = max; 
            }
        }
        #endregion  // Methods

        #region List of lines 
        private void CorrectLinesList(bool isSetpoint, bool isPv, Line line, 
            ref Point referencePoint, ref List<Line> lines)
        {
            try
            {
                if (!IsTimerEnabled)
                {
                    AssignRefPoint(isSetpoint, isPv, line, ref referencePoint, ref lines);
                }
                else
                {
                    lines.Add(line); 
                }
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }
        #endregion  // List of lines 

        #region Assign variables
        private void AssignVariable(bool isSetpoint, bool isPv, 
            ref double varLeft, ref double varTop, ref Point referencePoint, 
            ref List<Line> lines, ref System.Windows.Media.Brush color)
        {
            if (isSetpoint)
            {
                AssignVarAsSetpoint(out varLeft, out varTop, out referencePoint, 
                    out lines, out color); 
            }
            else if (isPv)
            {
                AssignVarAsPv(out varLeft, out varTop, out referencePoint, 
                    out lines, out color);
            }
        }
        private void AssignVarAsSetpoint(out double varLeft, out double varTop, 
            out Point referencePoint, out List<Line> lines, out System.Windows.Media.Brush color)
        {
            varLeft = this.SetpointLeft; 
            varTop = this.SetpointTop; 
            referencePoint = this.SpRefPoint; 
            lines = this.SetpointLines; 
            color = System.Windows.Media.Brushes.Red; 
        }

        private void AssignVarAsPv(out double varLeft, out double varTop, 
            out Point referencePoint, out List<Line> lines, out System.Windows.Media.Brush color)
        {
            varLeft = this.ProcessVariableLeft; 
            varTop = this.ProcessVariableTop; 
            referencePoint = this.PvRefPoint; 
            lines = this.ProcessVarLines; 
            color = System.Windows.Media.Brushes.Blue; 
        }

        private void AssignRefPoint(bool isSetpoint, bool isPv, Line line, 
            ref Point referencePoint, ref List<Line> lines)
        {
            if (IsEverStarted)
            {
                ReassignRefPoint(line, ref referencePoint, ref lines); 
            }
            else
            {
                referencePoint = new Point(line.X2, line.Y2);
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
        #endregion  // Assign variables

        #region Reassign variables 
        private void ReassignVariable(bool isSetpoint, bool isPv, double varLeft, 
            double varTop, Point referencePoint, List<Line> lines) 
        {
            if (isSetpoint)
            {
                ReassignSetpoint(varLeft, varTop, referencePoint, lines);
            }
            else if (isPv)
            {
                ReassignPv(varLeft, varTop, referencePoint, lines); 
            }
        }

        private void ReassignSetpoint(double varLeft, double varTop, 
            Point referencePoint, List<Line> lines)
        {
            this.SetpointLeft = varLeft; 
            this.SetpointTop = varTop; 
            this.SpRefPoint = referencePoint; 
            this.SetpointLines = lines; 
        }

        private void ReassignPv(double varLeft, double varTop, 
            Point referencePoint, List<Line> lines)
        {
            this.ProcessVariableLeft = varLeft; 
            this.ProcessVariableTop = varTop; 
            this.PvRefPoint = referencePoint; 
            this.ProcessVarLines = lines; 
        }

        private void ReassignRefPoint(Line line, ref Point referencePoint, ref List<Line> lines)
        {
            // Define logical variables 
            bool isAtPoint = (line.Y1 == referencePoint.Y) ? true : false; 
            bool isUpper = (line.Y1 < referencePoint.Y) ? true : false; 
            bool isLower = (line.Y1 > referencePoint.Y) ? true : false; 
            bool isGoingUp = (line.Y1 > line.Y2) ? true : false; 
            bool isGoingDown = (line.Y1 < line.Y2) ? true : false; 

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
        #endregion  // Reassign variables 

        #region Time methods 
        private void SetTime(ref double value)
        {
            double tmin = MainWindow.MinTimeGraph; 
            double tmax = MainWindow.MaxTimeGraph; 

            if (value < tmin)
            {
                value = tmin; 
            }
            else if (value > tmax - 1)  
            {
                MainWindow.MinTimeGraph += 1;
                MainWindow.MaxTimeGraph += 1;
                MoveAxisTimeIncreased(tmin, tmax); 
            }
            else 
            {
                MovePointsTimeIncreased(value, tmin, tmax); 
            }
        }

        private void MoveAxisTimeIncreased(double tmin, double tmax)
        {
            try
            {
                MoveAllLinesLeft(tmin, tmax); 
                MovePointsLeft(tmin, tmax); 
                RemoveLinesOutOfRange(); 
                
                MainWindow.DrawCoordinates(); 
                MainWindow.DrawLine(SetpointLines); 
                MainWindow.DrawLine(ProcessVarLines); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception: {e}", "Exception");
            }
        }

        private void MoveAllLinesLeft(double tmin, double tmax)
        {
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
        }

        private void MovePointsLeft(double tmin, double tmax)
        {
            this.SetpointLeft -= MainWindow.GraphWidth / (tmax - tmin); 
            this.ProcessVariableLeft -= MainWindow.GraphWidth / (tmax - tmin); 
        }

        private void RemoveLinesOutOfRange()
        {
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
        }

        private void MovePointsTimeIncreased(double value, double tmin, double tmax)
        {
            try
            {
                Line lineSp = MoveSpTimeIncreased(value, tmin, tmax); 
                Line linePv = MovePvTimeIncreased(value, tmin, tmax); 

                SetpointLines.Add(lineSp); 
                ProcessVarLines.Add(linePv); 
                MainWindow.DrawLine(lineSp); 
                MainWindow.DrawLine(linePv); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception: {e}", "Exception");
            }
        }

        private Line MoveSpTimeIncreased(double value, double tmin, double tmax)
        {
            Line lineSp = new Line(); 
            lineSp.Stroke = System.Windows.Media.Brushes.Red;
            lineSp.StrokeThickness = 1.5;

            lineSp.X1 = this.SetpointLeft; 
            lineSp.Y1 = this.SetpointTop + 2.5; 
            this.SetpointLeft = ((value - tmin) * MainWindow.GraphWidth / (tmax - tmin)) - 2.5; 
            lineSp.X2 = this.SetpointLeft; 
            lineSp.Y2 = this.SetpointTop + 2.5; 
            
            return lineSp; 
        }

        private Line MovePvTimeIncreased(double value, double tmin, double tmax)
        {
            // A line of PV that needs to be added to the canvas 
            Line linePv = new Line(); 
            linePv.Stroke = System.Windows.Media.Brushes.Blue;
            linePv.StrokeThickness = 1.5;

            linePv.X1 = this.ProcessVariableLeft; 
            linePv.Y1 = this.ProcessVariableTop + 2.5; 
            this.ProcessVariableLeft = ((value - tmin) * MainWindow.GraphWidth / (tmax - tmin)) - 2.5; 
            linePv.X2 = this.ProcessVariableLeft; 
            linePv.Y2 = this.ProcessVariableTop + 2.5; 

            return linePv; 
        }
        #endregion  // Time methods 
    }
}