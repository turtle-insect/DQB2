using System;
using System.Globalization;
using System.Windows.Data;

namespace CMNDAT
{
	class CropIDConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Info.Instance().Search(Info.Instance().Crop, (uint)value)?.Name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
