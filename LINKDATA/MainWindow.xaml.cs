using Microsoft.Win32;
using System.Windows;

namespace LINKDATA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonSelectOriginalFile_Click(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ViewModel;
			if (vm == null) return;

			var dlg = new OpenFileDialog();
			dlg.Filter = "LINKDATA|LINKDATA.IDX;LINKDATA_PATCH.IDX";
			if (dlg.ShowDialog() == false) return;

			vm.OriginalFile = dlg.FileName;
		}

		private void ButtonDecryption_Click(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ViewModel;
			if (vm == null) return;
			vm.Decryption();
		}
	}
}
