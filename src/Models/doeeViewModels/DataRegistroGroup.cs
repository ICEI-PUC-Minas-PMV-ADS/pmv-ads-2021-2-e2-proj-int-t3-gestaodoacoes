using System;
using System.ComponentModel.DataAnnotations;

namespace doee.Models.doeeViewModels
{
    public class DataRegistroGroup
    {
        [DataType(DataType.Date)]
        public DateTime? InscricaoDate { get; set; }

        public int Count { get; set; }
    }
}
