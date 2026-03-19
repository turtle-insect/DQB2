using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
			OpenFile();
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "STGDAT*.BIN|STGDAT*.BIN";
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Open(dlg.FileName);
			OpenFile();
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

		private void OpenFile()
		{
			DataContext = new ViewModel();
			LoadChunk();
			LoadWorldMap();
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

		private void ButtonChunkUpdate_Click(object sender, RoutedEventArgs e)
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
			for (uint i = 0; i < chunkFlags.Count; i++)
			{
				var chunkindex = SaveData.Instance().ReadNumber(0x24C7C1 + i * 2, 2);
				var flag = chunkFlags[(int)i];
				var address = Util.CalcChunkAddress(count);
				if (chunkindex == 0xFFFF && flag == true)
				{
					SaveData.Instance().Extension(address, 0x30000);
					// append bedrock
					for (uint xz = 0; xz < 32 * 32; xz++)
					{
						SaveData.Instance().WriteNumber(address + xz * 2, 2, 1);
					}
				}
				else if (chunkindex != 0xFFFF && flag == false)
				{
					SaveData.Instance().Reducion(address, 0x30000);

					// ？？？？
					// format Entity Block？
				}

				SaveData.Instance().WriteNumber(0x24C7C1 + i * 2, 2, flag ? count : 0xFFFF);
				if (flag == true)
				{
					count++;
				}
			}

			SaveData.Instance().WriteNumber(0x24E7C5, 2, count);
			SaveData.Instance().Resize(Util.CalcChunkAddress((uint)chunkSize) - 0x110);
		}

		private void LoadWorldMap()
		{
			WorldModel.Children.Clear();

			var commonMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.ForestGreen));

			var maxX = (int)SaveData.Instance().ReadNumber(0x24E7C9, 2) / 32;
			var maxZ = (int)SaveData.Instance().ReadNumber(0x24E7CB, 2) / 32;

			var halfX = maxX / 2;
			var halfZ = maxZ / 2;

			var chunkMesh = new ChunkMesh();
			for (int z = 0; z < maxZ; z++)
			{
				for (int x = 0; x < maxX; x++)
				{
					var address = 0x24C7C1 + (z * maxX + x) * 2;
					var chunkID = SaveData.Instance().ReadNumber((uint)address, 2);
					if (chunkID == 0xFFFF) continue;

					var mesh = chunkMesh.Build(chunkID, (x - halfX) * 32, (z - halfZ) * 32);
					GeometryModel3D model = new GeometryModel3D
					{
						Geometry = mesh,
						Material = commonMaterial
					};

					WorldModel.Children.Add(new ModelVisual3D { Content = model });
				}
			}
		}
	}
}
