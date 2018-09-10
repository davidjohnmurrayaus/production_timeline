using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeline.Data
{
    public class Shot
    {
        public int shotId;
        public TimeSpan duration;
        public List<Comment> comments;
    }
}
