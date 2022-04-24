using System;
using System.Globalization;
using System.Windows.Data;

namespace CMNDAT
{
	internal class ItemIDConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			var info = Info.Instance().Search(Info.Instance().Item, id);
			if (info == null) return "";

			var name = $"{info.Name} {{ {info.Value} }}";
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
