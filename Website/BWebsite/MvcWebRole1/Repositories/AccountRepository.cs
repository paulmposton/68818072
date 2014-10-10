using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bettery.WebRole.Models;
using System.Data;
using Dapper;
using MvcWebRole1.Common;

namespace Bettery.WebRole.Repositories
{
    public class AccountRepository : BaseRepository
    {
        public IEnumerable<Credit> SelectCredits(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "SELECT AccountBalance " +
                                     "FROM Member WHERE MemberID = @MemberID";
                return connection.Query<Credit>(query, new { MemberID = memberID });
            }
        }
        public IEnumerable<Transaction> SelectTransactions(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "SELECT TransactionID, TotalAmount, AAVend, AAAVend, TransactionDateTime  " +
                                     "FROM KioskTransaction WHERE MemberID = @MemberID";
                return connection.Query<Transaction>(query, new { MemberID = memberID });
            }
        }

        public TransactionDetail SelectTransactionDetail(int transactionID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select kt.TotalAmount, kt.AAVend, kt.AAAVend, kt.AAReturn, kt.AAAReturn, ss.Store, kt.CardInfo, kt.PromoCodeAmount, kt.TaxAmount, kt.TransactionDateTime, kt.SwapPrice, kt.PurchasePrice " +
                "from kiosktransaction kt inner join SwapStation ss on kt.SwapStationID = ss.SwapStationID where kt.TransactionID = @TransactionID";
                                     
                return connection.Query<TransactionDetail>(query, new { TransactionID = transactionID }).SingleOrDefault();
            }
        }        
        
        public void InsertCreditCard (int memberID, CreditCard creditCard)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "UPDATE Member SET CustomerProfileID = @CustomerProfileID, PaymentProfileID = @PaymentProfileID, CCLastFourDigits = @CCLastFourDigits, CCExpireDate = @CCExpireDate " +
                                     "WHERE MemberID = @MemberID";
                connection.Execute(query, new { MemberID = memberID, CustomerProfileID = creditCard.CustomerProfileID.ToString(), PaymentProfileID = creditCard.PaymentProfileID.ToString(), CCLastFourDigits = creditCard.CCLastFourDigits, CCExpireDate = creditCard.CCExpireDate });
            }

        }



        public Member SelectMember(string registrationID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "Select MemberID, CustomerProfileID, FirstName, LastName, Address1, Address2, State, Country, EmailAddress, City, Zipcode, [Password], AccountBalance, BatteryPacksInPlan, BatteryPacksCheckedOut, MemberTotalBatteries, FreeCases, EmailOptIn, isMember, UpgradeSubscriptionDate " +
                                     "FROM Member WHERE RegistrationID = @RegistrationID";
                return connection.Query<Member>(query, new { RegistrationID = registrationID }).SingleOrDefault();
            }

        }

        public Member SelectMember(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "Select MemberID, CustomerProfileID, FirstName, LastName, Address1, Address2, State, Country, EmailAddress, City, Zipcode, [Password], AccountBalance, BatteryPacksInPlan, BatteryPacksCheckedOut, MemberTotalBatteries, FreeCases, EmailOptIn, isMember, UpgradeSubscriptionDate " +
                                     "FROM Member WHERE MemberID = @MemberID";
                return connection.Query<Member>(query, new { MemberID = memberID }).SingleOrDefault();
            }

        }
        public EditMember SelectEditMember(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "Select MemberID, CustomerProfileID, FirstName, LastName, Address1, Address2, State, Country, EmailAddress, City, Zipcode, [Password], AccountBalance, BatteryPacksInPlan, BatteryPacksCheckedOut, MemberTotalBatteries, FreeCases, EmailOptIn, isMember, UpgradeSubscriptionDate " +
                                     "FROM Member WHERE MemberID = @MemberID";
                return connection.Query<EditMember>(query, new { MemberID = memberID }).SingleOrDefault();
            }

        }
        public bool CheckEmailDoesntExist(string email)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "SELECT * FROM Member WHERE EmailAddress = @EmailAddress and isMember = 1 ";

                var parameters = new
                {
                    EmailAddress = email
                };

                var Member = connection.Query<int>(query, parameters);

                if (Member.Count() > 0)
                    return false;
                else
                    return true;
            }
        }
        public void CreateMember(Member member)
        {
            using (IDbConnection connection = OpenConnection())
            {
                //const string query = "INSERT INTO Member " +
                //                        "(FirstName, LastName, Zipcode, EmailAddress, Password, EmailOptIn, FreeCases, isMember) " +
                //                        "VALUES(@FirstName, @LastName, @Zipcode, @EmailAddress, @Password, @EmailOptIn, @FreeCases, @isMember)";
                var parameters = new
                {
                    CustomerProfileID = "",
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Zipcode = member.Zipcode,
                    EmailAddress = member.EmailAddress,
                    Password = member.Password,
                    EmailOptIn = member.EmailOptIn,
                    BatteryPacksInPlan = 0
                };
                connection.Execute("AddMember", parameters, commandType: CommandType.StoredProcedure);
                //return connection.Execute(query, parameters);
            }
        }

        public int UpdatePassword(ChangePasswordModel changePasswordModel)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "UPDATE Member " +
                                     "SET Password = @Password " +
                                     "WHERE MemberID = @MemberID";
                var parameters = new
                {
                    Password = changePasswordModel.NewPassword,
                    MemberID = changePasswordModel.MemberID
                };

                return connection.Execute(query, parameters);
            }
        }
        public int UpdateMember(EditMember editMember)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "UPDATE Member " + 
                                     "SET FirstName = @FirstName, " +
                                     "LastName = @LastName, " +
                                     "Address1 = @Address1, " +
                                     "Address2 = @Address2, " +
                                     "City = @City, " +
                                     "State = @State, " +
                                     "Zipcode = @Zipcode, " +
                                     "Country = @Country, " +
                                     "EmailOptIn = @EmailOptIn " +
                                     "WHERE MemberID = @MemberID";
                var parameters = new
                {
                    MemberID = editMember.MemberID,
                    CustomerProfileID = editMember.CustomerProfileID,
                    FirstName = editMember.FirstName,
                    LastName = editMember.LastName,
                    Address1 = editMember.Address1,
                    Address2 = editMember.Address2,
                    City = editMember.City,
                    State = editMember.State,
                    Zipcode = editMember.Zipcode,
                    Country = editMember.Country,
                    EmailOptIn = editMember.EmailOptIn
                };

                return connection.Execute(query, parameters);
            }
        }

        public void DeleteMember(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "DELETE Member " +
                                     "WHERE MemberID = @MemberID";
                var parameters = new
                {
                    MemberID = memberID,

                };
                connection.Execute(query, parameters);
            }
        }

        public int RegisterMember(Member member)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "UPDATE Member " +
                                     "SET FirstName = @FirstName, " +
                                     "LastName = @LastName, " +
                                     "Address1 = @Address1, " +
                                     "Address2 = @Address2, " +
                                     "City = @City, " +
                                     "State = @State, " +
                                     "Zipcode = @Zipcode, " +
                                     "Country = @Country, " +
                                     "EmailAddress = @EmailAddress, " +
                                     "Password = @Password, " +
                                     "RegistrationID = NULL, " +
                                     "EmailOptIn = @EmailOptIn, " +
                                     "FreeCases = 3, " +
                                     "isMember = 1 " +
                                     "WHERE MemberID = @MemberID";
                var parameters = new
                {
                    MemberID = member.MemberID,
                    CustomerProfileID = member.CustomerProfileID,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Address1 = member.Address1,
                    Address2 = member.Address2,
                    City = member.City,
                    State = member.State,
                    Zipcode = member.Zipcode,
                    Country = member.Country,
                    EmailAddress = member.EmailAddress,
                    Password = member.Password,
                    EmailOptIn = member.EmailOptIn
                };

                return connection.Execute(query, parameters);
            }
        }

        public string SelectPasswordVerifiationToken(string emailAddress)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string passwordVerificationToken = Guid.NewGuid().ToString();
                string passCode = Generators.GeneratePassCode(5);
                const string query = "UPDATE Member SET PasswordVerifiationToken = @PasswordVerifiationToken, PasswordVerificationPassCode = @PassCode, PasswordVerificationRequestDate = @CurrentDateTime WHERE EmailAddress = @EmailAddress";
                var parameters = new
                {

                    EmailAddress = emailAddress,
                    PasswordVerifiationToken = passwordVerificationToken,
                    PassCode = passCode,
                    CurrentDateTime = DateTime.UtcNow.ToString()
                };

                connection.Execute(query, parameters);
                return passwordVerificationToken;
            }
        }

    }
}