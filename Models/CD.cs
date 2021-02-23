using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CDCollectionApp.Models
{
    public class CD
    {
        public int CDId { get; set; }

        [DisplayName("Namn")]
        [Required(ErrorMessage = "Vänligen ange titel för CD-skivan du önskar lägga till")]
        public string Name { get; set; }

        [DisplayName("Utgivningsdatum")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Tillgänglig")]
        public bool Available { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [NotMapped]
        public Track Track { get; set; }

        [DisplayName("Låtar")]
        public ICollection<Track> Tracks { get; set; }
        public ICollection<Rent> Rented { get; set; }


    }
    public class Artist
    {
        public int ArtistId { get; set; }

        [DisplayName("Namn")]
        [Required(ErrorMessage = "Vänligen ange artist/band")]
        public string Name { get; set; }
        public ICollection<CD> CDs { get; set; }
    }
    public class Rent
    {
        public int RentId { get; set; }

        [DisplayName("Namn")]
        [MinLength(2, ErrorMessage = "Namn måste vara minst två tecken")]
        [Required(ErrorMessage = "Vänligen ange ditt namn för att låna en CD-skiva")]
        public string Name { get; set; }

        [DisplayName("Lånedatum")]
        public DateTime RentDate { get; set; }
        public int CDId { get; set; }
        public CD CD { get; set; }
    }
    public class Track
    {
        public int TrackId { get; set; }

        [DisplayName("Namn")]
        [Required(ErrorMessage = "Vänligen ange ditt låtens titel")]
        public string Name { get; set; }
        public int CDId { get; set; }
        public CD CD { get; set; }


    }
}
