using System;
using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;
using Bettery.Kiosk.Entities;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for Recycle.xaml
    /// </summary>
    public partial class Recycle : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Recycle" /> class.
        /// </summary>
        public Recycle()
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
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            ResetMedia();
        }
        /// <summary>
        /// Handles the MediaEnded event of the InsertPackMedia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void RecycleMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            ResetMedia();
        }

        /// <summary>
        /// Resets the recycle media.
        /// </summary>
        public void ResetMedia()
        {
            RecycleMedia.Position = TimeSpan.Zero;
            RecycleMedia.LoadedBehavior = MediaState.Play;
        }
    }
}