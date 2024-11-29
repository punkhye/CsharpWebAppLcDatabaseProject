using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace S.Data.Entities
{
    public class Screening
    {
        [Key]
        public int ScreeningId { get; set; }

        [Required]
        [Display(Name = "Movie")]
        public int? MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        [Display(Name = "Cinema")]
        public int? CinemaId { get; set; }
        public virtual Cinema? Cinema { get; set; }

        [Display(Name = "Screening Date")]
        [Required]
        // [DataType(dataType: DataType.Date)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ScreeningDate { get; set; }

        [Display(Name = "Number Of Seats")]
        public int NumberOfSeats { get; set; }

        [Display(Name = "Ticket Price")]
        
        public decimal TicketPrice { get; set; }
    }
}
