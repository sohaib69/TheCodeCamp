using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheCodeCamp.Models
{
    public class CampModel
    {
        /* Note:
         * Required - It will validate our model parameter using ModelState.isValid.
         * Range - It a another validate take place autometically if we define ModelState.isValid.
         */

        [Required]
        public string Name { get; set; }

        [Required]
        public string Moniker { get; set; }

        [Required]
        public DateTime EventDate { get; set; } = DateTime.MinValue;

        [Required]
        [Range(1, 30)]
        public int Length { get; set; } = 1;

        public ICollection<TalksModel> Talks { get; set; }

        // Include Location Information.
        /*  Note:
         *  If we add current attribute nae following by primary class name AutoMapper will autometically map it. It has it's own limitation.
         *  If we don't want to do that we need to sesify each attribute inside our MappingProfile to pick it. 
        */

        public string VenueName { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
    }
}