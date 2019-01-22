Event Aggregation and Message Binding Example:
This whole project has examples of event aggregation (Passing messages between view models), 
object binding, real time property changes (NotifyOfPropertyChange...) and how to load views within a view using "ActivateItem" method.

1. Add using Caliburn.Micro to all relavent classes
2. It all starts at app.xaml and Boostrapper.cs: 
   App.xaml should point to Boostrapper.cs as it's starting point
   e.g.,  <local:Bootstrapper  x:Key="Boostrapper" />

	Boostrapper.cs:
	Inhereits from BootstrapperBase and the following methods to Boostrapper.cs:
	protected override void Configure()
	{
		_container = new SimpleContainer();
		_container.Singleton<IWindowManager, WindowManager>();
		_container.Singleton<IEventAggregator, EventAggregator>();
		_container.PerRequest<ShellViewModel>();
	}

	protected override object GetInstance(Type service, string key)
	{
		var instance = _container.GetInstance(service, key);
		if (instance != null)
			return instance;
		throw new InvalidOperationException("Could not locate any instances.");
	}

	protected override IEnumerable<object> GetAllInstances(Type service)
	{
		return _container.GetAllInstances(service);
	}

	protected override void BuildUp(object instance)
	{
		_container.BuildUp(instance);
	}

	protected override void OnStartup(object sender, StartupEventArgs e)
	{
		DisplayRootViewFor<ShellViewModel>();
	}
3. In ShellViewModel.cs:
   ShellViewModel inherits from Conductor<object>, IScreen // Everything inherits from object
   its constructor should looks like this. Make sure to pass IEventAggregator as a param
   
   private readonly IEventAggregator _eventAggregator;
   public ShellViewModel(IEventAggregator eventAggregator)
	{
		_eventAggregator = eventAggregator;
		_eventAggregator.Subscribe(this);
	}

	if binding to any classes add similar lines as:
	private BindableCollection<Person> _people = new BindableCollection<Person>()
	When instantiating new ViewModel class make sure to pass the local var _eventAggregator to its constructor.
	
4. Event aggregation - Receive messages from view models (Subscribe)
   For a view model (VM) to retrieve a message, pass the eventAggregator to the VM's constructor
   Then make sure that the VM inherits from Screen, IHandle<<Message class type...>> and in it's constructor, 
   the event aggregator subscribes to "this" :
   Example:
   public class VM : Screen, IHandle<EventMessaging>
   public VM(IEventAggregator eventAggregator)
	{
		eventAggregator.Subscribe(this);
	}
	// Property for UI Binding such as a textbox, etc. 
	The view would look like this:
	"  <TextBlock Text="{Binding Path=DisplayMessage, Mode=OneWay}" Grid.Row="1" Grid.Column="1" Grid.ColumnSpan="2"></TextBlock>"
	public string DisplayMessage
		{
			get { return _displayMessage; }
			set
			{
				_displayMessage = value;
				NotifyOfPropertyChange(() => DisplayMessage);
			}
		}
	The VM class needs to have a handle method:
	public void Handle(EventMessaging message)
	{
		DisplayMessage = message.Text;
	}

	The incomming message from "Handle" here sets the "DisplayMessage" property 
	and since the textblock is bound to the property it will display the message because of the "NotifyOfPropertyChange" call. 
5. Event aggregation - Send messages from view models (Publish)
   The publishing view model inherits from screen and sets itself to the event aggregator within it's constructor.  would make a publish call from its constructor:
   Example:
	private readonly IEventAggregator _eventAggregator;
	public OtherViewModel(IEventAggregator eventAggregator)
	{
		this._eventAggregator = eventAggregator;
	}

	To send message:
	In this case, the view has a button tied to the method "SendMessage" 
	<Button x:Name="SendMessage" Grid.Row="2" Grid.Column="1">Send Message</Button>
	The method builds the message and using it's "_eventAggregator" var,
	calls PublishOnUIThread(em) with the message type as a param. 
	The subscribe and publish objects need to be of the same type: 
	example:
	subscribe(objA) receives PublishOnUIThread(objA)

	public void SendMessage()
	{
		var em = new EventMessaging()
		{
			Text = "Hell Yeah!"
		};

		_eventAggregator.PublishOnUIThread(em);
	}
