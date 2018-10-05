using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class State : Transport
    {
        #region Model Data
        
        public int StateId { get; set; }

        [Required(ErrorMessage="Name field is required!!!")]
        public string Name { get; set; }

        #endregion
    }
}
