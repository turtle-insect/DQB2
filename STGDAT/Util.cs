namespace STGDAT
{
	internal class Util
	{
		public static void WriteNumber(uint address, uint size, uint value, uint min, uint max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			SaveData.Instance().WriteNumber(address, size, value);
		}

		public static uint CalcChunkAddress(uint chunkID)
		{
			return 0x183FEF0 + chunkID * 0x30000;
		}
	}
}
