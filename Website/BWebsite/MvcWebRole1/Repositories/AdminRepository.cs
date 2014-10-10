using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bettery.WebRole.Models;
using System.Data;
using Dapper;

namespace Bettery.WebRole.Repositories
{
    public class AdminRepository : BaseRepository
    {

        public void InsertPromo(AddPromoModel addPromoModel)
        {
            using (IDbConnection connection = OpenConnection())
            {
                System.TimeSpan midnight = new System.TimeSpan(0, 23, 59, 59);
                addPromoModel.ExpireDate = addPromoModel.ExpireDate.Add(midnight);

                const string query = "INSERT INTO PromotionalCode (PromoCode, PromoCodeType, Amount, ExpireDate, MultiUse, Description, CreatedBy) VALUES (@PromoCode, @PromoCodeType, @Amount, @ExpireDate, @MultiUse, @Description, @CreatedBy)";
                var parameters = new
                {
                    PromoCode = addPromoModel.PromoCode,
                    Amount = addPromoModel.PromoAmount,
                    PromoCodeType = addPromoModel.PromoTypes,
                    ExpireDate = addPromoModel.ExpireDate,
                    MultiUse = addPromoModel.MultipleUse, 
                    Description = addPromoModel.Description,
                    CreatedBy = addPromoModel.CreatedBy

                };
                try
                {
                    connection.Execute(query, parameters);
                }
                catch
                {
                    throw;
                }
                
                return;
            }
        }

        public bool GetIsExistingPromoCode(string promoCode)
        {
            using (IDbConnection connection = OpenConnection())
            {
                
                const string query = "select PromoCode from PromotionalCode where PromoCode = @Promocode";

                var parameters = new
                {
                    PromoCode = promoCode
                };

                var Promo = connection.Query<int>(query, parameters);

                if (Promo.Count() > 0)
                    return false;
                else
                    return true;
                
            }
        }   
        
        
        public IEnumerable<Member> SelectMembers()
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "Select MemberID, CustomerProfileID, FirstName, LastName, Address1, Address2, State, Country, EmailAddress, City, Zipcode, [Password], AccountBalance, BatteryPacksInPlan, BatteryPacksCheckedOut, MemberTotalBatteries, FreeCases, EmailOptIn, isMember, UpgradeSubscriptionDate " +
                                     "FROM Member WHERE MemberID <> 357 and (deleted is null OR deleted = 0) ORDER BY LastName, EmailAddress ";
                return connection.Query<Member>(query);
            }

        }

        //public IEnumerable<EditMemberCorporateAccount> SelectCorpAccounts()
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        const string query = "Select Name AS Text, CorpAccountID AS Value " +
        //                             "FROM CorpAccount";
        //        return connection.Query<Member>(query);
        //    }

        //}

        public EditMember SelectMember(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "Select MemberID, CustomerProfileID, FirstName, LastName, Address1, Address2, State, Country, EmailAddress, City, Zipcode, [Password], AccountBalance, BatteryPacksInPlan, BatteryPacksCheckedOut, MemberTotalBatteries, FreeCases, EmailOptIn, isMember, UpgradeSubscriptionDate " +
                                     "FROM Member WHERE MemberID = @MemberID";

                var parameters = new
                {
                    MemberID = memberID
                };

                return connection.Query<EditMember>(query, parameters).SingleOrDefault();
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
                                     "FreeCases = @FreeCases, " +
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
                    FreeCases = editMember.FreeCases,
                    EmailOptIn = editMember.EmailOptIn
                };

                return connection.Execute(query, parameters);
            }
        }

        public int DeleteMember(int memberID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "UPDATE Member " +
                                     "SET Deleted = 1 " +
                                     "WHERE MemberID = @MemberID";
                var parameters = new
                {
                    MemberID = memberID
                };

                return connection.Execute(query, parameters);
            }
        }

    }
}