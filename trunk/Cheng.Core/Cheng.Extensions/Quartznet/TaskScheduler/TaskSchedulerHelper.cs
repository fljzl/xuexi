using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheng.Extensions.Quartznet
{
    public class TaskSchedulerHelper
    {
        public static string JobDataMapKeyJobId = "JobId";

        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        public static JobKey CreateJobKey(string jobName, string jobGroupName)
        {
            return new JobKey(jobName, jobGroupName);
        }
        public static TriggerKey CreateTriggerKey(string triggerName, string triggerGroupName)
        {
            return new TriggerKey(triggerName, triggerGroupName);
        }

    }
}
