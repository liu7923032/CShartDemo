using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nine.Model
{
    public class JSONObject
    {
        public string Name { get; set; }

        public List<Event> Events { get; set; }

        public string Color { get; set; }
    }

    public class Event
    {
        public bool IsFirst { get; set; }
        public string EName { get; set; }
    }
}
