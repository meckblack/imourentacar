using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models.Enums
{
    public enum Verification
    {
        Approve,
        Disapprove,
        Replied,
        LinkSent,
        Junk,
        [DisplayName("Yet To Reply")]
        YetToReply
    }
}
