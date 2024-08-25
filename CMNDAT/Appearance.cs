namespace CMNDAT
{
	internal class Appearance
	{
		public Item Hammer { get; private set; } = new Item(0x6A8C6) { mCountForce = false };
		public Item Weapon { get; private set; } = new Item(0x6A8C2) { mCountForce = false };
		public Item Armmer { get; private set; } = new Item(0x6A8CA) { mCountForce = false };
		public Item Shield { get; private set; } = new Item(0x6A8CE) { mCountForce = false };
		public Item Head { get; private set; } = new Item(0x6A8D2) { mCountForce = false };
		public Item Accessories1 { get; private set; } = new Item(0x6A8D6) { mCountForce = false };
		public Item Accessories2 { get; private set; } = new Item(0x6A8DA) { mCountForce = false };
		public Item Accessories3 { get; private set; } = new Item(0x6A8DE) { mCountForce = false };
	}
}
