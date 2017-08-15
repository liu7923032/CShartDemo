using NineEight.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineEight.WCF.Model;

namespace NineEight.WCF.Service
{
    public class MathService : IMathService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public List<UserModel> GetUsers()
        {
            List<UserModel> list = new List<UserModel>()
            {
                new UserModel(){UserId=1,UserName="dark"},
                new UserModel(){UserId=1,UserName="dark"},
            };
            return list;
        }
    }
}
