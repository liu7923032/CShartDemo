using NineEight.Common;
using NineEight.Common.Attibutes;
using NineEight.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineEight.BLL
{
    [AutoCommit]
    public class AppService:IAppService
    {

        protected static Logger logger = new Logger(typeof(AppService));
    }
}
