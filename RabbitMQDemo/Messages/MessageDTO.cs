using System;
using EasyNetQ;

namespace Messages
{
    public class DtoOrder111
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DtoOrderWQ
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DtoOrderTopic
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}