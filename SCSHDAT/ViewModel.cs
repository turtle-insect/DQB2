using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SCSHDAT
{
	class ViewModel
	{
		public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();

		public ViewModel()
		{
			for(uint i = 0; i < 100; i++)
			{
				Photos.Add(new Photo(Util.PHOTO_ADDRESS + Util.PHOTO_SIZE * i));
			}
		}
	}
}
