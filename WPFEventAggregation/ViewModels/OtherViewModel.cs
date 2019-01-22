using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFEventAggregation.Models;

namespace WPFEventAggregation.ViewModels
{
	// Example of publishing a message to be sent to other view models
	public class OtherViewModel : Screen
	{
		private readonly IEventAggregator _eventAggregator;
		public OtherViewModel(IEventAggregator eventAggregator)
		{
			// Set the local _eventAggregator var
			this._eventAggregator = eventAggregator;
		}

		public void SendMessage()
		{
			var em = new EventMessaging()
			{
				Text = "Hell Yeah!"
			};

			// Publish the object 
			_eventAggregator.PublishOnUIThread(em);
		}
	}
}
