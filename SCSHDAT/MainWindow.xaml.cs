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

namespace SCSHDAT
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "SCSHDAT.BIN|SCSHDAT.BIN";
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Open(dlg.FileName);
			DataContext = new ViewModel();
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileImport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Import(dlg.FileName);
		}

		private void MenuItemFileExport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Export(dlg.FileName);
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ListBoxItemMenuImport_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxPhoto.SelectedIndex < 0) return;

			Photo photo = (DataContext as ViewModel)?.Photos[ListBoxPhoto.SelectedIndex];
			var dlg = new OpenFileDialog();
			dlg.Filter = "jpg|*.jpg";
			if (dlg.ShowDialog() == false) return;
			photo.Import(dlg.FileName);
		}

		private void ListBoxItemMenuExport_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxPhoto.SelectedIndex < 0) return;

			Photo photo = (DataContext as ViewModel)?.Photos[ListBoxPhoto.SelectedIndex];
			var dlg = new SaveFileDialog();
			dlg.Filter = "jpg|*.jpg";
			if (dlg.ShowDialog() == false) return;
			photo.Export(dlg.FileName);
		}
	}
}
