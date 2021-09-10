using System;
using System.Globalization;
using System.Windows.Data;

namespace CMNDAT
{
	class PartyTypeConverter : IValueConverter
	{
		// Type
		// Human = 1
		// Animal = 2
		// Monster = 4
		private uint[] mTypes = { 0, 1, 2, 4 };

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			
			uint type = (uint)value;
			for (int i = 0; i < mTypes.Length; i++)
			{
				if (type == mTypes[i]) return i;
			}
			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int index = (int)value;
			return mTypes[index];
		}
	}
}
