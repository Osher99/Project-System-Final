using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string ProjectName { get; set; }

        public string ImageURL { get; set; }

        public bool IsDone { get; set; }

        public string Description { get; set; }



    }
}
