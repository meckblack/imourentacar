using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class LGA : Transport
    {
        #region Data Model

        public int LGAId { get; set; }

        [Required(ErrorMessage="Name field is requried!!!")]
        public string Name { get; set; }

        #endregion

        #region Foreign Key

        [DisplayName("State")]
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }

        #endregion
    }
}
