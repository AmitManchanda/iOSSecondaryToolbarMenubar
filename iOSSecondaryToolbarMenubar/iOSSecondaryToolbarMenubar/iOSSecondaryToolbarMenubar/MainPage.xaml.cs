using iOSSecondaryToolbarMenubar.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace iOSSecondaryToolbarMenubar
{
	public partial class MainPage : CustomToolbarContentPage
	{
		private ToolbarItem _item1;

		public override event EventHandler ToolbarItemAdded;

		public MainPage()
		{
			InitializeComponent();
			BindingContext = new MainPageViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			RefreshToolbarOptions();
		}

		public ICommand Item1Command { get; set; }

		private void RefreshToolbarOptions()
		{
			var viewModel = BindingContext as MainPageViewModel;

			Item1Command = new Command(async () => await Item1CommandMethod());

			ToolbarItems.Clear();

			if (viewModel != null)
			{
				_item1 = new ToolbarItem
				{
					Text = "Item 1",
					Command = Item1Command,
					Order = ToolbarItemOrder.Secondary
				};

				ToolbarItems.Add(_item1);
				OnToolbarItemAdded();
			}
		}

		private async Task Item1CommandMethod()
		{
			await DisplayAlert("Menubar Item", "Toolbar Item Clicked!", "OK");
			if (BindingContext is MainPageViewModel viewModel && viewModel.Item1Command.CanExecute(null))
			{
				viewModel.Item1Command.Execute(null);
			}

			await Task.CompletedTask;
		}

		protected void OnToolbarItemAdded()
		{
			var e = ToolbarItemAdded;
			e?.Invoke(this, new EventArgs());
		}

		public override Color CellBackgroundColor => Color.White;

		public override Color CellTextColor => Color.Black;

		public override Color MenuBackgroundColor => Color.White;

		public override float RowHeight => 56;

		public override Color ShadowColor => Color.Black;

		public override float ShadowOpacity => 0.3f;

		public override float ShadowRadius => 5.0f;

		public override float ShadowOffsetDimension => 5.0f;

		public override float TableWidth => 250;
	}
}
