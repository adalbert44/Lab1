using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
    public partial class Query
    {
        public Query()
        {
        }
        public int Id { get; set; }
        public int Parameter { get; set; }
        public string StringParameter { get; set; }
    }
}
