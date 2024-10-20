using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
			SaveChunk();
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

		private void SaveChunk()
		{
			// There are two possible methods:
			// [1] one is to generate an array after completion and fill it with contents
			// [2] the other is to extend or reduce the current array

			var chunkFlags = ChunkCanvas.GetFlags();
			var flagCount = chunkFlags.Count(flag => flag == true);
			if (flagCount == 0) return;
			// original map's chunk
			var chunkSize = (SaveData.Instance().Length() + 0x110 - 0x183FEF0) / 0x30000;
			if (flagCount > chunkSize) return;

			var count = 0u;
			for (uint i = 0; i < 64 * 64; i++)
			{
				var chunkindex = SaveData.Instance().ReadNumber(0x24C7C1 + i * 2, 2);
				var flag = chunkFlags[(int)i];
				var address = 0x183FEF0 + count * 0x30000;
				if (chunkindex == 0xFFFF && flag == true)
				{
					SaveData.Instance().Extension(address, 0x30000);
					// append bedrock
					for (uint j = 0; j < 32 * 32; j++)
					{
						SaveData.Instance().WriteNumber(address + j * 2, 2, 1);
					}
				}
				else if (chunkindex != 0xFFFF && flag == false)
				{
					SaveData.Instance().Extension(address, 0x30000);

					// TBD.
					// Entities in chunks remain
				}

				SaveData.Instance().WriteNumber(0x24C7C1 + i * 2, 2, flag ? count : 0xFFFF);
				if (flag == true)
				{
					count++;
				}
			}

			SaveData.Instance().WriteNumber(0x24E7C5, 2, count);
			SaveData.Instance().Resize(0x183FEF0 + (uint)chunkSize * 0x30000 - 0x110);
		}
    }
}
