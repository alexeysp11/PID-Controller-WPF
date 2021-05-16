namespace PID_Controller_WPF.ViewModel
{
    public class MainWindowViewModel
    {
        #region ViewModels
        private PidViewModel _PidViewModel;
        public PidViewModel PidViewModel
        {
            get { return _PidViewModel; }
        }

        private TextBlockViewModel _TextBlockViewModel;
        public TextBlockViewModel TextBlockViewModel
        {
            get { return _TextBlockViewModel; }
        }

        private GraphCanvasViewModel _GraphCanvasViewModel;
        public GraphCanvasViewModel GraphCanvasViewModel
        {
            get { return _GraphCanvasViewModel; }
        }
        #endregion  // ViewModels

        #region Constructor
        public MainWindowViewModel()
        {
            // Initialize ViewModels
            this._TextBlockViewModel = new TextBlockViewModel(); 
            this._GraphCanvasViewModel = new GraphCanvasViewModel();
            this._PidViewModel = new PidViewModel(ref _TextBlockViewModel, ref _GraphCanvasViewModel); 
        }
        #endregion  // Constructor
    }
}