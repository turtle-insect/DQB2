using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINKDATA
{
	internal class ViewModel : INotifyPropertyChanged
	{
		private String mOriginalFile = "";
		public String OriginalFile
		{
			get { return mOriginalFile; }
			set
			{
				mOriginalFile = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OriginalFile)));
			}
		}

		private String mOutputPath = @"E:\DQB2\output";
		public String OutputPath
		{
			get { return mOutputPath; }
			set
			{
				mOutputPath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputPath)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public bool Decryption()
		{
			// Check.
			if (String.IsNullOrEmpty(OriginalFile)) return false;
			if (String.IsNullOrEmpty(OutputPath)) return false;
			if (!System.IO.File.Exists(OriginalFile)) return false;
			if (!System.IO.Directory.Exists(OutputPath))
			{
				System.IO.Directory.CreateDirectory(OutputPath);
			}
			if (!System.IO.Directory.Exists(OutputPath)) return false;
			String binFile = OriginalFile.Substring(0, OriginalFile.Length - 3) + "BIN";
			if (!System.IO.File.Exists(binFile)) return false;


			// Decrypt.
			var idxList = new List<IDX>();
			Byte[] buffer = System.IO.File.ReadAllBytes(OriginalFile);
			for (int index = 0; index < buffer.Length; index += 32)
			{
				var idx = new IDX();
				idx.Offset = BitConverter.ToUInt64(buffer, index + 0);
				idx.UncompressedSize = BitConverter.ToUInt64(buffer, index + 8);
				idx.CompressedSize = BitConverter.ToUInt64(buffer, index + 16);
				idx.IsCompressed = BitConverter.ToUInt64(buffer, index + 24);
				idxList.Add(idx);
			}

			buffer = System.IO.File.ReadAllBytes(binFile);
			for (int idxIndex = 0; idxIndex < idxList.Count; idxIndex++)
			{
				var idx = idxList[idxIndex];
				if (idx.IsCompressed == 0)
				{
					if (idx.UncompressedSize == 0) continue;
					Byte[] tmp = new Byte[idx.UncompressedSize];
					Array.Copy(buffer, (int)idx.Offset, tmp, 0, tmp.Length);
					System.IO.File.WriteAllBytes(System.IO.Path.Combine(OutputPath, $"{idxIndex:0000}"), tmp);
				}
				else
				{
					Int32 offset = (int)idx.Offset + 4;
					Int32 blockCount = BitConverter.ToInt32(buffer, offset);
					var blockSizeList = new List<Int32>();
					offset += 8;
					for (Int32 blockIndex = 0; blockIndex < blockCount; blockIndex++)
					{
						blockSizeList.Add(BitConverter.ToInt32(buffer, offset));
						offset += 4;
					}

					for (Int32 blockIndex = 0; blockIndex < blockSizeList.Count; blockIndex++)
					{
						if (offset % 0x80 != 0)
						{
							offset = (offset / 0x80 + 1) * 0x80;
						}

						// 0:Buffer Length
						// 4:Buffer
						Int32 blockSize = blockSizeList[blockIndex] - 4;
						Byte[] tmp = new Byte[blockSize];
						Array.Copy(buffer, offset + 4, tmp, 0, blockSize);
						try
						{
							System.IO.File.WriteAllBytes(System.IO.Path.Combine(OutputPath, $"{idxIndex:0000} {blockIndex:0000}"), Ionic.Zlib.ZlibStream.UncompressBuffer(tmp));
						}
						catch(Exception)
						{
							System.Console.WriteLine($"{idx.Offset} {idx.CompressedSize} {idx.UncompressedSize} {idx.IsCompressed}");
						}
					}
				}
			}
			return true;
		}
	}
}
