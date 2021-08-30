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

namespace STGDAT
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
			dlg.Filter = "STGDAT*.BIN|STGDAT*.BIN";
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

		private void MenuItemFileOtherMap_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().OtherMap(dlg.FileName);
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonMapGeneratorUp_Click(object sender, RoutedEventArgs e)
		{
			int index = ListBoxMap.SelectedIndex;
			if (index == -1) return;
			MapGenerator map = (DataContext as ViewModel)?.Map;
			if (map == null) return;
			map.Front(index);
		}

		private void ButtonMapGeneratorDown_Click(object sender, RoutedEventArgs e)
		{
			int index = ListBoxMap.SelectedIndex;
			if (index == -1) return;
			MapGenerator map = (DataContext as ViewModel)?.Map;
			if (map == null) return;
			map.Back(index);
		}

		private void ButtonMapGeneratorAppend_Click(object sender, RoutedEventArgs e)
		{
			MapGenerator map = (DataContext as ViewModel)?.Map;
			if (map == null) return;
			map.Append();
		}

		private void ButtonMapGeneratorRemove_Click(object sender, RoutedEventArgs e)
		{
			int index = ListBoxMap.SelectedIndex;
			if (index == -1) return;
			MapGenerator map = (DataContext as ViewModel)?.Map;
			if (map == null) return;
			map.RemoveAt(index);
		}

		private void ButtonMapGeneratorClear_Click(object sender, RoutedEventArgs e)
		{
			MapGenerator map = (DataContext as ViewModel)?.Map;
			if (map == null) return;
			map.Clear();
		}

		private void ButtonMapGeneratorExecution_Click(object sender, RoutedEventArgs e)
		{
			MapGenerator map = (DataContext as ViewModel)?.Map;
			if (map == null) return;
			map.Execution();
		}
	}
}
