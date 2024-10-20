using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
			LoadChunk();
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
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

		private void LoadChunk()
		{
			ChunkCanvas.ClearElement();

			for (uint i = 0; i < 64 * 64; i++)
			{
				ChunkCanvas.AddElement(SaveData.Instance().ReadNumber(0x24C7C1 + i * 2, 2) != 0xFFFF);
			}
			ChunkCanvas.AddGuidElement();
		}
    }
}
