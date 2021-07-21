using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		public string SubTotal
		{
			get
			{
				// TODO - replace with calculation
				return "$0.00";
			}
		}

		public string Tax
		{
			get
			{
				// TODO - replace with calculation
				return "$0.00";
			}
		}
		public string Total
		{
			get
			{
				// TODO - replace with calculation
				return "$0.00";
			}
		}

		private BindingList<string> _products;

		public BindingList<string> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(()=> Products);
			}
		}

		private BindingList<string> _cart;

		public BindingList<string> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}


		//putting the proper type
		//caliburn does a validation the data entered
		private int _itemQuantity;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}

		public bool AddToCart
		{
			get
			{
				bool output = false;

				return output;
			}
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				return output;
			}
		}

		public void RemoveFromCart()
		{
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				return output;
			}
		}

		public void CheckOut()
		{

		}
	}
}
