using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CMNDAT
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

		private void Window_Closed(object sender, EventArgs e)
		{
			Properties.Settings.Default.Save();
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
			dlg.InitialDirectory = Properties.Settings.Default.FileOpenPath;
			dlg.Filter = "CMNDAT.BIN|CMNDAT.BIN";
			if (dlg.ShowDialog() == false) return;

			Properties.Settings.Default.FileOpenPath = System.IO.Path.GetDirectoryName(dlg.FileName);
			SaveData.Instance().Open(dlg.FileName);
			DataContext = new ViewModel();
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonSceneriesAllCheck_Click(object sender, RoutedEventArgs e)
		{
			SceneriesCheck(true);
		}

		private void ButtonSceneriesAllUnCheck_Click(object sender, RoutedEventArgs e)
		{
			SceneriesCheck(false);
		}

		private void TextBoxCraftItem_TextChanged(object sender, TextChangedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			vm.CreateCraft();
		}

		private void SceneriesCheck(bool isCheck)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			foreach (var item in vm.Sceneries)
			{
				item.Visit = isCheck;
			}
		}
	}
}
