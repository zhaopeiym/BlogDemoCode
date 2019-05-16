using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperDemo.Filters
{
    public class ActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var profiler = MiniProfiler.StartNew("StartNew");
            using (profiler.Step("Level1"))
            {
                //执行Action
                await next();
            }
            WriteLog(profiler);
        }

        /// <summary>
        /// sql跟踪
        /// 下载：MiniProfiler.AspNetCore  4.0
        /// </summary>
        /// <param name="profiler"></param>
        private void WriteLog(MiniProfiler profiler)
        {
            if (profiler?.Root != null)
            {
                var root = profiler.Root;
                if (root.HasChildren)
                {
                    root.Children.ForEach(chil =>
                    {
                        if (chil.CustomTimings?.Count > 0)
                        {
                            foreach (var customTiming in chil.CustomTimings)
                            {
                                var all_sql = new List<string>();
                                var err_sql = new List<string>();
                                var all_log = new List<string>();
                                int i = 1;
                                customTiming.Value?.ForEach(value =>
                                {
                                    if (value.ExecuteType != "OpenAsync")
                                        all_sql.Add(value.CommandString);
                                    if (value.Errored)
                                        err_sql.Add(value.CommandString);
                                    var log = $@"【{customTiming.Key}{i++}】{value.CommandString} Execute time :{value.DurationMilliseconds} ms,Start offset :{value.StartMilliseconds} ms,Errored :{value.Errored}";
                                    all_log.Add(log);
                                });

                                //TODO  日志记录
                                //if (err_sql.Any())
                                //    Logger.Error(new Exception("sql异常"), "异常sql:\r\n" + string.Join("\r\n", err_sql), sql: string.Join("\r\n\r\n", err_sql));
                                //Logger.Debug(string.Join("\r\n", all_log), sql: string.Join("\r\n\r\n", all_sql));
                            }
                        }
                    });
                }
            }
        }
    }
}
