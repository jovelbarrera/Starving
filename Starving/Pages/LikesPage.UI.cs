using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Starving.Services;
using Starving.Models;
using Starving.Helpers;

namespace Starving
{
	public partial class LikesPage
	{
		private RelativeLayout _mainLayout;
		private RelativeLayout _loadingLayoutLikes;
		private ListView _userList;
		private Label _titleLablel;
		private Label _emptyLabel;

		public void InitializeComponents ()
		{
			Title = _place.Name;

			_mainLayout = new RelativeLayout ();

			_emptyLabel = new Label {
				Style = Styles.Subtitle,
				HorizontalTextAlignment = TextAlignment.Center
			};
			_mainLayout.Children.Add (_emptyLabel,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height)
			);

			_loadingLayoutLikes = DynamicsLayouts.Loading (AppResources.RetrievingLikes, Color.Transparent);
			_mainLayout.Children.Add (_loadingLayoutLikes,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height)
			);

			_titleLablel = new Label {
				Text = AppResources.UsersWhoLikes,
				Style = Styles.Title,
				TextColor = Styles.Colors.SecondaryColor,
				IsVisible = false,
			};
			_mainLayout.Children.Add (_titleLablel,
				Constraint.Constant (20),
				Constraint.Constant (20),
				Constraint.RelativeToParent (p => p.Width - 40),
				Constraint.Constant (40)
			);

			_userList = new ListView {
				RowHeight = 80,
				ItemTemplate = new DataTemplate (typeof(UserViewCell)),
				IsVisible = false,
			};
			_userList.ItemTapped += _userList_ItemTapped;
			_mainLayout.Children.Add (_userList,
				Constraint.Constant (0),
				Constraint.RelativeToView (_titleLablel, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToView (_titleLablel, (p, v) => p.Height - v.Height)
			);

			Content = _mainLayout;
		}
	}
}


