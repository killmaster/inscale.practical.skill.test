using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy.contracts
{
    public class AtLeastNReactionsAndSpecificTagRequest
    {
        public int? Reactions { get; set; }
        public string? Tag { get; set; }
    }
}
