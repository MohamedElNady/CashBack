using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models.ViewModel
{
    public class AddItemVM
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public string Title {  get; set; }

        [Required]
        public string Description {  get; set; }

        [Required]
        public string BigImgUrl {  get; set; }

        [Required]
        public string smallImgUrl { get; set; }

        public string discount {  get; set; }

    }
}
