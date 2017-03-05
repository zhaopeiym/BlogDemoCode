using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 窗体程序
{
    class MyAsyncTest
    {
        public async Task<string> GetUrlStringAsync(HttpClient http, string url, int time)
        {
            await Task.Delay(time);
            return await http.GetStringAsync(url);
        }
    }
}


internal class MyAsyncTest
{
    public Task<string> GetUrlStringAsync(HttpClient http, string url, int time)
    {
        GetUrlStringAsyncdStateMachine stateMachine = new GetUrlStringAsyncdStateMachine()
        {
            _this = this,
            http = http,
            url = url,
            time = time,
            _builder = AsyncTaskMethodBuilder<string>.Create(),
            _state = -1
        };
        stateMachine._builder.Start(ref stateMachine);
        return stateMachine._builder.Task;
    }

    private sealed class GetUrlStringAsyncdStateMachine : IAsyncStateMachine
    {
        public int _state;
        public MyAsyncTest _this;
        private string _str1;
        public AsyncTaskMethodBuilder<string> _builder;
        private TaskAwaiter taskAwaiter1;
        private TaskAwaiter<string> taskAwaiter2;
        public HttpClient http;
        public int time;
        public string url;

        private void MoveNext()
        {
            string str;
            int num = this._state;
            try
            {
                TaskAwaiter awaiter;
                MyAsyncTest.GetUrlStringAsyncdStateMachine d__;
                string str2;
                switch (num)
                {
                    case 0:
                        break;

                    case 1:
                        goto Label_00CD;

                    default:
                        awaiter = Task.Delay(this.time).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_0077;
                        }
                        this._state = num = 0;
                        this.taskAwaiter1 = awaiter;
                        d__ = this;
                        this._builder.AwaitUnsafeOnCompleted<TaskAwaiter, MyAsyncTest.GetUrlStringAsyncdStateMachine>(ref awaiter, ref d__);
                        return;
                }
                awaiter = this.taskAwaiter1;
                this.taskAwaiter1 = new TaskAwaiter();
                this._state = num = -1;
            Label_0077:
                awaiter.GetResult();
                awaiter = new TaskAwaiter();
                TaskAwaiter<string> awaiter2 = this.http.GetStringAsync(this.url).GetAwaiter();
                if (awaiter2.IsCompleted)
                {
                    goto Label_00EA;
                }
                this._state = num = 1;
                this.taskAwaiter2 = awaiter2;
                d__ = this;
                this._builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, MyAsyncTest.GetUrlStringAsyncdStateMachine>(ref awaiter2, ref d__);
                return;
            Label_00CD:
                awaiter2 = this.taskAwaiter2;
                this.taskAwaiter2 = new TaskAwaiter<string>();
                this._state = num = -1;
            Label_00EA:
                str2 = awaiter2.GetResult();
                awaiter2 = new TaskAwaiter<string>();
                this._str1 = str2;
                str = this._str1;
            }
            catch (Exception exception)
            {
                this._state = -2;
                this._builder.SetException(exception);
                return;
            }
            this._state = -2;
            this._builder.SetResult(str);
        }

        [DebuggerHidden]
        private void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        void IAsyncStateMachine.MoveNext()
        {
            throw new NotImplementedException();
        }

        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new NotImplementedException();
        }
    }
}





