using System.Collections.Generic;

namespace Szakdolgozat.Models.DTOModels
{
    public class FromAngularProject
    {
        public string id { get; set; }

        public string name { get; set; }

        public List<string> users { get; set; }

        public string company { get; set; }
    }
}
