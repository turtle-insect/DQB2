using System.Collections.ObjectModel;

namespace SCSHDAT
{
	internal class ViewModel
	{
		public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();

		public ViewModel()
		{
			for (uint i = 0; i < 100; i++)
			{
				Photos.Add(new Photo(0x69E90 + 409600 * i, 409600));
			}
		}
	}
}
