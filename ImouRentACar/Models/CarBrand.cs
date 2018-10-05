using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class CarBrand : Transport
    {
        #region Data Model

        public int CarBrandId { get; set; }

        public string Name { get; set; }

        #endregion
    }
}
