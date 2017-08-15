using NineEight.WCF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.WCF.Interface
{
    [ServiceContract]
    public interface IMathService
    {
        [OperationContract]
        int Add(int a, int b);

        List<UserModel> GetUsers();
    }
}
