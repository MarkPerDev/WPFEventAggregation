using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFEventAggregation.Models;

namespace WPFEventAggregation.ViewModels
{
	public class SecondChildViewModel : Screen, IHandle<EventMessaging>
	{
		private string _displayMessage;
		public SecondChildViewModel(IEventAggregator eventAggregator)
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
