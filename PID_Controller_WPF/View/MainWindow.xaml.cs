using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PID_Controller_WPF.ViewModel;

namespace PID_Controller_WPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members 
        /// <summary>
        /// Instance of a canvas named `GraphCanvas` that allows to 
        /// use this canvas from static methods
        /// </summary>
        public static Canvas _GraphCanvas = null; 
        /// <summary>
        /// Instance of a canvas named `ProcessVariableCanvas` that allows to 
        /// use this canvas from static methods
        /// </summary>
        public static Canvas _ProcessVariableCanvas = null; 
        /// <summary>
        /// Instance of a canvas named `TimeValuesCanvas` that allows to 
        /// use this canvas from static methods
        /// </summary>
        public static Canvas _TimeValuesCanvas = null; 
        #endregion  // Members 

        #region Properties
        /// <summary>
        /// Minimum value of a process variable at the graph 
        /// </summary>
        public static double MinPvGraph { get; set; } = 0.0; 
        /// <summary>
        /// Maximum value of a process variable at the graph 
        /// </summary>
        public static double MaxPvGraph { get; set; } = 50.0; 
        /// <summary>
        /// Number of lines along process variable axis at the graph 
        /// </summary>
        public static int NumPvGraph { get; set; } = 10; 
        /// <summary>
        /// Minimum value of a time values at the graph
        /// </summary>
        public static double MinTimeGraph { get; set; } = 0.0; 
        /// <summary>
        /// Maximum value of a time values at the graph
        /// </summary>
        public static double MaxTimeGraph { get; set; } = 60.0; 
        /// <summary>
        /// Number of lines along time axis at the graph 
        /// </summary>
        public static int NumTimeGraph { get; set; } = 10; 
        /// <summary>
        /// Width of a graph
        /// </summary>
        public static double GraphWidth { get; set; } = 0.0; 
        /// <summary>
        /// Width of a graph
        /// </summary>
        public static double GraphHeight { get; set; } = 0.0; 
        #endregion  // Properties

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            _GraphCanvas = GraphCanvas; 
            _ProcessVariableCanvas = ProcessVariableCanvas; 
            _TimeValuesCanvas = TimeValuesCanvas; 

            // When canvas is loaded, get size of Graph using lambda expression.
            Loaded += (o, e) => 
            {
                // Get actual sizes of the canvas for a graph.
                GraphWidth = GraphCanvas.ActualWidth; 
                GraphHeight = GraphCanvas.ActualHeight;

                // Pass width and height of a canvas to the GraphCanvasViewModel 
                ((MainWindowViewModel)(this.DataContext)).GraphCanvasViewModel.Setpoint = 0; 

                // Draw coordinates 
                DrawCoordinates();
                
                // Set labels for each axis of the graph
                Canvas.SetTop(ValueLabel, GraphHeight/2 - 12.5);
                Canvas.SetLeft(ValueLabel, 0);
                Canvas.SetTop(TimeLabel, (float)TimeValuesCanvas.ActualHeight - 32.5);
                Canvas.SetLeft(TimeLabel, GraphWidth / 2 - 17.5);
            }; 
        }
        #endregion  // Constructor
        
        #region Methods
        /// <summary>
        /// Draws coordinates and grid for a graph
        /// </summary>
        public static void DrawCoordinates()
        {
            // Remove all elements from the canvas except SetpointEllipse
            List<UIElement> itemstoremove = new List<UIElement>();
            foreach (UIElement ui in _GraphCanvas.Children)
            {
                if (!ui.Uid.StartsWith("SetpointEllipse"))
                {
                    itemstoremove.Add(ui);
                }
            }
            foreach (UIElement ui in itemstoremove)
            {
                _GraphCanvas.Children.Remove(ui);
            }
            _ProcessVariableCanvas.Children.Clear();
            _TimeValuesCanvas.Children.Clear();

            // Add horizontal lines and their labels to the canvas
            for (int i = 0; i < NumPvGraph - 1; i++)
            {
                // Horizontal line
                Line xAxis = new Line(); 
                xAxis.X1 = 0; 
                xAxis.X2 = GraphWidth; 
                xAxis.Y1 = (GraphHeight / NumPvGraph) + (i * GraphHeight / NumPvGraph);
                xAxis.Y2 = xAxis.Y1; 
                xAxis.Stroke = System.Windows.Media.Brushes.Black; 
                xAxis.StrokeThickness = 0.5;
                _GraphCanvas.Children.Add(xAxis);
                
                // Label for horizontal line 
                Label xLabel = new Label();
                xLabel.Content = $"{(MaxPvGraph - (MaxPvGraph - MinPvGraph) / NumPvGraph) - (i * (MaxPvGraph - MinPvGraph) / NumPvGraph)}"; 
                xLabel.Height = 25; 
                xLabel.Width = 50; 
                Canvas.SetTop(xLabel, xAxis.Y1 - 12.5);
                Canvas.SetLeft(xLabel, 25);
                _ProcessVariableCanvas.Children.Add(xLabel); 
            }

            // Add vertical lines and their labels to the canvas
            for (int i = 0; i < NumTimeGraph; i++)
            {
                // Vertical line
                Line yAxis = new Line(); 
                yAxis.X1 = (GraphWidth / NumTimeGraph) + (i * GraphWidth / NumTimeGraph);
                yAxis.X2 = yAxis.X1;
                yAxis.Y1 = 0;
                yAxis.Y2 = GraphHeight;
                yAxis.Stroke = System.Windows.Media.Brushes.Black; 
                yAxis.StrokeThickness = 0.5;
                _GraphCanvas.Children.Add(yAxis);

                // Label for vertical line 
                Label yLabel = new Label();
                yLabel.Content = $"{(MinTimeGraph + (MaxTimeGraph - MinTimeGraph) / NumTimeGraph) + (i * (MaxTimeGraph - MinTimeGraph) / NumTimeGraph)}"; 
                yLabel.Height = 25; 
                yLabel.Width = 50; 
                Canvas.SetTop(yLabel, 0);
                Canvas.SetLeft(yLabel, yAxis.X1 - 10);
                _TimeValuesCanvas.Children.Add(yLabel); 
            }
        }

        /// <summary>
        /// Draws each line of lines array for a graph 
        /// </summary>
        public static void DrawLine(List<Line> lines)
        {
            foreach (var line in lines)
            {
                _GraphCanvas.Children.Add(line);
            }
        }

        /// <summary>
        /// Draws a line for a graph 
        /// </summary>
        public static void DrawLine(Line line)
        {
            _GraphCanvas.Children.Add(line);
        }
        #endregion  // Methods
    }
}
