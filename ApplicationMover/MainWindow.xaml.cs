using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;
using Forms = System.Windows.Forms;

namespace ApplicationMover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIElement[] _elementsPreProcess;

        private delegate void PathAction();

        private readonly AppMoverInfo _appMoverInfo = new AppMoverInfo();

        public MainWindow()
        {
            InitializeComponent();
            InitElements();
        }

        private void InitElements()
        {
            _elementsPreProcess = new UIElement[]
            {
                TxtSourceFolder, TxtDestinationFolder, BtnSelectSource, BtnSelectDestination
            };
        }

        private void BtnSelectSource_OnClick(object sender, RoutedEventArgs e)
        {
            var path = PickDirectoryPath();
            AssignSourceDirectory(path);
        }

        private void BtnSelectDestination_OnClick(object sender, RoutedEventArgs e)
        {
            var path = PickDirectoryPath();
            AssignTargetDirectory(path);
        }

        private void TxtSourceFolder_OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var textBox = (TextBox)sender;
            var text = textBox.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                AssignSourceDirectory(text);
            }
        }

        private void TxtDestinationFolder_OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var textBox = (TextBox)sender;
            var text = textBox.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                AssignTargetDirectory(text);
            }
        }

        private void BtnProcess_OnClick(object sender, RoutedEventArgs e)
        {
            if (TryEnableProcess())
            {
                StartProcessing();
            }
        }

        private async void StartProcessing()
        {
            ChangePreProcessElements(enable: false);
            await DoProcessingAsync();
            ChangePreProcessElements(enable: true);
            PbMain.Value = 100;
        }

        private void DoProcessing()
        {
            IAppMover appMover = new MkLinkAppMover();
            appMover.MoveApplication(_appMoverInfo);
        }

        private Task DoProcessingAsync()
        {
            return Task.Run(new Action(DoProcessing));
        }

        private void AssignSourceDirectory(string path)
        {
            AssignDirectory(() => _appMoverInfo.SourceDirectory = path,
                () => TxtSourceFolder.Text = path);
        }

        private void AssignTargetDirectory(string path)
        {
            AssignDirectory(() => _appMoverInfo.TargetDirectory = path,
                () => TxtDestinationFolder.Text = path);
        }

        private void AssignDirectory(PathAction actionAssignInfo, PathAction actionAssignTextBox)
        {
            try
            {
                actionAssignInfo();
                actionAssignTextBox();
                TryEnableProcess();
            }
            catch (UserException e)
            {
                ShowErrorMessage(e.Message, "Directory error");
            }
        }

        private void ChangePreProcessElements(bool enable)
        {
            _elementsPreProcess.ToList().ForEach(x => x.IsEnabled = enable);
        }

        private bool TryEnableProcess()
        {
            return BtnProcess.IsEnabled = _appMoverInfo.IsValid();
        }

        private void ShowErrorMessage(string message, string caption = "")
        {
            MessageBox.Show(this, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static string PickDirectoryPath()
        {
            string path = null;

            var dialog = new Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }

            return path;
        }
    }
}
