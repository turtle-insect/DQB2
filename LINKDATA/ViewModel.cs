using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace LINKDATA
{
	internal class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		
		public ObservableCollection<IDX> IDXs { get; private set; } = new ObservableCollection<IDX>();
		public IDXzrc IDXzrc { get; private set; } = new IDXzrc();
		public CommandAction CommandOpenIDXFile { get; private set; }
		public CommandAction CommandOpenIDXzrcFile { get; private set; }
		public CommandAction CommandOpenUnPackIDXzrcFile { get; private set; }
		public CommandActionParam CommandExportIDX { get; private set; }
		public CommandActionParam CommandImportIDX { get; private set; }
		public CommandActionParam CommandUnPackIDXzrc { get; private set; }
		public CommandAction CommandPackIDXzrc { get; private set; }
		public Int32 PackSplitSize { get; set; } = 0x200000;
		public String IDXIndexFilter
		{
			get => mIDXIndexFilter;
			set
			{
				mIDXIndexFilter = value;
				FilterIdxList();
			}
		}

		private List<IDX> mIDXs = new List<IDX>();
		private String mIDXIndexFilter = "";
		private String mLinkDataPath = "";
		private String mIDXzrcPath = "";
		private String mUnPackIDXzrcPath = "";
		private String mItemResoucePath = "";
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
			CommandOpenIDXFile = new CommandAction(OpenIDXFile);
			CommandOpenIDXzrcFile = new CommandAction(OpenIDXzrcFile);
			CommandOpenUnPackIDXzrcFile = new CommandAction(OpenUnPackIDXzrcFile);
			CommandExportIDX = new CommandActionParam(ExportIDX);
			CommandImportIDX = new CommandActionParam(ImportIDX);
			CommandUnPackIDXzrc = new CommandActionParam(UnPackIDXzrc);
			CommandPackIDXzrc = new CommandAction(PackIDXzrc);
		}

		private void OpenIDXFile()
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
			for (int index = 0; index < buffer.Length / 32; index++)
			{
				var idx = new IDX(index);
				idx.Offset = BitConverter.ToUInt64(buffer, index * 32 + 0);
				idx.UncompressedSize = BitConverter.ToUInt64(buffer, index * 32 + 8);
				idx.CompressedSize = BitConverter.ToUInt64(buffer, index * 32 + 16);
				idx.IsCompressed = BitConverter.ToUInt64(buffer, index * 32 + 24);
				mIDXs.Add(idx);
			}

			FilterIdxList();
		}

		private void FilterIdxList()
		{
			IDXs.Clear();
			foreach (IDX idx in mIDXs)
			{
				if (String.IsNullOrEmpty(IDXIndexFilter) || idx.Index.ToString().IndexOf(IDXIndexFilter) != -1)
				{
					IDXs.Add(idx);
				}
			}
		}

		private void OpenIDXzrcFile()
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "idxzrc|*.idxzrc";
			if (dlg.ShowDialog() == false) return;

			IDXzrcPath = dlg.FileName;
			IDXzrc.Read(dlg.FileName);
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
			int size = (int)idx.UncompressedSize;
			dlg.FileName = idx.Index.ToString("00000");
			if (idx.IsCompressed == 0)
			{
				dlg.Filter = "idxout|*.idxout";
			}
			else
			{
				dlg.Filter = "idxzrc|*.idxzrc";
				size = (int)idx.CompressedSize;
			}
			
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
			dlg.Filter = "idxzrc|*.idxzrc";
			if (dlg.ShowDialog() == false) return;
			IDXzrc idxzrc = new IDXzrc();
			idxzrc.Read(dlg.FileName);
			if (idxzrc.UncompressedSize == 0) return;

			Byte[] importFile = System.IO.File.ReadAllBytes(dlg.FileName);
			Byte[] link_idx = System.IO.File.ReadAllBytes(mLinkDataPath);
			Byte[] link_bin = System.IO.File.ReadAllBytes(path);

			// offset = after - original
			// original file
			int offset = ((int)idx.CompressedSize + 0x80) / 0x100 * 0x100;
			// after file
			offset = (importFile.Length + 0x80) / 0x100 * 0x100 - offset;
			
			
			Byte[] bin = new Byte[link_bin.Length + offset];
			Array.Copy(link_bin, 0, bin, 0, (int)IDXs[idx.Index].Offset);

			Array.Fill<Byte>(link_idx, 0, idx.Index * 32 + 8, 24);
			Array.Copy(BitConverter.GetBytes(idxzrc.UncompressedSize), 0, link_idx, idx.Index * 32 + 8, 4);
			Array.Copy(BitConverter.GetBytes(importFile.Length), 0, link_idx, idx.Index * 32 + 16, 4);
			Array.Copy(BitConverter.GetBytes(1), 0, link_idx, idx.Index * 32 + 24, 4);
			Array.Copy(importFile, 0, bin, (int)IDXs[idx.Index].Offset, importFile.Length);

			for (int index = idx.Index + 1; index < IDXs.Count; index++)
			{
				int address = (int)IDXs[index].Offset + offset;
				Array.Copy(BitConverter.GetBytes((UInt64)address), 0, link_idx, index * 32, 8);
				Array.Copy(link_bin, (int)IDXs[index].Offset, bin, address, (int)IDXs[index].CompressedSize);
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
			dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(path));
			dlg.Filter = "unpack|*.unpack";

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

		private void OpenUnPackIDXzrcFile()
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "builder file|*.unpack;*.g1t;*.g1m;*.g2m";
			if (dlg.ShowDialog() == false) return;

			UnPackIDXzrcPath = dlg.FileName;
		}

		private void PackIDXzrc()
		{
			String path = mUnPackIDXzrcPath;
			if (!System.IO.File.Exists(path)) return;

			Byte[] Buffer = System.IO.File.ReadAllBytes(path);
			Int32 packCount = (Buffer.Length + PackSplitSize - 1) / PackSplitSize;

			List<Byte> Packed = new List<Byte>();
			// split size.
			foreach (Byte b in BitConverter.GetBytes(PackSplitSize))
			{
				Packed.Add(b);
			}
			// split count.
			foreach (Byte b in BitConverter.GetBytes(packCount))
			{
				Packed.Add(b);
			}
			// unpack file size.
			foreach (Byte b in BitConverter.GetBytes(Buffer.Length))
			{
				Packed.Add(b);
			}

			// chunk size.
			// reserve.
			for (int index = 0; index < packCount * 4; index++)
			{
				Packed.Add(0);
			}

			// padding.
			int count = 0x80 - (Packed.Count % 0x80);
			if(count == 0x80)count = 0;
			for (int index = 0; index < count; index++)
			{
				Packed.Add(0);
			}

			// data
			for (int pack = 0; pack < packCount; pack++)
			{
				int length = PackSplitSize;
				if (pack + 1 == packCount) length = Buffer.Length % PackSplitSize;
				Byte[] tmp = new Byte[length];
				Array.Copy(Buffer, PackSplitSize * pack, tmp, 0, tmp.Length);
				tmp = Comp(tmp);

				int index = 0;
				int size = tmp.Length;
				foreach (Byte b in BitConverter.GetBytes(size + 4))
				{
					Packed[0x0C + pack * 4 + index] = b;
					index++;
				}
				foreach (Byte b in BitConverter.GetBytes(size))
				{
					Packed.Add(b);
				}
				foreach (Byte b in tmp)
				{
					Packed.Add(b);
				}

				// padding.
				count = 0x80 - (Packed.Count % 0x80);
				if (count == 0x80) count = 0;
				for (index = 0; index < count; index++)
				{
					Packed.Add(0);
				}
			}

			var dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.Filter = "idxzrc|*.idxzrc";
			dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(path));
			if (dlg.ShowDialog() == false) return;
			System.IO.File.WriteAllBytes(dlg.FileName, Packed.ToArray());
		}

		private Byte[] Comp(Byte[] data)
		{
			Byte[] result = null;
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
			Byte[] result = null;
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
