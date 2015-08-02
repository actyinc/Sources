using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DocumentDBDataService
{
    [DataContract]
    public class ResponseData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string Exp { get; set; }

        [DataMember]
        public string Technology { get; set; }
    }
}