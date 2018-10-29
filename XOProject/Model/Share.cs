using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XOProject
{
    public class Share
    {
        public int Id { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public decimal Rate { get; set; }
    }
}
