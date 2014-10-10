using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bettery.Kiosk.Common;
using Bettery.Kiosk.DataAccess;
using Bettery.Kiosk.Entities;
using Bettery.Kiosk.Controllers;

namespace Bettery.Kiosk.UserControls
{
    /// <summary>
    /// Interaction logic for InventoryAdmin.xaml
    /// </summary>
    public partial class InventoryAdmin : UserControl
    {
        private UIElement _currentControl;

        public InventoryAdmin()
        {
            InitializeComponent();

            InventoryKeyboard.OnTextButtonClicked += InventoryKeyboard_OnTextButtonClicked;
        }

        /// <summary>
        /// Occurs when [on continue transaction clicked].
        /// </summary>
        public event EventHandler OnDoneButton_Clicked;
        
        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (OnCancelButton_Clicked != null)
        //    {
        //        OnCancelButton_Clicked.Invoke(sender, e);
        //    }
        //}

        private void BinInventory_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            #region Save inventory updates
            ///int i = 0;
            bool validated = true;
            //if (!int.TryParse(Bin16.ToString(), out i))
            //{
            //    Bin16.Background = Brushes.Red;
            //    Bin16.DelayedFocus();
            //    validated = false;
            //}
            if (validated)
            {
                if (string.IsNullOrEmpty(Bin16.Text))
                    Bin16.Text = "0";
                if (string.IsNullOrEmpty(Bin17.Text))
                    Bin17.Text = "0";
                if (string.IsNullOrEmpty(Bin18.Text))
                    Bin18.Text = "0";
                if (string.IsNullOrEmpty(Bin20.Text))
                    Bin20.Text = "0";
                if (string.IsNullOrEmpty(Bin21.Text))
                    Bin21.Text = "0";
                if (string.IsNullOrEmpty(Bin22.Text))
                    Bin22.Text = "0";
                if (string.IsNullOrEmpty(Bin23.Text))
                    Bin23.Text = "0";
                if (string.IsNullOrEmpty(Bin24.Text))
                    Bin24.Text = "0";
                if (string.IsNullOrEmpty(Bin25.Text))
                    Bin25.Text = "0";
                if (string.IsNullOrEmpty(Bin26.Text))
                    Bin26.Text = "0";
                if (string.IsNullOrEmpty(Bin27.Text))
                    Bin27.Text = "0";
                if (string.IsNullOrEmpty(Bin28.Text))
                    Bin28.Text = "0";


                BaseDAL.UpdateInventory(16, Int32.Parse(Bin16.Text), GetProductType(Bin16Type.Content.ToString()));
                BaseDAL.UpdateInventory(17, Int32.Parse(Bin17.Text), GetProductType(Bin17Type.Content.ToString()));
                BaseDAL.UpdateInventory(18, Int32.Parse(Bin18.Text), GetProductType(Bin18Type.Content.ToString()));
                BaseDAL.UpdateInventory(20, Int32.Parse(Bin20.Text), GetProductType(Bin20Type.Content.ToString()));
                BaseDAL.UpdateInventory(21, Int32.Parse(Bin21.Text), GetProductType(Bin21Type.Content.ToString()));
                BaseDAL.UpdateInventory(22, Int32.Parse(Bin22.Text), GetProductType(Bin22Type.Content.ToString()));
                BaseDAL.UpdateInventory(23, Int32.Parse(Bin23.Text), GetProductType(Bin23Type.Content.ToString()));
                BaseDAL.UpdateInventory(24, Int32.Parse(Bin24.Text), GetProductType(Bin24Type.Content.ToString()));
                BaseDAL.UpdateInventory(25, Int32.Parse(Bin25.Text), GetProductType(Bin25Type.Content.ToString()));
                BaseDAL.UpdateInventory(26, Int32.Parse(Bin26.Text), GetProductType(Bin26Type.Content.ToString()));
                BaseDAL.UpdateInventory(27, Int32.Parse(Bin27.Text), GetProductType(Bin27Type.Content.ToString()));
                BaseDAL.UpdateInventory(28, Int32.Parse(Bin28.Text), GetProductType(Bin28Type.Content.ToString()));
                if (OnDoneButton_Clicked != null)
                {
                    OnDoneButton_Clicked.Invoke(sender, e);
                }
            }
            #endregion
        }
        private int GetProductType(string Description)
        {
            switch (Description)
                {
                    case "AAA":
                        return Constants.BetteryProduct.AaaID;
                    case "AA":
                        return Constants.BetteryProduct.AaID;
                    case "CASE":
                        return Constants.BetteryProduct.CaseID;
                }
            return 0;

        }
        private string GetProductType(int ProductType)
        {
            switch (ProductType)
            {
                case 1:
                    return Constants.BetteryProduct.AaaDesc;
                case 2:
                    return Constants.BetteryProduct.AaDesc;
                case 3:
                    return Constants.BetteryProduct.CaseDesc;
            }
            return string.Empty;

        }

