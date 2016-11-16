﻿using System;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;
using keep.grass.Helpers;
using System.Diagnostics;

namespace keep.grass
{
	public class AlphaFriendsPage : ResponsiveContentPage
	{
		AlphaApp Root = AlphaFactory.MakeSureApp();
		Languages.AlphaLanguage L = AlphaFactory.MakeSureLanguage();

		const int MaxFriendCount = 5;
		VoidEntryCell[] FriendNameCellList = null;

		public AlphaFriendsPage()
		{
			Title = L["Friends"];
			FriendNameCellList = Enumerable.Range(0, MaxFriendCount)
         		.Select
               	(
               		i =>
					{
						var Cell = AlphaFactory.MakeEntryCell();
						Cell.Label = L["User ID"];
						return Cell;
					}
              	)
             	.ToArray();
		}

		public override void Build()
		{
			base.Build();
			Debug.WriteLine("AlphaSettingsPage.Rebuild();");

			if (Width <= Height || FriendNameCellList.Count() < 6)
			{
				Content = new StackLayout
				{
					Children =
					{
						new TableView
						{
							Root = new TableRoot
							{
								new TableSection(L["Friends"])
								{
									FriendNameCellList.Select(i => i.AsCell()),
								},
							},
						},
					},
				};
			}
			else
			{
				Content = new StackLayout
				{
					Children =
					{
						new StackLayout
						{
							Orientation = StackOrientation.Horizontal,
							Spacing = 1.0,
							BackgroundColor = Color.Gray,
							Children =
							{
								new TableView
								{
									BackgroundColor = Color.White,
									Root = new TableRoot
									{
										new TableSection(L["Friends"])
										{
											FriendNameCellList.Where((i,index) => 0 == index %2).Select(i => i.AsCell()),
										},
									},
								},
								new TableView
								{
									BackgroundColor = Color.White,
									Root = new TableRoot
									{
										new TableSection(L["Friends"])
										{
											FriendNameCellList.Where((i,index) => 1 == index %2).Select(i => i.AsCell()),
										},
									},
								},
							},
						},
					},
				};
			}
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

			for (var i = 0; 0 < FriendNameCellList.Count(); ++i)
			{
				FriendNameCellList[i].Text = Settings.GetFriend(i);
			}
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			bool IsChanged = false;

			for (var i = 0; 0 < FriendNameCellList.Count(); ++i)
			{
				FriendNameCellList[i].Text = Settings.GetFriend(i);

				var NewFriend = FriendNameCellList[i].Text.Trim();
				if (Settings.GetFriend(i) != NewFriend)
				{
					Settings.SetFriend(i, NewFriend);
					//これ相当のデータが Friend ごとに必要なんじゃない？
					//Settings.IsValidUserName = false;
					IsChanged = true;
				}
			}
			if (IsChanged)
			{
				Root.OnChangeSettings();
			}
		}
	}
}


