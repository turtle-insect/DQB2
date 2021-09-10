namespace CMNDAT
{
	class Crop
	{
		private readonly uint mAddress;

		public Crop(uint address, uint id)
		{
			mAddress = address;
			ID = id;
		}

		public uint ID { get; private set; }

		public uint Count
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 4); }
			set { SaveData.Instance().WriteNumber(mAddress, 4, value); }
		}

		public bool Harvest
		{
			get { return SaveData.Instance().ReadBit(mAddress + 4, 0); }
			set { SaveData.Instance().WriteBit(mAddress + 4, 0, value); }
		}

		public bool Plant
		{
			get { return SaveData.Instance().ReadBit(mAddress + 4, 2); }
			set { SaveData.Instance().WriteBit(mAddress + 4, 2, value); }
		}

		public bool Growth
		{
			get { return SaveData.Instance().ReadBit(mAddress + 4, 3); }
			set { SaveData.Instance().WriteBit(mAddress + 4, 3, value); }
		}
	}
}
