using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szakdolgozat.Models.DTOModels
{
    public class FromAngularTask
    {
        public string id { get; set; }

        public string name { get; set; }

        public string project { get; set; }

        public string priority { get; set; }

        public string status { get; set; }

        public string type { get; set; }

        public string users { get; set; }

        public string subTasks { get; set; }

        public string description { get; set; }

        public DateTime endDate { get; set; }
    }
}
