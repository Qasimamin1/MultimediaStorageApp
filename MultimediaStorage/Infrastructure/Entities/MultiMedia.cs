using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class MultiMedia
    {
        [Key, Required]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string? JsonData { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
