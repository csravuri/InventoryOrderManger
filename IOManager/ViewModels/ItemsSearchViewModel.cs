﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;

namespace IOManager.ViewModels
{
	public partial class ItemsSearchViewModel : ObservableObject
	{
		public ItemsSearchViewModel(DbConnection connection)
		{
			Connection = connection;
			Items = new ObservableCollection<ItemModel>();
		}

		public ObservableCollection<ItemModel> Items { get; }

		[ObservableProperty]
		string searchText;

		[RelayCommand]
		async Task Search()
		{
			if (string.IsNullOrWhiteSpace(SearchText))
			{
				return;
			}

			Items.Clear();
			var items = await Connection.GetAll<ItemModel>(x => x.ItemName.ToLower().Contains(SearchText.ToLower()));

			foreach (var item in items)
			{
				Items.Add(item);
			}

		}
		DbConnection Connection { get; }
	}
}