        private void InventoryTypeButton_Click(object sender, RoutedEventArgs e)
        {
            //
            // Only allow SuperUsers to change the inventory setup.   Service Techs not allowed to do this.
            //
            if (BaseController.LoggedOnUser.GroupID == Constants.Group.SuperUser)
            {
                if (((Button)sender).Content == "AA")
                    ((Button)sender).Content = "AAA";
                else if (((Button)sender).Content == "AAA")
                    ((Button)sender).Content = "CASE";
                else
                    ((Button)sender).Content = "AA";
            }
        }
        /// <summary>
        /// Handles the OnTextButtonClicked event of the InventoryKeyboard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyBoard.TextButtonClickedEventArgs" /> instance containing the event data.</param>
        protected void InventoryKeyboard_OnTextButtonClicked(object sender, KeyBoard.TextButtonClickedEventArgs e)
        {
            if (e.Character == "BACKSPACE")
            {

                if (((TextBox)_currentControl).Text.Length > 0)
                {
                    ((TextBox)_currentControl).Text = ((TextBox)_currentControl).Text.Substring(0, ((TextBox)_currentControl).Text.Length - 1);
                    ((TextBox)_currentControl).Select(((TextBox)_currentControl).Text.Length, 0);
                }
                

            }
            else
            {
                ((TextBox)_currentControl).Foreground = Brushes.Black;
                UIHelper.SendInput(_currentControl, e.Character);
            }


        }

        /// <summary>
        /// Handles the GotFocus event of the Password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void BinGotFocus(object sender, RoutedEventArgs e)
        {
            
            _currentControl = (UIElement)sender;
            
        }




        public void Load()
        {
            #region Load Bins
            BinProduct binProduct = new BinProduct(); 

            binProduct = BaseDAL.GetInventorybyBin(16);
            Bin16.Text = binProduct.Quantity.ToString();
            Bin16Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin16.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(17);
            Bin17.Text = binProduct.Quantity.ToString();
            Bin17Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin17.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(18);
            Bin18.Text = binProduct.Quantity.ToString();
            Bin18Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin18.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(20);
            Bin20.Text = binProduct.Quantity.ToString();
            Bin20Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin20.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(21);
            Bin21.Text = binProduct.Quantity.ToString();
            Bin21Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin21.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(22);
            Bin22.Text = binProduct.Quantity.ToString();
            Bin22Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin22.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(23);
            Bin23.Text = binProduct.Quantity.ToString();
            Bin23Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin23.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(24);
            Bin24.Text = binProduct.Quantity.ToString();
            Bin24Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin24.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(25);
            Bin25.Text = binProduct.Quantity.ToString();
            Bin25Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin25.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(26);
            Bin26.Text = binProduct.Quantity.ToString();
            Bin26Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin26.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(27);
            Bin27.Text = binProduct.Quantity.ToString();
            Bin27Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin27.Foreground = Brushes.Red;

            binProduct = BaseDAL.GetInventorybyBin(28);
            Bin28.Text = binProduct.Quantity.ToString();
            Bin28Type.Content = GetProductType(binProduct.ProductID);
            if (!binProduct.Enabled)
                Bin28.Foreground = Brushes.Red;

            #endregion

            Bin16.DelayedFocus();

        }

        private void FullButton_Click(object sender, RoutedEventArgs e)
        {
            ((TextBox)_currentControl).Text = "";
            ((TextBox)_currentControl).Foreground = Brushes.Black;
            UIHelper.SendInput(_currentControl, Constants.BetteryProduct.BinFullMax.ToString());
            
        }

    }
}
