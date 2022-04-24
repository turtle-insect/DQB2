using System;
using System.Windows.Input;

namespace G1L
{
	internal class ViewModel
	{
		public String WorkPath { get; set; } = "";
		public String OutputPath { get; set; } = "";
		public ICommand ExecuteCommand { get; set; }

		public ViewModel()
		{
			ExecuteCommand = new ExecuteCommand(Execute);
		}

		private void Execute()
		{
			if (!System.IO.Directory.Exists(WorkPath)) return;
			if (!System.IO.Directory.Exists(OutputPath))
			{
				System.IO.Directory.CreateDirectory(OutputPath);
			}

			foreach (var file in System.IO.Directory.GetFiles(WorkPath))
			{
				String filename = System.IO.Path.GetFileName(file);
				if (System.IO.Path.GetExtension(filename) != ".win") continue;
				// bgm0xx.g1l.win => bgm0xx
				filename = filename.Substring(0, filename.IndexOf('.'));

				Byte[] buffer = System.IO.File.ReadAllBytes(file);

				String header = System.Text.Encoding.UTF8.GetString(buffer, 0, 8);
				if (header != "_L1G0000") continue;
				uint filesize = BitConverter.ToUInt32(buffer, 8);
				if (filesize != buffer.Length) continue;
				uint filecount = BitConverter.ToUInt32(buffer, 20);
				for (int index = 0; index < filecount; index++)
				{
					int address = BitConverter.ToInt32(buffer, index * 4 + 24);
					header = System.Text.Encoding.UTF8.GetString(buffer, address, 4);
					int loop = 1;
					if (header == "ATSL")
					{
						loop = BitConverter.ToInt32(buffer, address + 20);
						address += 0xB0;
					}
					for (int atsl = 0; atsl < loop; atsl++)
					{
						header = System.Text.Encoding.UTF8.GetString(buffer, address, 4);
						if (header != "KOVS") continue;

						filesize = BitConverter.ToUInt32(buffer, address + 4);
						address += 0x20;
						if (filesize < 0xFF) continue;
						Byte[] ogg = new Byte[filesize];
						for (int i = 0; i < 0x100; i++)
						{
							ogg[i] = (Byte)(buffer[address + i] ^ i);
						}
						Array.Copy(buffer, address + 0x100, ogg, 0x100, filesize - 0x100);
						System.IO.File.WriteAllBytes(System.IO.Path.Combine(OutputPath, $"{filename}_{index}_{atsl}.ogg"), ogg);
						address += (int)filesize + 1;
					}
				}
			}

			System.Windows.MessageBox.Show("End");
		}
	}
}
