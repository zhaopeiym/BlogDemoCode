using System;
using System.Collections.Generic;
using System.Linq;

namespace ModbusTcpServer
{
    public class DataPersist
    {
        string prefix;
        static Dictionary<string, string> data = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix">前缀</param> 
        public DataPersist(string prefix)
        {
            this.prefix = $"iot_{prefix}_";
        }

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Read(string key)
        {
            key = prefix + key;
            if (data.Keys.Contains(key))
            {
                return data[key];
            }
            return string.Empty;
        }

        public string Read(int key)
        {
            return Read(key.ToString());
        }

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Write(string key, string value)
        {
            key = prefix + key;
            if (data.Keys.Contains(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

        public void Write(int key, string value)
        {
            Write(key.ToString(), value);
        }
    }
}
