using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class Client
    {
        public Client()
        {
            ClientCard = new HashSet<ClientCard>();
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ClientCard> ClientCard { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
