using System.Windows.Input;
using Xamarin.Forms;

namespace iOSSecondaryToolbarMenubar.ViewModels
{
	public class MainPageViewModel
	{
		public ICommand Item1Command { get; internal set; }
		
		//Constructor
		public MainPageViewModel()
		{
			Item1Command = new Command(() => Item1Method());
		}

		private void Item1Method()
		{
		}
	}
}
