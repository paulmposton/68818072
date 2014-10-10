using System.Windows;
using System.Windows.Controls;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for GetCase.xaml
    /// </summary>
    public partial class GetCase : UserControl
    {
        private int _maxEmptyCases;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCase" /> class.
        /// </summary>
        public GetCase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Free Case EventHandler
        /// </summary>
        /// <param name="value">The value.</param>
        public delegate void FreeCaseEventHandler(int value);

        /// <summary>
        /// Gets or sets the on done button clicked.
        /// </summary>
        /// <value>
        /// The on done button clicked.
        /// </value>
        public event FreeCaseEventHandler OnDoneButtonClicked;

        /// <summary>
        /// Gets or sets the free case.
        /// </summary>
        /// <value>
        /// The free case.
        /// </value>
        public int FreeCases { get; set; }

        /// <summary>
        /// Handles the Click event of the DoneButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDoneButtonClicked != null)
            {
                OnDoneButtonClicked.Invoke(FreeCases);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the FreeCaseTextbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void FreeCaseTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                OnFreeCasesChanged();
            }
        }

        /// <summary>
        /// Handles the Click event of the FreeCasePlus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FreeCasePlus_Click(object sender, RoutedEventArgs e)
        {
            FreeCases++;

            FreeCaseTextbox.Text = FreeCases.ToString();
        }

        /// <summary>
        /// Handles the Click event of the FreeCaseMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FreeCaseMinus_Click(object sender, RoutedEventArgs e)
        {
            if (FreeCases > 0)
            {
                FreeCases--;
                FreeCaseTextbox.Text = FreeCases.ToString();
            }
        }

        /// <summary>
        /// Called when [free cases changed].
        /// </summary>
        private void OnFreeCasesChanged()
        {
            bool isReachedMax = FreeCases == _maxEmptyCases;
            int emptyCasesRemaining = 0;

            if (isReachedMax)
            {
                LimitedEmptyPackagesTextBlock.Visibility = Visibility.Visible;
                FreeCasePlus.IsEnabled = false;
            }
            else
            {
                LimitedEmptyPackagesTextBlock.Visibility = Visibility.Collapsed;
                FreeCasePlus.IsEnabled = true;
            }

            if (FreeCases > 0)
            {
                FreeCaseMinus.IsEnabled = true;
            }
            else
            {
                FreeCaseMinus.IsEnabled = false;
            }

            if (BaseController.LoggedOnUser != null)
            {
                emptyCasesRemaining = BaseController.LoggedOnUser.FreeCasesRemaining;
            }

            if (emptyCasesRemaining == _maxEmptyCases)
            {
                //LimitedEmptyPackagesTextBlock.Text = Constants.Messages.LimittedEmptyPackages;
                LimitedEmptyPackagesTextBlock.Text = string.Format(Constants.Messages.LimittedEmptyPackages, emptyCasesRemaining);
            }
            else
            {
                LimitedEmptyPackagesTextBlock.Text = Constants.Messages.LimittedInventory;
            }

            GetCaseMessage.Text = string.Format(Constants.Messages.GetCaseMessage, FreeCases, _maxEmptyCases);
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load(int maxEmptyCases)
        {
            _maxEmptyCases = maxEmptyCases;
            FreeCases = 0;
            FreeCaseTextbox.Text = "0";
            OnFreeCasesChanged();
        }
    }
}