using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Models
{
    public class ProjectModel
    {
        public string UserId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }

    }
}
