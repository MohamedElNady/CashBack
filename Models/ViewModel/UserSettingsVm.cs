using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models.ViewModel
{
    public class UserSettingsVm
    {
        public string Name  { get; set; }
        public string UserName {  get; set; }
        public string Email {  get; set; }
        public string Password {  get; set; }
        public string Birthdate {  get; set; }
        public string PhoneNumber {  get; set; }
        public string Address {  get; set; }  
        public string Gender {  get; set; }
        public string ChangePassword {  get; set; }
        public string ConfirmChangedPass {  get; set; }
    }
}
