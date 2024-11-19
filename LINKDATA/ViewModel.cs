using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace LINKDATA
{
	internal class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		
		public ObservableCollection<IDX> IDXs { get; init; } = new ObservableCollection<IDX>();
		public IDXzrc IDXzrc { get; init; } = new IDXzrc();
		public ICommand CommandOpenWorkPath { get; init; }
		public ICommand CommandOpenIDXFile { get; init; }
		public ICommand CommandOpenIDXzrcFile { get; init; }
		public ICommand CommandOpenUnPackIDXzrcFile { get; init; }
		public ICommand CommandExportIDX { get; init; }
		public ICommand CommandImportIDX { get; init; }
		public ICommand CommandUnPackIDXzrc { get; init; }
		public ICommand CommandPackIDXzrc { get; init; }
		public Int32 PackSplitSize { get; set; } = 0x200000;

		public uint IDXSizeFilter
		{
			get => mIDXSizeFilter;
			set
			{
				mIDXSizeFilter = value;
				FilterIdxList();
			}
		}

		public String IDXIndexFilter
		{
			get => mIDXIndexFilter;
			set
			{
				mIDXIndexFilter = value;
				FilterIdxList();
			}
		}

		public uint IDXFilterType
		{
			get => mIDXFilterType;
			set
			{
				mIDXFilterType = value;
				FilterIdxList();
			}
		}

		private List<IDX> mIDXs = new List<IDX>();
		private String mWorkPath = "";
		private uint mIDXSizeFilter = 0;
		private String mIDXIndexFilter = "";
		private uint mIDXFilterType = 0;
		private String mLinkDataPath = "";
		private String mIDXzrcPath = "";
		private String mUnPackIDXzrcPath = "";
		private String mItemResoucePath = "";
		public String WorkPath
		{
			get => mWorkPath;
			set
			{
				mWorkPath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WorkPath)));
			}
		}

		public String LinkDataPath
		{
			get => mLinkDataPath;
			set
			{
				mLinkDataPath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LinkDataPath)));
			}
		}
		public String IDXzrcPath
		{
			get => mIDXzrcPath;
			set
			{
				mIDXzrcPath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IDXzrcPath)));
			}
		}

		public String UnPackIDXzrcPath
		{
			get => mUnPackIDXzrcPath;
			set
			{
				mUnPackIDXzrcPath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnPackIDXzrcPath)));
			}
		}

		public String ItemResoucePath
		{
			get => mItemResoucePath;
			set
			{
				mItemResoucePath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemResoucePath)));
			}
		}

		public ViewModel()
		{
			mWorkPath = Environment.CurrentDirectory;
			CommandOpenWorkPath = new CommandAction(OpenWorkPath);
			CommandOpenIDXFile = new CommandAction(OpenIDXFile);
			CommandOpenIDXzrcFile = new CommandAction(OpenIDXzrcFile);
			CommandOpenUnPackIDXzrcFile = new CommandAction(OpenUnPackIDXzrcFile);
			CommandExportIDX = new CommandAction(ExportIDX);
			CommandImportIDX = new CommandAction(ImportIDX);
			CommandUnPackIDXzrc = new CommandAction(UnPackIDXzrc);
			CommandPackIDXzrc = new CommandAction(PackIDXzrc);
		}

		private void OpenWorkPath(object? param)
		{
			var dlg = new Microsoft.Win32.OpenFolderDialog();
			dlg.FolderName = WorkPath;
			if (dlg.ShowDialog() == false) return;

			WorkPath = dlg.FolderName;
		}

		private void OpenIDXFile(object? param)
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "LINKDATA|LINKDATA.IDX";
			if (dlg.ShowDialog() == false) return;

			LinkDataPath = dlg.FileName;
			CreateIdxList();
		}

		private void CreateIdxList()
		{
			mIDXs.Clear();
			String path = mLinkDataPath;
			if (!System.IO.File.Exists(path)) return;

			Byte[] buffer = System.IO.File.ReadAllBytes(path);

			int gameIndex = 0;
			for (int index = 0; index < buffer.Length / 32; index++)
			{
				var idx = new IDX(index);
				idx.Offset = BitConverter.ToUInt64(buffer, index * 32 + 0);
				idx.UncompressedSize = BitConverter.ToUInt64(buffer, index * 32 + 8);
				idx.CompressedSize = BitConverter.ToUInt64(buffer, index * 32 + 16);
				idx.IsCompressed = BitConverter.ToUInt64(buffer, index * 32 + 24);

				if (idx.UncompressedSize != 0)
				{
					idx.GameIndex = gameIndex;
					gameIndex++;
				}
				mIDXs.Add(idx);
			}

			FilterIdxList();
		}

		private void FilterIdxList()
		{
			IDXs.Clear();
			foreach (IDX idx in mIDXs)
			{
				if (idx.UncompressedSize < mIDXSizeFilter) continue;

				if (String.IsNullOrEmpty(IDXIndexFilter))
				{
					IDXs.Add(idx);
				}
				else
				{
					if(mIDXFilterType == 0)
					{
						// contain
						if (idx.Index.ToString().IndexOf(IDXIndexFilter) != -1) IDXs.Add(idx);
						else if (idx.GameIndex.ToString().IndexOf(IDXIndexFilter) != -1) IDXs.Add(idx);
					}
					else if (mIDXFilterType == 1)
					{
						// >=
						uint index;
						if (uint.TryParse(IDXIndexFilter, out index) == true)
						{
							if(idx.Index >= index) IDXs.Add(idx);
						}
					}
				}
			}
		}

		private void OpenIDXzrcFile(object? param)
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "idxzrc|*.idxzrc";
			dlg.InitialDirectory = WorkPath;
			if (dlg.ShowDialog() == false) return;

			IDXzrcPath = dlg.FileName;
			IDXzrc.Read(IDXzrcPath);
		}

		private void ExportIDX(object? param)
		{
			String path = mLinkDataPath;
			if (!System.IO.File.Exists(path)) return;
			path = path.Substring(0, path.Length - 3) + "BIN";
			if (!System.IO.File.Exists(path)) return;

			IDX? idx = param as IDX;
			if (idx == null) return;

			var dlg = new Microsoft.Win32.SaveFileDialog();
			uint size = (uint)idx.UncompressedSize;
			if (idx.IsCompressed == 0)
			{
				dlg.Filter = "unpack|*.unpack";
			}
			else
			{
				dlg.Filter = "idxzrc|*.idxzrc";
				size = (uint)idx.CompressedSize;
			}
			dlg.InitialDirectory = WorkPath;
			dlg.FileName = idx.Index.ToString("d5");

			if (dlg.ShowDialog() == false) return;

			Byte[] bin = System.IO.File.ReadAllBytes(path);
			Byte[] buffer = new Byte[size];
			Array.Copy(bin, (int)idx.Offset, buffer, 0, buffer.Length);
			System.IO.File.WriteAllBytes(dlg.FileName, buffer);
		}

		private void ImportIDX(object? param)
		{
			String path = mLinkDataPath;
			if (!System.IO.File.Exists(path)) return;
			path = path.Substring(0, path.Length - 3) + "BIN";
			if (!System.IO.File.Exists(path)) return;

			IDX? idx = param as IDX;
			if (idx == null) return;

			var dlg = new Microsoft.Win32.OpenFileDialog();
			if(idx.IsCompressed == 0)
			{
				dlg.Filter = "unpack|*.unpack";
			}
			else
			{
				dlg.Filter = "idxzrc|*.idxzrc";
			}
			dlg.InitialDirectory = WorkPath;
			if (dlg.ShowDialog() == false) return;

			uint size = 0;
			if (idx.IsCompressed == 0)
			{
				var info = new System.IO.FileInfo(dlg.FileName);
				size = (uint)info.Length;
			}
			else
			{
				IDXzrc idxzrc = new IDXzrc();
				idxzrc.Read(dlg.FileName);
				size = idxzrc.UncompressedSize;
			}

			Byte[] importFile = System.IO.File.ReadAllBytes(dlg.FileName);
			Byte[] link_idx = System.IO.File.ReadAllBytes(mLinkDataPath);
			Byte[] link_bin = System.IO.File.ReadAllBytes(path);

			// offset = after - original
			// original file
			int offset = ((int)idx.CompressedSize + 0x80) / 0x100 * 0x100;
			// after file
			offset = (importFile.Length + 0x80) / 0x100 * 0x100 - offset;
			
			
			Byte[] bin = new Byte[link_bin.Length + offset];
			Array.Copy(link_bin, 0, bin, 0, (int)idx.Offset);

			Array.Fill<Byte>(link_idx, 0, idx.Index * 32 + 8, 24);
			Array.Copy(BitConverter.GetBytes(size), 0, link_idx, idx.Index * 32 + 8, 4);
			Array.Copy(BitConverter.GetBytes(importFile.Length), 0, link_idx, idx.Index * 32 + 16, 4);
			Array.Copy(BitConverter.GetBytes(idx.IsCompressed), 0, link_idx, idx.Index * 32 + 24, 4);
			Array.Copy(importFile, 0, bin, (int)idx.Offset, importFile.Length);

			for (int index = idx.Index + 1; index < mIDXs.Count; index++)
			{
				int address = (int)mIDXs[index].Offset + offset;
				Array.Copy(BitConverter.GetBytes((UInt64)address), 0, link_idx, index * 32, 8);
				Array.Copy(link_bin, (int)mIDXs[index].Offset, bin, address, (int)mIDXs[index].CompressedSize);
			}

			System.IO.File.WriteAllBytes(mLinkDataPath, link_idx);
			System.IO.File.WriteAllBytes(path, bin);

			CreateIdxList();
		}

		private void UnPackIDXzrc(object? param)
		{
			IDXzrc? idxzrd = param as IDXzrc;
			if (idxzrd == null) return;
			if(idxzrd.UncompressedSize == 0) return;

			String path = mIDXzrcPath;
			if (!System.IO.File.Exists(path)) return;

			var dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.Filter = "unpack|*.unpack";
			dlg.InitialDirectory = WorkPath;
			dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(path));

			if (dlg.ShowDialog() == false) return;

			Byte[] bin = System.IO.File.ReadAllBytes(path);
			Byte[] buffer = new Byte[idxzrd.UncompressedSize];
			int index = 0;
			foreach (var chunk in idxzrd.Chunks)
			{
				int size = BitConverter.ToInt32(bin, (int)chunk.Offset);
				Byte[] tmp = new Byte[size];
				Array.Copy(bin, chunk.Offset + 4, tmp, 0, tmp.Length);
				tmp = Decomp(tmp);
				Array.Copy(tmp, 0, buffer, index, tmp.Length);
				index += tmp.Length;
			}
			System.IO.File.WriteAllBytes(dlg.FileName, buffer);
		}

		private void OpenUnPackIDXzrcFile(object? param)
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "builder file|*.unpack;*.g1t;*.g1m;*.g2m";
			dlg.InitialDirectory = WorkPath;
			if (dlg.ShowDialog() == false) return;

			UnPackIDXzrcPath = dlg.FileName;
		}

		private void PackIDXzrc(object? param)
		{
			String path = mUnPackIDXzrcPath;
			if (!System.IO.File.Exists(path)) return;

			Byte[] Buffer = System.IO.File.ReadAllBytes(path);
			Int32 packCount = (Buffer.Length + PackSplitSize - 1) / PackSplitSize;

			using var ms = new MemoryStream();
			// split size.
			ms.Write(BitConverter.GetBytes(PackSplitSize), 0, 4);
			// split count.
			ms.Write(BitConverter.GetBytes(packCount), 0, 4);
			// unpack file size.
			ms.Write(BitConverter.GetBytes(Buffer.Length), 0, 4);

			// chunk size.
			// reserve.
			// overwrite when writing data
			for (int index = 0; index < packCount * 4; index++)
			{
				ms.WriteByte(0);
			}

			// padding.
			int count = 0x80 - ((int)ms.Length % 0x80);
			if(count == 0x80)count = 0;
			for (int index = 0; index < count; index++)
			{
				ms.WriteByte(0);
			}

			// data
			for (int pack = 0; pack < packCount; pack++)
			{
				int length = PackSplitSize;
				if (pack + 1 == packCount) length = Buffer.Length % PackSplitSize;
				Byte[] tmp = new Byte[length];
				Array.Copy(Buffer, PackSplitSize * pack, tmp, 0, tmp.Length);
				tmp = Comp(tmp);

				// rewrite chunk size
				ms.Seek(0x0C + pack * 4, SeekOrigin.Begin);
				ms.Write(BitConverter.GetBytes(tmp.Length + 4), 0, 4);
				// chunk data
				// size & bytes
				ms.Seek(0, SeekOrigin.End);
				ms.Write(BitConverter.GetBytes(tmp.Length), 0, 4);
				ms.Write(tmp, 0, tmp.Length);

				// padding.
				count = 0x80 - ((int)ms.Length % 0x80);
				if (count == 0x80) count = 0;
				for (int index = 0; index < count; index++)
				{
					ms.WriteByte(0);
				}
			}

			var dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.Filter = "idxzrc|*.idxzrc";
			dlg.InitialDirectory = WorkPath;
			dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(path));
			if (dlg.ShowDialog() == false) return;
			System.IO.File.WriteAllBytes(dlg.FileName, ms.ToArray());
		}

		private Byte[] Comp(Byte[] data)
		{
			Byte[] result = [];
			using (var input = new MemoryStream(data))
			{
				using (var output = new MemoryStream())
				{
					using (var zlib = new System.IO.Compression.ZLibStream(output, System.IO.Compression.CompressionLevel.Fastest))
					{
						input.CopyTo(zlib);
					}
					result = output.ToArray();
				}
			}
			return result;
		}

		private Byte[] Decomp(Byte[] data)
		{
			Byte[] result = [];
			using (var input = new MemoryStream(data))
			{
				using (var zlib = new System.IO.Compression.ZLibStream(input, System.IO.Compression.CompressionMode.Decompress))
				{
					using (var output = new MemoryStream())
					{
						zlib.CopyTo(output);
						result = output.ToArray();
					}
				}
			}
			return result;
		}
	}
}
