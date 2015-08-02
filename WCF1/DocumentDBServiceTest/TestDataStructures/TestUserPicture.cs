using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDBServiceTest
{
    public class TestUserPicture
    {
        public string UserId { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
    }
}
