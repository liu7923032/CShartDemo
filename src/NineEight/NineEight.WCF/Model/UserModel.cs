using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.WCF.Model
{
    [DataContract]
    public class UserModel
    {
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}
