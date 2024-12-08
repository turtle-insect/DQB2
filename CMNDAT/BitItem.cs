using System.ComponentModel;

namespace CMNDAT
{
	internal class BitItem : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly uint mAddress;
		private readonly uint mBit;

		public BitItem(uint address, uint bit)
		{
			mAddress = address;
			mBit = bit;
		}

		public bool Value
		{
			get { return SaveData.Instance().ReadBit(mAddress, mBit); }
			set
			{
				SaveData.Instance().WriteBit(mAddress, mBit, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}
	}
}
