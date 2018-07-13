using iOSSecondaryToolbarMenubar.Interface;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iOSSecondaryToolbarMenubar
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomToolbarContentPage : ContentPage, IAddToolbarItem
	{
		public CustomToolbarContentPage ()
		{
			InitializeComponent ();
		}

		public virtual Color CellBackgroundColor { get; set; }

		public virtual Color CellTextColor { get; set; }

		public virtual Color MenuBackgroundColor { get; set; }

		public virtual float RowHeight { get; set; }

		public virtual Color ShadowColor { get; set; }

		public virtual float ShadowOpacity { get; set; }

		public virtual float ShadowRadius { get; set; }

		public virtual float ShadowOffsetDimension { get; set; }

		public virtual float TableWidth { get; set; }

		public virtual event EventHandler ToolbarItemAdded;
	}
}