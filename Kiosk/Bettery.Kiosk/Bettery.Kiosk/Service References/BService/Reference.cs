﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bettery.Kiosk.BService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BetteryMember", Namespace="http://schemas.datacontract.org/2004/07/WebRole1.HelperClasses")]
    [System.SerializableAttribute()]
    public partial class BetteryMember : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AccountBalanceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int BatteryPacksCheckedOutField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int BatteryPacksInPlanField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CCExPireDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CCLastFourDigitsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerProfileIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int FreeCasesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int GroupIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MemberFirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MemberIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MemberLastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MemberTotalBatteriesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PaymentProfileIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal AccountBalance {
            get {
                return this.AccountBalanceField;
            }
            set {
                if ((this.AccountBalanceField.Equals(value) != true)) {
                    this.AccountBalanceField = value;
                    this.RaisePropertyChanged("AccountBalance");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BatteryPacksCheckedOut {
            get {
                return this.BatteryPacksCheckedOutField;
            }
            set {
                if ((this.BatteryPacksCheckedOutField.Equals(value) != true)) {
                    this.BatteryPacksCheckedOutField = value;
                    this.RaisePropertyChanged("BatteryPacksCheckedOut");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BatteryPacksInPlan {
            get {
                return this.BatteryPacksInPlanField;
            }
            set {
                if ((this.BatteryPacksInPlanField.Equals(value) != true)) {
                    this.BatteryPacksInPlanField = value;
                    this.RaisePropertyChanged("BatteryPacksInPlan");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CCExPireDate {
            get {
                return this.CCExPireDateField;
            }
            set {
                if ((object.ReferenceEquals(this.CCExPireDateField, value) != true)) {
                    this.CCExPireDateField = value;
                    this.RaisePropertyChanged("CCExPireDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CCLastFourDigits {
            get {
                return this.CCLastFourDigitsField;
            }
            set {
                if ((object.ReferenceEquals(this.CCLastFourDigitsField, value) != true)) {
                    this.CCLastFourDigitsField = value;
                    this.RaisePropertyChanged("CCLastFourDigits");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomerProfileID {
            get {
                return this.CustomerProfileIDField;
            }
            set {
                if ((object.ReferenceEquals(this.CustomerProfileIDField, value) != true)) {
                    this.CustomerProfileIDField = value;
                    this.RaisePropertyChanged("CustomerProfileID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int FreeCases {
            get {
                return this.FreeCasesField;
            }
            set {
                if ((this.FreeCasesField.Equals(value) != true)) {
                    this.FreeCasesField = value;
                    this.RaisePropertyChanged("FreeCases");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int GroupID {
            get {
                return this.GroupIDField;
            }
            set {
                if ((this.GroupIDField.Equals(value) != true)) {
                    this.GroupIDField = value;
                    this.RaisePropertyChanged("GroupID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MemberFirstName {
            get {
                return this.MemberFirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MemberFirstNameField, value) != true)) {
                    this.MemberFirstNameField = value;
                    this.RaisePropertyChanged("MemberFirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MemberID {
            get {
                return this.MemberIDField;
            }
            set {
                if ((this.MemberIDField.Equals(value) != true)) {
                    this.MemberIDField = value;
                    this.RaisePropertyChanged("MemberID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MemberLastName {
            get {
                return this.MemberLastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MemberLastNameField, value) != true)) {
                    this.MemberLastNameField = value;
                    this.RaisePropertyChanged("MemberLastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MemberTotalBatteries {
            get {
                return this.MemberTotalBatteriesField;
            }
            set {
                if ((this.MemberTotalBatteriesField.Equals(value) != true)) {
                    this.MemberTotalBatteriesField = value;
                    this.RaisePropertyChanged("MemberTotalBatteries");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PaymentProfileID {
            get {
                return this.PaymentProfileIDField;
            }
            set {
                if ((object.ReferenceEquals(this.PaymentProfileIDField, value) != true)) {
                    this.PaymentProfileIDField = value;
                    this.RaisePropertyChanged("PaymentProfileID");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Promo", Namespace="http://schemas.datacontract.org/2004/07/WebRole1.HelperClasses")]
    [System.SerializableAttribute()]
    public partial class Promo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PromoTypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Amount {
            get {
                return this.AmountField;
            }
            set {
                if ((this.AmountField.Equals(value) != true)) {
                    this.AmountField = value;
                    this.RaisePropertyChanged("Amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PromoType {
            get {
                return this.PromoTypeField;
            }
            set {
                if ((this.PromoTypeField.Equals(value) != true)) {
                    this.PromoTypeField = value;
                    this.RaisePropertyChanged("PromoType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BService.IKioskService")]
    public interface IKioskService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/AuthenticateUser", ReplyAction="http://tempuri.org/IKioskService/AuthenticateUserResponse")]
        Bettery.Kiosk.BService.BetteryMember AuthenticateUser(string UserName, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/AddBetteryMember", ReplyAction="http://tempuri.org/IKioskService/AddBetteryMemberResponse")]
        bool AddBetteryMember(string FirstName, string LastName, string Email, string Password, string Zipcode, bool GetEmails, int SubscriptionPlan);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/UpdateMembership", ReplyAction="http://tempuri.org/IKioskService/UpdateMembershipResponse")]
        void UpdateMembership(int MemberID, string CustomerProfileID, string PaymentProfileID, int BatteriesInPlan, decimal NewAccountBalance, bool isUpgrade);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/UpdateProfile", ReplyAction="http://tempuri.org/IKioskService/UpdateProfileResponse")]
        void UpdateProfile(int MemberID, string CustomerProfileID, string PaymentProfileID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/KioskPing", ReplyAction="http://tempuri.org/IKioskService/KioskPingResponse")]
        bool KioskPing();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/CheckEmail", ReplyAction="http://tempuri.org/IKioskService/CheckEmailResponse")]
        bool CheckEmail(string EmailAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/GetPromoCredit", ReplyAction="http://tempuri.org/IKioskService/GetPromoCreditResponse")]
        Bettery.Kiosk.BService.Promo GetPromoCredit(string PromoCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/RedeemPromoCode", ReplyAction="http://tempuri.org/IKioskService/RedeemPromoCodeResponse")]
        void RedeemPromoCode(string PromoCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/ReturnBinFullAlert", ReplyAction="http://tempuri.org/IKioskService/ReturnBinFullAlertResponse")]
        void ReturnBinFullAlert(string KioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/ProductInventoryAlert", ReplyAction="http://tempuri.org/IKioskService/ProductInventoryAlertResponse")]
        void ProductInventoryAlert(string KioskID, int ProductID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/AuthorizeDotNetAlert", ReplyAction="http://tempuri.org/IKioskService/AuthorizeDotNetAlertResponse")]
        void AuthorizeDotNetAlert(string KioskID, string ErrorMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/TransactionFailureAlert", ReplyAction="http://tempuri.org/IKioskService/TransactionFailureAlertResponse")]
        void TransactionFailureAlert(string KioskID, string ErrorMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKioskService/EmptyCaseVend", ReplyAction="http://tempuri.org/IKioskService/EmptyCaseVendResponse")]
        void EmptyCaseVend(int MemberID, int EmptyCases);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IKioskServiceChannel : Bettery.Kiosk.BService.IKioskService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KioskServiceClient : System.ServiceModel.ClientBase<Bettery.Kiosk.BService.IKioskService>, Bettery.Kiosk.BService.IKioskService {
        
        public KioskServiceClient() {
        }
        
        public KioskServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KioskServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KioskServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KioskServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Bettery.Kiosk.BService.BetteryMember AuthenticateUser(string UserName, string Password) {
            return base.Channel.AuthenticateUser(UserName, Password);
        }
        
        public bool AddBetteryMember(string FirstName, string LastName, string Email, string Password, string Zipcode, bool GetEmails, int SubscriptionPlan) {
            return base.Channel.AddBetteryMember(FirstName, LastName, Email, Password, Zipcode, GetEmails, SubscriptionPlan);
        }
        
        public void UpdateMembership(int MemberID, string CustomerProfileID, string PaymentProfileID, int BatteriesInPlan, decimal NewAccountBalance, bool isUpgrade) {
            base.Channel.UpdateMembership(MemberID, CustomerProfileID, PaymentProfileID, BatteriesInPlan, NewAccountBalance, isUpgrade);
        }
        
        public void UpdateProfile(int MemberID, string CustomerProfileID, string PaymentProfileID) {
            base.Channel.UpdateProfile(MemberID, CustomerProfileID, PaymentProfileID);
        }
        
        public bool KioskPing() {
            return base.Channel.KioskPing();
        }
        
        public bool CheckEmail(string EmailAddress) {
            return base.Channel.CheckEmail(EmailAddress);
        }
        
        public Bettery.Kiosk.BService.Promo GetPromoCredit(string PromoCode) {
            return base.Channel.GetPromoCredit(PromoCode);
        }
        
        public void RedeemPromoCode(string PromoCode) {
            base.Channel.RedeemPromoCode(PromoCode);
        }
        
        public void ReturnBinFullAlert(string KioskID) {
            base.Channel.ReturnBinFullAlert(KioskID);
        }
        
        public void ProductInventoryAlert(string KioskID, int ProductID) {
            base.Channel.ProductInventoryAlert(KioskID, ProductID);
        }
        
        public void AuthorizeDotNetAlert(string KioskID, string ErrorMessage) {
            base.Channel.AuthorizeDotNetAlert(KioskID, ErrorMessage);
        }
        
        public void TransactionFailureAlert(string KioskID, string ErrorMessage) {
            base.Channel.TransactionFailureAlert(KioskID, ErrorMessage);
        }
        
        public void EmptyCaseVend(int MemberID, int EmptyCases) {
            base.Channel.EmptyCaseVend(MemberID, EmptyCases);
        }
    }
}
