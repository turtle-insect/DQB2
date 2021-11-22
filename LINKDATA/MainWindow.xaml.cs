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
using Microsoft.Win32;

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
