using System.Diagnostics;
using System.Windows;

namespace Cheat_Launcher.Windows
{
    /// <summary>
    /// Interaction logic for ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        public Process SelectedProcess { get; private set; }

        public ProcessWindow(Process[] processes)
        {
            InitializeComponent();

            ProcessListBox.ItemsSource = processes;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedProcess = ProcessListBox.SelectedItem as Process;

            if (SelectedProcess != null)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите процесс.");
            }
        }
    }
}
