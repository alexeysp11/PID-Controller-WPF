namespace PidControllerWpf.ViewModel
{
    public class MainWindowVM
    {
        private PidVM _PidVM;
        public PidVM PidVM
        {
            get { return _PidVM; }
        }

        private TextBlockVM _TextBlockVM;
        public TextBlockVM TextBlockVM
        {
            get { return _TextBlockVM; }
        }

        private GraphCanvasVM _GraphCanvasVM;
        public GraphCanvasVM GraphCanvasVM
        {
            get { return _GraphCanvasVM; }
        }

        public MainWindowVM()
        {
            this._TextBlockVM = new TextBlockVM(); 
            this._GraphCanvasVM = new GraphCanvasVM();
            this._PidVM = new PidVM(ref _TextBlockVM, ref _GraphCanvasVM); 
        }
    }
}