using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Exchange.xaml
    /// </summary>
    public partial class Exchange : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Exchange" /> class.
        /// </summary>
        public Exchange()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when [on done button clicked].
        /// </summary>
        public event EventHandler OnDoneButtonClicked;

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            EventHandler handler = OnDoneButtonClicked;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(sender, e); }));
            }
        }

        /// <summary>
        /// Handles the Click event of the ReturnButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaseController.CartridgeInserted)
            {
                // For now, this accepts input from the scanner and increments aa batteries if the phidget Rollball detected a cartridge was inserted
                if (BaseController.SelectedBettery == null)
                {
                    BaseController.SelectedBettery = new BetteryVend();
                }

                BaseController.SelectedBettery.AaReturn++;
                BaseController.CartridgeInserted = true;
            }

            ShowBatteriesCount();
        }

        /// <summary>
        /// Shows the batteries count.
        /// </summary>
        private void ShowBatteriesCount()
        {
            int aaCount = 0;

            if (BaseController.SelectedBettery != null)
            {
                aaCount = BaseController.SelectedBettery.AaReturn;
            }

            BatteriesCountTextBlock.Text = string.Format(Constants.Messages.BetteriesReturned, aaCount);
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ResetMedia();
            ShowBatteriesCount();

            ///UPCCode.DelayedFocus();  CK- no more UPC code required - delete this line
        }
        /// <summary>
        /// Handles the MediaEnded event of the InsertPackMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void InsertPackMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ResetMedia();
        }

        /// <summary>
        /// Resets the vending media.
        /// </summary>
        public void ResetMedia()
        {
            InsertPackMedia.Position = TimeSpan.Zero;
            InsertPackMedia.LoadedBehavior = MediaState.Play;
        }
    }
}