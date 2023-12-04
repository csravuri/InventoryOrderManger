namespace IOManager.Utils
{
	public static class GlobalConstants
	{
		public static string RootFolder => FileSystem.AppDataDirectory;

		public const string ItemSearchText = "ItemSearchText";
		public const string ItemUpdate = "ItemUpdate";
		public const string ItemSelect = "ItemSelect";
		public const string SelectedItems = "SelectedItems";

		public static string ImagesFolder => Path.Combine(RootFolder, ImagesSubFolder);
		public const string DefaultItemImage = "default_image.png";
		const string ImagesSubFolder = "Images";
	}

}
