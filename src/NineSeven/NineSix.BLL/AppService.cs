using NineSeven.Common;
using NineSeven.Common.Attibutes;
using NineSeven.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineSeven.BLL
{
    [AutoCommit]
    public class AppService:IAppService
    {

        protected static Logger logger = new Logger(typeof(AppService));
    }
}
