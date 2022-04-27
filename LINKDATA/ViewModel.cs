using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace LINKDATA
{
	internal class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public ObservableCollection<IDX> IDXs { get; private set; } = new ObservableCollection<IDX>();
		public IDXzrc IDXzrc { get; private set; } = new IDXzrc();
		public CommandAction CommandOpenIDXFile { get; private set; }
		public CommandAction CommandOpenIDXzrcFile { get; private set; }
		public CommandAction CommandIdx { get; private set; }
		public CommandActionParam CommandExportIDX { get; private set; }
		public CommandActionParam CommandUnPackIDXzrc { get; private set; }
		private String mLinkDataPath = "";
		private String mIDXzrcPath = "";
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

		public ViewModel()
		{
			CommandOpenIDXFile = new CommandAction(OpenIDXFile);
			CommandOpenIDXzrcFile = new CommandAction(OpenIDXzrcFile);
			CommandIdx = new CommandAction(CreateIdxList);
			CommandExportIDX = new CommandActionParam(ExportIDX);
			CommandUnPackIDXzrc = new CommandActionParam(UnPackIDXzrc);
		}

		private void OpenIDXFile()
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "LINKDATA|LINKDATA.IDX";
			if (dlg.ShowDialog() == false) return;

			LinkDataPath = dlg.FileName;
		}

		private void CreateIdxList()
		{
			IDXs.Clear();
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
				IDXs.Add(idx);
			}

			System.Windows.MessageBox.Show("Done");
		}

		private void OpenIDXzrcFile()
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();
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

		private void UnPackIDXzrc(object? param)
		{
			IDXzrc? idxzrd = param as IDXzrc;
			if (idxzrd == null) return;
			if(idxzrd.UncompressedSize == 0) return;

			var dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.Filter = "unpack|*.unpack";

			if (dlg.ShowDialog() == false) return;

			Byte[] bin = System.IO.File.ReadAllBytes(mIDXzrcPath);
			Byte[] buffer = new Byte[idxzrd.UncompressedSize];
			int index = 0;
			foreach (var chunk in idxzrd.Chunks)
			{
				int size = BitConverter.ToInt32(bin, (int)chunk.Offset);
				Byte[] tmp = new Byte[size];
				Array.Copy(bin, chunk.Offset + 4, tmp, 0, tmp.Length);
				tmp = Ionic.Zlib.ZlibStream.UncompressBuffer(tmp);
				Array.Copy(tmp, 0, buffer, index, tmp.Length);
				index += tmp.Length;
			}
			System.IO.File.WriteAllBytes(dlg.FileName, buffer);
		}
	}
}
