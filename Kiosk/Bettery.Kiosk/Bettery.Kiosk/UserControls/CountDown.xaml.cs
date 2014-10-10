using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Xceed.Wpf.Toolkit;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for CountDown.xaml
    /// </summary>
    public partial class CountDown : UserControl
    {
        private readonly Timer _timer = new Timer();
        private readonly Timer _timerCountDown = new Timer();
        
        private Control _currentControlFocus;
        private BusyIndicator _busyIndicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountDown" /> class.
        /// </summary>
        public CountDown()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on count down end].
        /// </summary>
        public event EventHandler OnCountDownEnd;

        /// <summary>
        /// Occurs when [on yes clicked].
        /// </summary>
        public event EventHandler OnYesClicked;

        /// <summary>
        /// Occurs when [on no clicked].
        /// </summary>
        public event EventHandler OnNoClicked;

        /// <summary>
        /// Gets or sets the total time of count down.
        /// </summary>
        /// <value>
        /// The total time of count down.
        /// </value>
        public int PopupTimeOut { get; set; }

        /// <summary>
        /// Gets or sets number of times a priv users timer has elapsed
        /// </summary>
        /// <value>
        /// The number of times a priv users timer has elapsed
        /// </value>

        private int PrivUserTimerCount { get; set; }

        /// <summary>
        /// Gets or sets the current time of popup time out in count down.
        /// </summary>
        /// <value>
        /// The current time of popup time out in count down.
        /// </value>
        public int CurrentPopupTimeOut { get; set; }

        /// <summary>
        /// Loads the specified max idle time.
        /// </summary>
        /// <param name="busyIndicator">The busy indicator.</param>
        /// <param name="maxIdleTime">The max idle time.</param>
        /// <param name="popupTimeOut">The popupTimeOut.</param>
        public void Load(BusyIndicator busyIndicator, int maxIdleTime, int popupTimeOut)
        {
            PrivUserTimerCount = 0;
            _busyIndicator = busyIndicator;
            PopupTimeOut = popupTimeOut;
            
            int timerInterval = (maxIdleTime - popupTimeOut) * 1000;
            if (timerInterval > 0)
            {
                _timer.Interval = timerInterval;
                _timerCountDown.Interval = 1000;
                _timer.Elapsed += Timer_Elapsed;
                _timerCountDown.Elapsed += TimerCountDown_Elapsed;
            }
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs" /> instance containing the event data.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UIHelper.DispatchThread(this, main =>
                                              {
                                                  if (_busyIndicator.IsBusy)
                                                  {
                                                      ResetPopup();
                                                  }
                                                  else
                                                  {
                                                      //
                                                      //  Don't time out for service people and superusers, except for 5x longer than regular users
                                                      //
                                                      if (BaseController.LoggedOnUser != null)
                                                      {
                                                          if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser || BaseController.LoggedOnUser.GroupID == Constants.Group.SwapStationAdmin)
                                                          {
                                                              PrivUserTimerCount++;
                                                              if (PrivUserTimerCount < 5)
                                                              {
                                                                  ResetPopup();
                                                              }
                                                              else
                                                              {
                                                                  PrivUserTimerCount = 0;
                                                                  StartCountDown();
                                                              }
                                                          }
                                                          else
                                                          {
                                                              PrivUserTimerCount = 0;
                                                              StartCountDown();
                                                          }
                                                      }
                                                      else
                                                      {
                                                          PrivUserTimerCount = 0;
                                                          StartCountDown();
                                                      }
                                                  }
                                              });
        }

        /// <summary>
        /// Handles the Elapsed event of the TimerCountDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs" /> instance containing the event data.</param>
        private void TimerCountDown_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentPopupTimeOut <= 0D)
            {
                StopCountDown(true);
            }
            else
            {
                CurrentPopupTimeOut--;
                UpdateTime(CurrentPopupTimeOut);
            }
        }

        /// <summary>
        /// Runs the popup.
        /// </summary>
        public void RunPopup()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stops the popup.
        /// </summary>
        public void StopPopup()
        {
            _timer.Stop();
            StopCountDown(false);
        }

        /// <summary>
        /// Resets the popup.
        /// </summary>
        public void ResetPopup()
        {
            StopPopup();

            RunPopup();
        }

        /// <summary>
        /// Starts the count down.
        /// </summary>
        private void StartCountDown()
        {
            _currentControlFocus = BaseController.CurrentControlFocus;

            CurrentPopupTimeOut = PopupTimeOut;
            if (CountDownController.ReturnedBatteriesExists())
            {
                UpdateMessage(Constants.Messages.ReturnBatteriesMessage);
            }
            else
            {
                UpdateMessage(Constants.Messages.DefaultMessage);
            }

            UIHelper.DispatchThread(this, main => main.Visibility = Visibility.Visible);
            UpdateTime(CurrentPopupTimeOut);

            _timerCountDown.Start();
            _timer.Stop();
        }

        /// <summary>
        /// Stops the count down.
        /// </summary>
        /// <param name="raiseEvent">if set to <c>true</c> [raise event].</param>
        private void StopCountDown(bool raiseEvent)
        {
            _timerCountDown.Stop();

            UIHelper.DispatchThread(this, main => main.Visibility = Visibility.Collapsed);

            if (raiseEvent && OnCountDownEnd != null)
            {
                OnCountDownEnd.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Updates the time.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public void UpdateTime(int seconds)
        {
            UIHelper.DispatchThread(this, main => main.Time.Text = seconds.ToString());
        }

        /// <summary>
        /// Updates the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void UpdateMessage(string message)
        {
            UIHelper.DispatchThread(this, main => main.Message.Text = message);
        }

        /// <summary>
        /// Handles the Click event of the Yes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            ResetPopup();

            if (OnYesClicked != null)
            {
                OnYesClicked.Invoke(sender, e);
            }

            if (_currentControlFocus != null)
            {
                _currentControlFocus.Focus();
            }
        }

        /// <summary>
        /// Handles the Click event of the No control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void No_Click(object sender, RoutedEventArgs e)
        {
            if (OnNoClicked != null)
            {
                OnNoClicked.Invoke(sender, e);
            }
        }
    }
}