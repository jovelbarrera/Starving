using System;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Starving.Controls;

namespace Starving.Pages
{
	public partial class CreateReviewPage
	{
		private RelativeLayout _mainLayout;
		private Label _questionLabel;
		private Editor _commentEditor;

		public void InitializeComponents ()
		{
			Title = _place.Name;
			_mainLayout = new RelativeLayout ();

			_questionLabel = new Label {
				Style = Styles.Title,
				TextColor = Styles.Colors.SecondaryColor,
			};
			_mainLayout.Children.Add (_questionLabel,
				Constraint.Constant (20),
				Constraint.Constant (20),
				Constraint.RelativeToParent (p => p.Width - 40),
				Constraint.Constant (40)
			);

			var invitationLabel = new Label {
				Text = AppResources.ShareReview,
				Style = Styles.Subtitle,
			};
			_mainLayout.Children.Add (invitationLabel,
				Constraint.RelativeToView (_questionLabel, (p, v) => v.X),
				Constraint.RelativeToView (_questionLabel, (p, v) => v.Y + v.Height),
				Constraint.RelativeToView (_questionLabel, (p, v) => v.Width),
				Constraint.RelativeToView (_questionLabel, (p, v) => v.Height)
			);

			_commentEditor = new Editor ();
			_commentEditor.TextChanged += _commentEditor_TextChanged;
			_mainLayout.Children.Add (_commentEditor,
				Constraint.Constant (0),
				Constraint.RelativeToView (invitationLabel, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.Constant (100)
			);

			ToolbarItem postReviewButton = new ToolbarItem {
				Icon = "ic_action_done.png",
				Text = AppResources.Done
			};
			postReviewButton.Clicked += PostReviewButton_Clicked;
			ToolbarItems.Add (postReviewButton);

			Content = _mainLayout;
		}
	}
}


