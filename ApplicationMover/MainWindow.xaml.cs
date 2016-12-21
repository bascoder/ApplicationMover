using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Forms = System.Windows.Forms;

namespace ApplicationMover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIElement[] _elementsPreProcess;
        private TextBox[] _txtPathHolders;

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

            _txtPathHolders = new TextBox[]
            {
                TxtSourceFolder, TxtDestinationFolder
            };
        }

        private void BtnSelectSource_OnClick(object sender, RoutedEventArgs e)
        {
            RetrieveDirectoryPath(TxtSourceFolder);
            CheckCanProcess();
        }

        private void BtnSelectDestination_OnClick(object sender, RoutedEventArgs e)
        {
            RetrieveDirectoryPath(TxtDestinationFolder);
            CheckCanProcess();
        }

        private void TxtSourceDestFolder_OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var textBox = (TextBox)sender;
            var text = textBox.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                AssignDirectoryPath(textBox, text);
            }

            CheckCanProcess();
        }

        private void BtnProcess_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckCanProcess())
            {
                DisablePreProcessElements();
                StartProcessing();
            }
        }

        private void StartProcessing()
        {

        }

        private void DisablePreProcessElements()
        {
            _elementsPreProcess.ToList().ForEach(x => x.IsEnabled = false);
        }

        private bool CheckCanProcess()
        {
            bool allValid = _txtPathHolders.All(x => IsValidDirectory(x.Text));

            // enable button if all input fields are valid
            return BtnProcess.IsEnabled = allValid;
        }

        private void ShowErrorMessage(string message, string caption = "")
        {
            MessageBox.Show(this, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RetrieveDirectoryPath(TextBox target)
        {
            var path = PickDirectoryPath();
            if (path != null)
            {
                AssignDirectoryPath(target, path);
            }
        }

        private void AssignDirectoryPath(TextBox target, string path)
        {
            if (IsValidDirectory(path))
            {
                target.Text = path;
            }
            else
            {
                ShowErrorMessage("Please provide an existing directory", "Invalid directory");
            }
        }

        private static bool IsValidDirectory(string path)
        {
            return Directory.Exists(path);
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
