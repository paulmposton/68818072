using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Bettery.Kiosk.Common;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for KeyBoard.xaml
    /// </summary>
    public partial class KeyBoard : UserControl
    {
        private bool _isSymbolMode = false;
        private bool _isShiftUp = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBoard"/> class.
        /// </summary>
        public KeyBoard()
        {
            InitializeComponent();
        }

        public delegate void CapsLockEventHandler(bool isCapsLock);

        /// <summary>
        /// Occurs when [on caps lock].
        /// </summary>
        public event CapsLockEventHandler OnCapsLock;

        /// <summary>
        /// Occurs when [on text button clicked].
        /// </summary>
        public event EventHandler<TextButtonClickedEventArgs> OnTextButtonClicked;

        /// <summary>
        /// Occurs when [on back button clicked].
        /// </summary>
        public event EventHandler OnBackButtonClicked;

        /// <summary>
        /// Occurs when [on enter button clicked].
        /// </summary>
        public event EventHandler OnEnterButtonClicked;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is caps lock.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is caps lock; otherwise, <c>false</c>.
        /// </value>
        public bool IsCapsLock { get; set; }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            if (_isShiftUp)
            {
                UpButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }

            if (_isSymbolMode)
            {
                SymbolButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        /// <summary>
        /// Handles the Click event of the Keyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            string character = Utils.ToString(((Button)sender).Content);
            RaiseOnTextButtonClicked(character);
        }

        /// <summary>
        /// Handles the Click event of the UpButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isShiftUp)
            {
                QButton.Content = Utils.ToLowerString(QButton.Content);
                WButton.Content = Utils.ToLowerString(WButton.Content);
                EButton.Content = Utils.ToLowerString(EButton.Content);
                RButton.Content = Utils.ToLowerString(RButton.Content);
                TButton.Content = Utils.ToLowerString(TButton.Content);
                YButton.Content = Utils.ToLowerString(YButton.Content);
                UButton.Content = Utils.ToLowerString(UButton.Content);
                IButton.Content = Utils.ToLowerString(IButton.Content);
                OButton.Content = Utils.ToLowerString(OButton.Content);
                PButton.Content = Utils.ToLowerString(PButton.Content);
                AButton.Content = Utils.ToLowerString(AButton.Content);
                SButton.Content = Utils.ToLowerString(SButton.Content);
                DButton.Content = Utils.ToLowerString(DButton.Content);
                FButton.Content = Utils.ToLowerString(FButton.Content);
                GButton.Content = Utils.ToLowerString(GButton.Content);
                HButton.Content = Utils.ToLowerString(HButton.Content);
                JButton.Content = Utils.ToLowerString(JButton.Content);
                KButton.Content = Utils.ToLowerString(KButton.Content);
                LButton.Content = Utils.ToLowerString(LButton.Content);
                ZButton.Content = Utils.ToLowerString(ZButton.Content);
                XButton.Content = Utils.ToLowerString(XButton.Content);
                CButton.Content = Utils.ToLowerString(CButton.Content);
                VButton.Content = Utils.ToLowerString(VButton.Content);
                BButton.Content = Utils.ToLowerString(BButton.Content);
                NButton.Content = Utils.ToLowerString(NButton.Content);
                MButton.Content = Utils.ToLowerString(MButton.Content);

                _isShiftUp = false;
                UpButton.Content = "5";
                IsCapsLock = false;
            }
            else
            {
                QButton.Content = Utils.ToUpperString(QButton.Content);
                WButton.Content = Utils.ToUpperString(WButton.Content);
                EButton.Content = Utils.ToUpperString(EButton.Content);
                RButton.Content = Utils.ToUpperString(RButton.Content);
                TButton.Content = Utils.ToUpperString(TButton.Content);
                YButton.Content = Utils.ToUpperString(YButton.Content);
                UButton.Content = Utils.ToUpperString(UButton.Content);
                IButton.Content = Utils.ToUpperString(IButton.Content);
                OButton.Content = Utils.ToUpperString(OButton.Content);
                PButton.Content = Utils.ToUpperString(PButton.Content);
                AButton.Content = Utils.ToUpperString(AButton.Content);
                SButton.Content = Utils.ToUpperString(SButton.Content);
                DButton.Content = Utils.ToUpperString(DButton.Content);
                FButton.Content = Utils.ToUpperString(FButton.Content);
                GButton.Content = Utils.ToUpperString(GButton.Content);
                HButton.Content = Utils.ToUpperString(HButton.Content);
                JButton.Content = Utils.ToUpperString(JButton.Content);
                KButton.Content = Utils.ToUpperString(KButton.Content);
                LButton.Content = Utils.ToUpperString(LButton.Content);
                ZButton.Content = Utils.ToUpperString(ZButton.Content);
                XButton.Content = Utils.ToUpperString(XButton.Content);
                CButton.Content = Utils.ToUpperString(CButton.Content);
                VButton.Content = Utils.ToUpperString(VButton.Content);
                BButton.Content = Utils.ToUpperString(BButton.Content);
                NButton.Content = Utils.ToUpperString(NButton.Content);
                MButton.Content = Utils.ToUpperString(MButton.Content);

                _isShiftUp = true;
                UpButton.Content = "6";
                IsCapsLock = true;
            }

            if (OnCapsLock != null)
            {
                OnCapsLock.Invoke(IsCapsLock);
            }
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOnBackButtonClicked();
        }

        /// <summary>
        /// Handles the Click event of the EnterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOnEnterButtonClicked();
        }

        /// <summary>
        /// Handles the Click event of the SpaceButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SpaceButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOnTextButtonClicked(" ");
        }

        /// <summary>
        /// Handles the Click event of the SymbolButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SymbolButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isSymbolMode)
            {
                FirstCharStackPanel.Visibility = Visibility.Collapsed;
                FirstSymbolsStackPanel.Visibility = Visibility.Visible;
                SecondCharStackPanel.Visibility = Visibility.Collapsed;
                SecondSymbolsStackPanel.Visibility = Visibility.Visible;
                ThirdCharStackPanel.Visibility = Visibility.Collapsed;
                ThirdSymbolsStackPanel.Visibility = Visibility.Visible;

                _isSymbolMode = true;
                SymbolButton.Content = "abc";
            }
            else
            {
                FirstSymbolsStackPanel.Visibility = Visibility.Collapsed;
                FirstCharStackPanel.Visibility = Visibility.Visible;
                SecondSymbolsStackPanel.Visibility = Visibility.Collapsed;
                SecondCharStackPanel.Visibility = Visibility.Visible;
                ThirdSymbolsStackPanel.Visibility = Visibility.Collapsed;
                ThirdCharStackPanel.Visibility = Visibility.Visible;

                _isSymbolMode = false;
                SymbolButton.Content = "?#+";
            }
        }

        /// <summary>
        /// Raises the on text button clicked.
        /// </summary>
        /// <param name="character">The character.</param>
        private void RaiseOnTextButtonClicked(string character)
        {
            EventHandler<TextButtonClickedEventArgs> handler = OnTextButtonClicked;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, new TextButtonClickedEventArgs(character)); }));
            }
        }

        /// <summary>
        /// Raises the on back button clicked.
        /// </summary>
        private void RaiseOnBackButtonClicked()
        {
            EventHandler handler = OnBackButtonClicked;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// Raises the on enter button clicked.
        /// </summary>
        private void RaiseOnEnterButtonClicked()
        {
            EventHandler handler = OnEnterButtonClicked;
            if (handler != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { handler(this, EventArgs.Empty); }));
            }
        }

        /// <summary>
        /// EventArgs for TextButtonClickedEvent
        /// </summary>
        public class TextButtonClickedEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the character.
            /// </summary>
            /// <value>
            /// The character.
            /// </value>
            public string Character { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="TextButtonClickedEventArgs"/> class.
            /// </summary>
            /// <param name="character">The character.</param>
            public TextButtonClickedEventArgs(string character)
            {
                Character = character;
            }
        }
    }
}