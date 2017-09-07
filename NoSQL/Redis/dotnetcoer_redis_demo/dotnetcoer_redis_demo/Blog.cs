using ProtoBuf;
using System;

namespace dotnetcoer_redis_demo
{
    
    [Serializable]
    [ProtoContract]
    public class Blog
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(4)]
        public string Content { get; set; }
    }
}