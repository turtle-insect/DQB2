using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace SCSHDAT
{
	internal class Photo : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly uint mAddress;
		private readonly uint mSize;
		private Byte[]? mJpeg;
		private JpegBitmapDecoder? mDecoder;

		public Photo(uint address, uint size)
		{
			mAddress = address;
			mSize = size;
			mJpeg = SaveData.Instance().ReadValue(mAddress, mSize);
			if (!(mJpeg[0] == 0xFF && mJpeg[1] == 0xD8))
			{
				mJpeg = null;
				return;
			}

			int length = mJpeg.Length - 1;
			for (; length > 1; length--)
			{
				if (mJpeg[length] == 0xD9 && mJpeg[length - 1] == 0xFF) break;
			}
			Array.Resize(ref mJpeg, length + 1);
			CreateDecoder(mJpeg);
		}

		public void Import(String filename)
		{
			if (mJpeg == null) return;

			Byte[] tmp = System.IO.File.ReadAllBytes(filename);
			if (!(tmp[0] == 0xFF && tmp[1] == 0xD8)) return;
			if (tmp.Length > mSize) return;

			mJpeg = tmp;
			CreateDecoder(mJpeg);
			SaveData.Instance().Fill(mAddress, mSize, 0);
			SaveData.Instance().WriteValue(mAddress, mJpeg);

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
		}

		public void Export(String filename)
		{
			if (mJpeg == null) return;
			System.IO.File.WriteAllBytes(filename, mJpeg);
		}

		public System.Windows.Media.ImageSource? Image
		{
			get
			{
				if (mDecoder == null) return null;
				return mDecoder.Frames[0];
			}
		}

		private void CreateDecoder(Byte[] jpeg)
		{
			mDecoder = new JpegBitmapDecoder(new System.IO.MemoryStream(jpeg), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
		}
	}
}
