using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Header : Transport
    {
        #region Model Data

        public int HeaderId { get; set; }

        public string Image { get; set; }

        public string Heading { get; set; }

        public string Body { get; set; }

        #endregion
    }
}
