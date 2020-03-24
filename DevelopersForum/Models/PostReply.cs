using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Models
{
    public class PostReply
    {
        public int PostReplyId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string Id { get; set; }
        public virtual ApplicationUsers ApplicationUsers { get; set; }
        public virtual Post Post { get; set; }
        public int  PostId { get; set; }
    }
}
