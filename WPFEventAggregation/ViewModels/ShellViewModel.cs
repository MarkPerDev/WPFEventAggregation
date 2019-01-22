using Caliburn.Micro;
using System;
using WPFEventAggregation.Models;

namespace WPFEventAggregation.ViewModels
{
	public class ShellViewModel : Conductor<object>, IScreen // Everything inherits from object
	{
		private BindableCollection<Person> _people = new BindableCollection<Person>();
		private Person _selectedPerson;
		private string _firstName = "Mark";
		private string _lastName;

		private readonly IEventAggregator _eventAggregator;

		public ShellViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			_eventAggregator.Subscribe(this);

			People.Add(new Person { FirstName = "Steve", LastName = "Vai" });
			People.Add(new Person { FirstName = "Joe", LastName = "Satriani" });
			People.Add(new Person { FirstName = "Mark", LastName = "Perreault" });
		}

		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
				NotifyOfPropertyChange(() => FirstName);
				NotifyOfPropertyChange(() => FullName);
			}
		}

		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
				NotifyOfPropertyChange(() => LastName);
				NotifyOfPropertyChange(() => FullName);
			}
		}

		public string FullName
		{
			get { return $"{FirstName} { LastName}"; }

		}

		public BindableCollection<Person> People
		{
			get { return _people; }
			set { _people = value; }
		}

		public Person SelectedPerson
		{
			get
			{
				return _selectedPerson;
			}
			set
			{
				_selectedPerson = value;
				NotifyOfPropertyChange(() => SelectedPerson);
			}
		}

		// these parameters are wired to the props FirstName and LastName via caliburn.micro
		// Notice the "Can" keyword if using x:Name=Cleartext, caliburn will call 

		public bool CanClearText(string firstName, string lastName)
		{
			if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		// these parameters are wired to the props FirstName and LastName via caliburn.micro
		public void ClearText(string firstName, string lastName)
		{
			FirstName = "";
			LastName = "";
		}

		public void LoadPageOne()
		{
			// Loads a view within the calling view
			ActivateItem(new FirstChildViewModel(_eventAggregator));
		}

		public void LoadPageTwo()
		{
			// Loads a view within the calling view
			ActivateItem(new SecondChildViewModel(_eventAggregator));
		}

		public void LoadPageOther()
		{
			// Loads a new window
			var newView = new OtherViewModel(_eventAggregator);
			IWindowManager manager = new WindowManager();
			manager.ShowWindow(newView);
		}
	}
}