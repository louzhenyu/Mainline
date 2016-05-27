using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Visualization
{
    public static class TaskHelper
    {
        /// <summary>  
        /// 将一个方法function异步运行，在执行完毕时执行回调callback  
        /// </summary>  
        /// <param name="function">异步方法，该方法没有参数，返回类型必须是void</param>  
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法没有参数，返回类型必须是void</param>  
        public static Task RunAsync(Action function, Action callback)
        {
            Task result = Task.Factory.StartNew(function);

            if (callback != null)
            {
                callback();
            }
            return result;
        }

        public static Task RunWithWait(Action function, Action callback)
        {
            Task result = Task.Factory.StartNew(function);
            result.Wait();
            if (callback != null)
            {
                callback();
            }
            return result; 
        }
        public static Task<TResult> RunAsync<TResult>(Func<TResult> function, Action<TResult> callback)
        {
            Task<TResult> result = Task.Factory.StartNew<TResult>(function);

            if (callback != null)
            {
                callback(result.Result);
            }
            return result;
        }
        /// <summary>
        /// from Task.FromResult()      framework 4.5
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Task<TResult> FromResult<TResult>(TResult val)
        {
            return Task.Factory.StartNew<TResult>(() => { return val; });
        }
    } 
}
