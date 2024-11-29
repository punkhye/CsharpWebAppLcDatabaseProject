using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S.Data.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [ StringLength(250, MinimumLength = 5)]
        public string Genre { get; set; }

        [Display(Name = "Release Date ")]
        //[DataType(dataType: DataType.Date)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Duration (minutes)")]
        public int Duration { get; set; }

        [StringLength(2500, MinimumLength = 15)]
        public string Description { get; set; }

        public virtual ICollection<Screening>? Screenings { get; set; }
    }
}
