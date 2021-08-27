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

namespace CMNDAT
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

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			String[] files = e.Data.GetData(DataFormats.FileDrop) as String[];
			if (files == null) return;

			SaveData.Instance().Open(files[0]);
			DataContext = new ViewModel();
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "CMNDAT.BIN|CMNDAT.BIN";
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

		private void ButtonItemChoice_Click(object sender, RoutedEventArgs e)
		{
			Item item = (sender as Button)?.DataContext as Item;
			if (item == null) return;

			var window = new ChoiceWindow();
			window.ID = item.ID;
			window.ShowDialog();
			item.ID = window.ID;

			item.Count = item.ID == 0 ? 0 : 1u;
		}

		private void ButtonBluePrintBackup_Click(object sender, RoutedEventArgs e)
		{
			BluePrint item = (sender as Button)?.DataContext as BluePrint;
			if (item == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			System.IO.File.WriteAllBytes(dlg.FileName, SaveData.Instance().ReadValue(item.Address, 0x30008));
		}

		private void ButtonBluePrintLoad_Click(object sender, RoutedEventArgs e)
		{
			BluePrint item = (sender as Button)?.DataContext as BluePrint;
			if (item == null) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			Byte[] buf = System.IO.File.ReadAllBytes(dlg.FileName);
			if (buf.Length != 0x30008) return;
			SaveData.Instance().WriteValue(item.Address, buf);

			item.X = buf[0x30000];
			item.Y = buf[0x30002];
			item.Z = buf[0x30004];
		}
	}
}
