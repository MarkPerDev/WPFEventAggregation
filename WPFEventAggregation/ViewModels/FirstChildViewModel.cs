using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFEventAggregation.Models;

namespace WPFEventAggregation.ViewModels
{
	// Example of how to intercept a message based on subscription and 
	// displaying of the message using NotifyOfPropertyChange - Binding Path...
	public class FirstChildViewModel : Screen, IHandle<EventMessaging>
	{
	
		private string _displayMessage;
		public FirstChildViewModel(IEventAggregator eventAggregator)
		{
			eventAggregator.Subscribe(this);
		}

		public string DisplayMessage
		{
			get { return _displayMessage; }
			set
			{
				_displayMessage = value;
				NotifyOfPropertyChange(() => DisplayMessage);
			}
		}

		public void Handle(EventMessaging message)
		{
			DisplayMessage = message.Text;
		}
	}
}
