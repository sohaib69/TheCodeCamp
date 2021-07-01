using System.ComponentModel.DataAnnotations;

namespace TheCodeCamp.Models
{
    public class TalksModel
    {
        public int TalkId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(4000, MinimumLength = 10)]
        public string Abstract { get; set; }

        [Required]
        [Range(100, 500)]
        public int Level { get; set; }
        public SpeakerModel Speaker { get; set; }
    }
}