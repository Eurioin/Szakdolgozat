using System.Collections.Generic;

namespace Szakdolgozat.Models.DTOModels
{
    public class FromAngularAccount
    {
        public string id { get; set; }
        
        public string name { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; }

        public List<string> roles { get; set; }
    }
}
