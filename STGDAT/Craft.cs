namespace STGDAT
{
	class Craft
	{
		private readonly uint mAddress;

		public Craft(uint address)
		{
			mAddress = address;
		}

		public void Clear()
		{
			SaveData.Instance().Fill(mAddress, 52, 0);
		}
	}
}
