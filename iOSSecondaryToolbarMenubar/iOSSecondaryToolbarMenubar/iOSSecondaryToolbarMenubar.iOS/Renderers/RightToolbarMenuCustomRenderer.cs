using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using iOSSecondaryToolbarMenubar;
using iOSSecondaryToolbarMenubar.Interface;
using iOSSecondaryToolbarMenubar.iOS.Renderers;
using iOSSecondaryToolbarMenubar.iOS.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomToolbarContentPage), typeof(RightToolbarMenuCustomRenderer))]
namespace iOSSecondaryToolbarMenubar.iOS.Renderers
{
	public class RightToolbarMenuCustomRenderer : PageRenderer
	{
		private List<ToolbarItem> _primaryItems;
		private List<ToolbarItem> _secondaryItems;
		private UITableView _table;
		private UITapGestureRecognizer _tapGestureRecognizer;
		private UIView _transparentView;

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			if (e.NewElement is IAddToolbarItem item)
			{
				item.ToolbarItemAdded += Item_ToolbarItemAdded;
			}
			base.OnElementChanged(e);
		}

		private void Item_ToolbarItemAdded(object sender, System.EventArgs e)
		{
			if (Element is ContentPage page)
			{
				_primaryItems = page.ToolbarItems.Where(i => i.Order == ToolbarItemOrder.Primary).ToList();
				_secondaryItems = page.ToolbarItems.Where(i => i.Order == ToolbarItemOrder.Secondary).ToList();
				_secondaryItems.ForEach(t => page.ToolbarItems.Remove(t));
			}

			var element = (ContentPage)Element;

			if (_secondaryItems?.Count == 0 && element.ToolbarItems.Any(a => a.Icon == "more.png"))
			{
				element.ToolbarItems.Clear();
			}
			else if (_secondaryItems?.Count >= 1 && !element.ToolbarItems.Any(a => a.Icon == "more.png"))
			{
				element.ToolbarItems.Add(new ToolbarItem()
				{
					Order = ToolbarItemOrder.Primary,
					Icon = "more.png",
					Priority = 1,
					Command = new Command(ToggleDropDownMenuVisibility)
				});
			}
		}

		private void ToggleDropDownMenuVisibility()
		{
			if (!IsTableExists())
			{
				if ((View?.Subviews != null)
					&& (View.Subviews.Length > 0)
					&& (View.Bounds != null)
					&& (_secondaryItems != null)
					&& (_secondaryItems.Count > 0))
				{
					_table = OpenDropDownMenu(Element as IAddToolbarItem);
					Add(_table);
				}
			}
			else
				CloseDropDownMenu();
		}

		private bool IsTableExists()
		{
			if (View?.Subviews != null)
			{
				foreach (var subview in View.Subviews)
				{
					if (_table != null && subview == _table)
					{
						return true;
					}
				}
			}
			if (_tapGestureRecognizer != null)
			{
				_transparentView?.RemoveGestureRecognizer(_tapGestureRecognizer);
				_tapGestureRecognizer = null;
			}
			_table = null;
			_tapGestureRecognizer = null;
			return false;
		}

		private UITableView OpenDropDownMenu(IAddToolbarItem secondaryMenuSupport)
		{
			_transparentView = _transparentView = new UIView(new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height))
			{
				BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0)
			};
			_tapGestureRecognizer = new UITapGestureRecognizer(CloseDropDownMenu);
			_transparentView.AddGestureRecognizer(_tapGestureRecognizer);
			Add(_transparentView);

			UITableView table = null;
			if (_secondaryItems != null && _secondaryItems.Count > 0)
			{
				table = new UITableView(GetPositionForDropDownMenu(secondaryMenuSupport.RowHeight, secondaryMenuSupport.TableWidth))
				{
					Source = new TableSource(_secondaryItems, _transparentView),
					ClipsToBounds = false
				};

				table.ScrollEnabled = false;
				table.Layer.ShadowColor = secondaryMenuSupport.ShadowColor.ToCGColor();
				table.Layer.ShadowOpacity = secondaryMenuSupport.ShadowOpacity;
				table.Layer.ShadowRadius = secondaryMenuSupport.ShadowRadius;
				table.Layer.ShadowOffset = new System.Drawing.SizeF(secondaryMenuSupport.ShadowOffsetDimension, secondaryMenuSupport.ShadowOffsetDimension);
				table.BackgroundColor = secondaryMenuSupport.MenuBackgroundColor.ToUIColor();
			}
			return table;
		}

		public override void ViewWillDisappear(bool animated)
		{
			CloseDropDownMenu();
			base.ViewWillDisappear(animated);
		}

		private RectangleF GetPositionForDropDownMenu(float rowHeight, float tableWidth)
		{
			if ((View?.Bounds != null)
				&& (_secondaryItems != null)
				&& (_secondaryItems.Count > 0))
			{
				return new RectangleF(
					(float)View.Bounds.Width - tableWidth,
					0,
					tableWidth,
					_secondaryItems.Count() * rowHeight);
			}
			else
			{
				return new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
			}
		}

		private void CloseDropDownMenu()
		{
			if (_table != null)
			{
				if (_tapGestureRecognizer != null)
				{
					_transparentView?.RemoveGestureRecognizer(_tapGestureRecognizer);
					_tapGestureRecognizer = null;
				}

				if (View?.Subviews != null)
				{
					foreach (var subview in View.Subviews)
					{
						if (subview == _table)
						{
							_table.RemoveFromSuperview();
							break;
						}
					}

					if (_transparentView != null)
					{
						foreach (var subview in View.Subviews)
						{
							if (subview == _transparentView)
							{
								_transparentView.RemoveFromSuperview();
								break;
							}
						}
					}
				}
				_table = null;
				_transparentView = null;
			}
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

			if (_table != null)
			{
				if (Element is IAddToolbarItem secondaryMenuSupport)
					PositionExistingDropDownMenu(secondaryMenuSupport.RowHeight, secondaryMenuSupport.TableWidth);
			}
		}

		private void PositionExistingDropDownMenu(float rowHeight, float tableWidth)
		{
			if ((View?.Bounds != null)
				&& (_secondaryItems != null)
				&& (_secondaryItems.Count > 0)
				&& (_table != null))
			{
				_table.Frame = GetPositionForDropDownMenu(rowHeight, tableWidth);
			}
		}
	}
}