using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class ClientCard
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ClientCard1 { get; set; }

        public virtual Client Client { get; set; }
    }
}
