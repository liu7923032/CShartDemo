using NineSix.Common;
using NineSix.Common.Attibutes;
using NineSix.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSix.BLL
{
    [AutoCommit]
    public class AppService:IAppService
    {

        protected static Logger logger = new Logger(typeof(AppService));
    }
}
