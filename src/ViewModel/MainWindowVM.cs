namespace PidControllerWpf.ViewModel
{
    public class MainWindowVM
    {
        private PidVM pidVM;
        public PidVM PidVM
        {
            get { return pidVM; }
        }

        private TextBlockVM textBlockVM;
        public TextBlockVM TextBlockVM
        {
            get { return textBlockVM; }
        }

        private GraphCanvasVM graphCanvasVM;
        public GraphCanvasVM GraphCanvasVM
        {
            get { return graphCanvasVM; }
        }

        public MainWindowVM()
        {
            this.textBlockVM = new TextBlockVM(); 
            this.graphCanvasVM = new GraphCanvasVM();
            this.pidVM = new PidVM(ref textBlockVM, ref graphCanvasVM); 
        }
    }
}