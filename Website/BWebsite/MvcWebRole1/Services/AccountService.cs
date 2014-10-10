    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bettery.WebRole.Repositories;
using Bettery.WebRole.Models;
using MvcWebRole1.Common;

namespace Bettery.WebRole.Services
{
    public class AccountService
    {
        private AccountRepository accountRepository;
        
        public AccountService()
            : this(new AccountRepository())
        {
        }

        public AccountService(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        public IEnumerable<Credit> GetCredits(int memberID)
        {
            return accountRepository.SelectCredits(memberID);
        }
        public IEnumerable<Transaction> GetTransactions(int memberID)
        {
            return accountRepository.SelectTransactions(memberID);
        }
        public TransactionDetail GetTransactionDetail(int transactionID)
        {
            return accountRepository.SelectTransactionDetail(transactionID);
        }

        public void AddCreditCard(int MemberID, CreditCard creditCard)
        {
            accountRepository.InsertCreditCard(MemberID, creditCard);
        }

        public Member GetMember(string registrationID)
        {
            return accountRepository.SelectMember(registrationID);
        }

        public Member GetMember(int memberID)
        {
            return accountRepository.SelectMember(memberID);
        }
        public EditMember GeEdittMember(int memberID)
        {
            return accountRepository.SelectEditMember(memberID);
        }
        public void UpdateMember(EditMember editMember)
        {

            accountRepository.UpdateMember(editMember);
        }
        public void UpdatePassword(ChangePasswordModel changePasswordModel)
        {
            // We use the default SHA-256 & 4 byte length
            HashUtils hashUtils = new HashUtils();

            // We have a password, which will generate a Hash and Salt
            string Hash;
            string Salt;

            hashUtils.GetHashAndSaltString(changePasswordModel.NewPassword, out Hash, out Salt);

            changePasswordModel.NewPassword = Hash + Salt;
            accountRepository.UpdatePassword(changePasswordModel);
        }
        public void RegisterMember(Member member)
        {
            // We use the default SHA-256 & 4 byte length
            HashUtils hashUtils = new HashUtils();

            // We have a password, which will generate a Hash and Salt
            string Hash;
            string Salt;

            hashUtils.GetHashAndSaltString(member.Password, out Hash, out Salt);

            member.Password = Hash + Salt;
            accountRepository.RegisterMember(member);
        }
        public void CreateMember(Member member)
        {
            // We use the default SHA-256 & 4 byte length
            HashUtils hashUtils = new HashUtils();

            // We have a password, which will generate a Hash and Salt
            string Hash;
            string Salt;

            hashUtils.GetHashAndSaltString(member.Password, out Hash, out Salt);

            member.Password = Hash + Salt;

            accountRepository.CreateMember(member);
        }

        public void DeleteMember(int memberID)
        {
            accountRepository.DeleteMember(memberID);
        }


        public bool CheckEmailDoesntExist(string email)
        {
            return accountRepository.CheckEmailDoesntExist(email);
        }

        public string SelectPasswordVerifiationToken(string emailAddress)
        {
            return accountRepository.SelectPasswordVerifiationToken(emailAddress);
        }
    }
}