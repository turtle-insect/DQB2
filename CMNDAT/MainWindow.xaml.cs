using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace CMNDAT
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			String[] files = e.Data.GetData(DataFormats.FileDrop) as String[];
			if (files == null) return;

			SaveData.Instance().Open(files[0]);
			DataContext = new ViewModel();
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "CMNDAT.BIN|CMNDAT.BIN";
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Open(dlg.FileName);
			DataContext = new ViewModel();
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileImport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Import(dlg.FileName);
		}

		private void MenuItemFileExport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Export(dlg.FileName);
		}


		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonItemChoice_Click(object sender, RoutedEventArgs e)
		{
			Item item = (sender as Button)?.DataContext as Item;
			ItemChoice(item);
		}

		private void ButtonResidentWeaponItemChoice_Click(object sender, RoutedEventArgs e)
		{
			Pelple item = (sender as Button)?.DataContext as Pelple;
			ItemChoice(item?.Weapon);
		}

		private void ButtonResidentArmorItemChoice_Click(object sender, RoutedEventArgs e)
		{
			Pelple item = (sender as Button)?.DataContext as Pelple;
			ItemChoice(item?.Armor);
		}

		private void ButtonBluePrintExport_Click(object sender, RoutedEventArgs e)
		{
			BluePrint item = (sender as Button)?.DataContext as BluePrint;
			if (item == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			System.IO.File.WriteAllBytes(dlg.FileName, SaveData.Instance().ReadValue(item.Address, Util.BluePrintSize));
		}

		private void ButtonBluePrintImport_Click(object sender, RoutedEventArgs e)
		{
			BluePrint item = (sender as Button)?.DataContext as BluePrint;
			if (item == null) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			Byte[] buf = System.IO.File.ReadAllBytes(dlg.FileName);
			if (buf.Length != Util.BluePrintSize) return;
			SaveData.Instance().WriteValue(item.Address, buf);

			item.Reload();
		}

		private void ButtonPeopleExport_Click(object sender, RoutedEventArgs e)
		{
			Pelple item = (sender as Button)?.DataContext as Pelple;
			if (item == null) return;

			var dlg = new SaveFileDialog();
			dlg.FileName = item.Name;
			if (dlg.ShowDialog() == false) return;

			System.IO.File.WriteAllBytes(dlg.FileName, SaveData.Instance().ReadValue(item.Address, Util.PeopleSize));
		}

		private void ButtonPeopleImport_Click(object sender, RoutedEventArgs e)
		{
			Pelple item = (sender as Button)?.DataContext as Pelple;
			if (item == null) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			Byte[] buf = System.IO.File.ReadAllBytes(dlg.FileName);
			if (buf.Length != Util.PeopleSize) return;
			SaveData.Instance().WriteValue(item.Address, buf);

			item.Reload();
		}

		private void ComboBoxResidentFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			PeopleFilter(sender, vm.Residents, Util.ResidentAddress, Util.ResidentCount, Util.PeopleSize);
		}

		private void ComboBoxStoryPeopleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			PeopleFilter(sender, vm.StoryPeople, Util.StoryPeopleAddress, Util.StoryPeopleCount, Util.PeopleSize);
		}

		private void TextBoxCraftItem_TextChanged(object sender, TextChangedEventArgs e)
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm == null) return;

			String filter = (sender as TextBox)?.Text;

			vm.Crafts.Clear();

			for (uint i = 0; i < Info.Instance().Item.Count; i++)
			{
				var item = Info.Instance().Item[(int)i];
				if (item.Value == 0) continue;

				if (String.IsNullOrEmpty(filter) || item.Name.IndexOf(filter) >= 0)
				{
					vm.Crafts.Add(new Craft(Util.CraftAddress + item.Value, item.Value));
				}
			}
		}

		private void ItemChoice(Item item)
		{
			if (item == null) return;

			var window = new ChoiceWindow();
			window.ID = item.ID;
			window.ShowDialog();
			item.ID = window.ID;

			item.Count = item.ID == 0 ? 0 : 1u;
		}

		private void PeopleFilter(object sender, ObservableCollection<Pelple> collection, uint address, uint count, uint size)
		{
			int index = (sender as ComboBox).SelectedIndex;
			uint island = Info.Instance().StoryIsland[index].Value;
			collection.Clear();

			for (uint i = 0; i < count; i++)
			{
				Pelple item = new Pelple(address + i * size);
				if (island == 0 || item.Island == island)
				{
					collection.Add(item);
				}
			}
		}
	}
}
