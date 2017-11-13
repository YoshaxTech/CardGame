using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Server
{
    [ProtoContract]
    public class MessageObject
    {
        [ProtoMember(1)]
        public string message { get; set; }
        [ProtoMember(2)]
        public string action { get; set; }
       
        public MessageObject() { }
        public MessageObject(string _message,string _action)
        {
            this.message = _message;
            this.action = _action;
        }
    }
}
