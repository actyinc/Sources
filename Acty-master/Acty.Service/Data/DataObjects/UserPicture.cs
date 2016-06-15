using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Acty
{
    public class UserPicture
    {
        public string UserId { get; set; }
        public Stream Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
    }
}