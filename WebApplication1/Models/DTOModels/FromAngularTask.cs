using System;
using System.Collections.Generic;
using Szakdolgozat.Models.DatabaseModels;

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

        // usernames
        public List<string> users { get; set; }

        public List<SubTask> subTasks { get; set; }

        public List<Comment> comments { get; set; }

        public string description { get; set; }

        public DateTime endDate { get; set; }
    }
}
