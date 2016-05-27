using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Quartz;

namespace JinRi.Policy.Jobs
{
    /// <summary>
    /// 政策同步Job
    /// </summary>
    public class PolicySyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            PolicySync policySync = new PolicySync();
            policySync.PolicySyncProcess();
        }
    }
}
