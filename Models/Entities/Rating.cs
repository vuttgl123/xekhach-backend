using System.ComponentModel.DataAnnotations.Schema;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public int UserId { get; set; }

        [Column("Rating")]
        public byte RatingValue { get; set; }

        public string Feedback { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Ride Ride { get; set; }
        public User User { get; set; }
    }
}
