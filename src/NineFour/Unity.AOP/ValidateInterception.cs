using Microsoft.Practices.Unity.InterceptionExtension;
using NineFour.Common;
using NineFour.Common.Extentision;
using NineFour.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.AOP
{
    public class ValidateInterception : IInterceptionBehavior
    {
        private static LogHelper logger = new LogHelper(typeof(ValidateInterception));
        public bool WillExecute { get { return true; } }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn retVal = null;
            logger.Info("++++++执行方法前++++++");
            logger.Info("++++++SQL 操作执行前对数据进行校验++++++");
            try
            {
                Type type = input.Arguments[0].GetType();
                var parameter = input.Arguments[0];
                if (parameter != null)
                {
                    if (type.CheckValidate(parameter))
                    {
                        retVal = getNext()(input, getNext);
                        logger.Error("++++++数据保存成功++++++");
                    }
                    else
                    {
                        logger.Error("++++++数据保存失败++++++");
                        //不执行方法
                        retVal = input.CreateMethodReturn(null);
                    }
                }
                else
                {
                    //如果没有参数就不对数据进行校验
                    retVal = getNext()(input, getNext);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("++++++执行方法后++++++");
            return retVal;
        }
    }
}
