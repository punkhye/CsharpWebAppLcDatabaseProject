using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S.Data.Entities
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }


        [Required] 
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Address { get; set; }

        [Display(Name = "Number Of Halls")]
        public byte NumberOfHalls { get; set; }

        [StringLength(250, MinimumLength = 5)]
        public string Website { get; set; }

        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        public virtual ICollection<Screening>? Screenings { get; set; }
    }
}
