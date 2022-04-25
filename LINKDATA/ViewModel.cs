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
			IDXzrc.Create(dlg.FileName);
		}
	}
}
