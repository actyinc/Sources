using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DocumentDBDataService
{
    [DataContract(Namespace = "http://hemanth.azureservices.net/acti")]
    public class RequestData
    {
        [DataMember]
        public string details { get; set; }
    }
}