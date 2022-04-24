namespace STGDAT
{
	internal class Tableware
	{
		private readonly uint mAddress;
		public Item Item { get; set; }

		public Tableware(uint info, uint item)
		{
			mAddress = info;
			Item = new Item(item);
		}

		public void Clear()
		{
			SaveData.Instance().Fill(mAddress, 8, 0);
			Item.ID = 0;
			Item.Count = 0;
		}
	}
}
