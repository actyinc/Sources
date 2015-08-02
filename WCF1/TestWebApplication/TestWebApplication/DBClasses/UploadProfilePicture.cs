using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApplication
{
    public class UploadProfilePicture
    {
        public string UserId { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
    }
}