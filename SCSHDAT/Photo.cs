using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace SCSHDAT
{
	class Photo : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly uint mAddress;
		private Byte[] mJpeg;
		private JpegBitmapDecoder mDecoder;

		public Photo(uint address)
		{
			mAddress = address;
			mJpeg = SaveData.Instance().ReadValue(mAddress, Util.PHOTO_SIZE);
			if (!(mJpeg[0] == 0xFF && mJpeg[1] == 0xD8)) return;

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
			if (tmp.Length > Util.PHOTO_SIZE) return;

			mJpeg = tmp;
			CreateDecoder(mJpeg);
			SaveData.Instance().Fill(mAddress, Util.PHOTO_SIZE, 0);
			SaveData.Instance().WriteValue(mAddress, mJpeg);

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
		}

		public void Export(String filename)
		{
			if (mJpeg == null) return;
			System.IO.File.WriteAllBytes(filename, mJpeg);
		}

		public System.Windows.Media.ImageSource Image
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
