using System;
using System.IO;

namespace LINKDATA
{
	internal class G1T
	{
		public String Magic { get; private set; } = "";
		public UInt32 Version { get; private set; }
		public UInt32 FileSize { get; private set; }
		public UInt32 HeaderSize { get; private set; }
		public UInt32 Textures { get; private set; }
		public UInt32 Platform { get; private set; }
		public UInt32 ExtraSize { get; private set; }
		
		public void Read(String filename)
		{
			if (!System.IO.File.Exists(filename)) return;
			
			Byte[] buffer = File.ReadAllBytes(filename);
			Magic = System.Text.Encoding.ASCII.GetString(buffer, 0, 4);
			if (Magic != "GT1G") return;
			
			Version = BitConverter.ToUInt32(buffer, 4);
			FileSize = BitConverter.ToUInt32(buffer, 8);
			HeaderSize = BitConverter.ToUInt32(buffer, 12);
			Textures = BitConverter.ToUInt32(buffer, 16);
			Platform = BitConverter.ToUInt32(buffer, 20);
			ExtraSize = BitConverter.ToUInt32(buffer, 24);
			
			for (int index = 0; index < Textures; index++)
			{
				
				UInt32 offset = BitConverter.ToUInt32(buffer, (int)HeaderSize + index * 4);
				
				Byte data = buffer[offset + 3];
				UInt32 dx = 1U << (data & 0xF0 >> 4);
				UInt32 dy = 1U << (data & 0x0F);
				
				// texture type
				switch (buffer[offset + 2])
				{
					// DXT5
					case 0x5B:
						UInt32 cols = System.Math.Max(1U, (dx + 3) / 4);
						UInt32 rows = System.Math.Max(1U, (dy + 3) / 4);
						UInt32 filesize = cols * rows * 16;
						Byte[] dds = new Byte[filesize];
						offset += 0x14;
						break;
					
					default:
						break;
				}
			}
		}
	}
}
