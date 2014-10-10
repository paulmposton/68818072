using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Bettery.Kiosk.Common
{
    /// <summary>
    /// Class UI Helper
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Sends the input.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="text">The text.</param>
        public static void SendInput(UIElement element, string text)
        {
            if (element != null)
            {
                InputManager inputManager = InputManager.Current;
                InputDevice inputDevice = inputManager.PrimaryKeyboardDevice;
                TextComposition composition = new TextComposition(inputManager, element, text);
                TextCompositionEventArgs args = new TextCompositionEventArgs(inputDevice, composition);
                args.RoutedEvent = UIElement.PreviewTextInputEvent;
                element.RaiseEvent(args);
                args.RoutedEvent = UIElement.TextInputEvent;
                element.RaiseEvent(args);
            }
        }

        /// <summary>
        /// Raises the key event.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="key">The key.</param>
        public static void RaiseKeyEvent(UIElement element, Key key)
        {
            if (element != null)
            {
                KeyboardDevice keyboardDevice = Keyboard.PrimaryDevice;
                PresentationSource presentationSource = PresentationSource.FromVisual(element);
                int timestamp = Environment.TickCount;
                if (presentationSource != null)
                {
                    KeyEventArgs args = new KeyEventArgs(keyboardDevice, presentationSource, timestamp, key);

                    args.RoutedEvent = Keyboard.PreviewKeyDownEvent;
                    element.RaiseEvent(args);

                    args.RoutedEvent = Keyboard.KeyDownEvent;
                    element.RaiseEvent(args);

                    args.RoutedEvent = Keyboard.PreviewKeyUpEvent;
                    element.RaiseEvent(args);

                    args.RoutedEvent = Keyboard.KeyUpEvent;
                    element.RaiseEvent(args);
                }
            }
        }

        /// <summary>
        ///   UI Thread dispatcher
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "control"></param>
        /// <param name = "action"></param>
        public static void DispatchThread<T>(T control, Action<T> action)
        {
            DependencyObject dependencyObject = control as DependencyObject;

            if (dependencyObject != null)
            {
                dependencyObject.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action, control);
            }
        }

        /// <summary>
        /// Delayeds the focus.  This is to fix issue with textbox keep its focus.
        /// </summary>
        /// <param name="source">The source.</param>
        public static void DelayedFocus(this UIElement source)
        {
            if (source != null)
            {
                source.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate
                                                                                       {
                                                                                           source.Focus();
                                                                                       }));
            }
        }
    }
}