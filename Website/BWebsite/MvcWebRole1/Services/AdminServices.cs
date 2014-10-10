using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bettery.WebRole.Repositories;
using Bettery.WebRole.Models;
using MvcWebRole1.Common;

namespace Bettery.WebRole.Services
{
    public class AdminService
    {
        private AdminRepository adminRepository;

        public AdminService()
            : this(new AdminRepository())
        {

        }

        public AdminService(AdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public IEnumerable<Member> GetMembers()
        {
            return adminRepository.SelectMembers();
        }

        public EditMember GetMember(int MemberID)
        {
            return adminRepository.SelectMember(MemberID);
        }

        public void UpdateMember(EditMember editMember)
        {

            adminRepository.UpdateMember(editMember);
        }

        public void DeleteMember(int MemberID)
        {

            adminRepository.DeleteMember(MemberID);
        }


        public void CreatePromo(AddPromoModel addPromoModel)
        {
            try
            {
                adminRepository.InsertPromo(addPromoModel);
            }
            catch
            {
                throw;
            }
            
        }

        public bool GetIsExistingPromoCode(string PromoCode)
        {
            return adminRepository.GetIsExistingPromoCode(PromoCode);
        }

        
    }
}