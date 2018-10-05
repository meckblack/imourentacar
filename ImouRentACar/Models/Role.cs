﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Role : Transport
    {
        #region Data Model

        public int RoleId { get; set; }

        [Required(ErrorMessage="Name is required")]
        public string Name { get; set; }

        [Display(Name="Can Do Everything")]
        public bool CanDoEverything { get; set; }

        #endregion
    }
}
