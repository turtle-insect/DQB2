using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SCSHDAT
{
	internal class ViewModel
	{
		public ICommand OpenFileCommand { get; private set; }
		public ICommand SaveFileCommand { get; private set; }
		public ICommand ImportFileCommand { get; private set; }
		public ICommand ExportFileCommand { get; private set; }
		public ICommand ImportPhotoCommand { get; private set; }
		public ICommand ExportPhotoCommand { get; private set; }

		public ObservableCollection<Photo> Photos { get; private set; } = new ObservableCollection<Photo>();

		public ViewModel()
		{
			OpenFileCommand = new CommandAction(OpenFile);
			SaveFileCommand = new CommandAction(SaveFile);
			ImportFileCommand = new CommandAction(ImportFile);
			ExportFileCommand = new CommandAction(ExportFile);
			ImportPhotoCommand = new CommandAction(ImportPhoto);
			ExportPhotoCommand = new CommandAction(ExportPhoto);
		}

		private void Initialize()
		{
			Photos.Clear();
			for (uint i = 0; i < 100; i++)
			{
				Photo photo = new Photo(0x69E90 + 409600 * i, 409600);
				if (photo.Image == null) continue;
				Photos.Add(photo);
			}
		}

		private void OpenFile(object? parameter)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "SCSHDAT.BIN|SCSHDAT.BIN";
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Open(dlg.FileName);
			Initialize();
		}

		private void SaveFile(object? parameter)
		{
			SaveData.Instance().Save();
		}

		private void ImportFile(object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Import(dlg.FileName);
		}

		private void ExportFile(object? parameter)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Export(dlg.FileName);
		}

		private void ImportPhoto(object? parameter)
		{
			Photo? photo = parameter as Photo;
			if (photo == null) return;

			var dlg = new OpenFileDialog();
			dlg.Filter = "jpg|*.jpg";
			if (dlg.ShowDialog() == false) return;
			photo.Import(dlg.FileName);
		}

		private void ExportPhoto(object? parameter)
		{
			Photo? photo = parameter as Photo;
			if (photo == null) return;

			var dlg = new SaveFileDialog();
			dlg.Filter = "jpg|*.jpg";
			if (dlg.ShowDialog() == false) return;
			photo.Export(dlg.FileName);
		}
	}
}
