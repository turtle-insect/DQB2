using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace STGDAT
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

		private void MenuItemFileDLMap_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().DLMap(dlg.FileName);
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

		private void ButtonItemChoice_Click(object sender, RoutedEventArgs e)
		{
			Item item = (sender as Button)?.DataContext as Item;
			ItemChoice(item);
		}

		private void ButtonStorageUnActive_Click(object sender, RoutedEventArgs e)
		{
			Strage item = (sender as Button)?.DataContext as Strage;
			if (item == null) return;

			item.Clear();
		}

		private void ButtonAllStrageUnActive_Click(object sender, RoutedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			vm.AllStorageUnActive();
		}

		private void ButtonAllCrockeryUnActive_Click(object sender, RoutedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			vm.AllTablewareUnActive();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			vm.FilterPart();
		}

		private void ItemChoice(Item item)
		{
			if (item == null) return;

			var window = new ChoiceWindow();
			window.ID = item.ID;
			window.ShowDialog();
			item.ID = window.ID;

			item.Count = item.ID == 0 ? 0 : 1u;
		}
    }
}
