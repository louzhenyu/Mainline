using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.PolicyJob.ConsoleApplication1
{
    public class JobInfo
    {
        private string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description = string.Empty;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string cronExpression;

        public string CronExpression
        {
            get { return cronExpression; }
            set { cronExpression = value; }
        }

        private string requestURL;

        public string RequestURL
        {
            get { return requestURL; }
            set { requestURL = value; }
        }

        private RequestType requestType = RequestType.Get;

        public RequestType RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }

        private string groupName = "DefaultGroup";

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }
    }

    public enum RequestType
    {
        Get,
        Post
    }
}
