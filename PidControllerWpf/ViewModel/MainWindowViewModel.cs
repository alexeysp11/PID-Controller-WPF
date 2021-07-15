namespace PidControllerWpf.ViewModel
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

        private GraphCanvasVM _GraphCanvasVM;
        public GraphCanvasVM GraphCanvasVM
        {
            get { return _GraphCanvasVM; }
        }
        #endregion  // ViewModels

        #region Constructor
        public MainWindowViewModel()
        {
            // Initialize ViewModels
            this._TextBlockViewModel = new TextBlockViewModel(); 
            this._GraphCanvasVM = new GraphCanvasVM();
            this._PidViewModel = new PidViewModel(ref _TextBlockViewModel, ref _GraphCanvasVM); 
        }
        #endregion  // Constructor
    }
}