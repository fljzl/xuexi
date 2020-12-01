using Cheng.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Cheng.Comon;
namespace Cheng.Extensions.Quartznet
{
    public class MangerScheduler : IMangerScheduler<Customer_JobInfo>
    {

        private readonly ILogger<MangerScheduler> _logger;
        private readonly IConfiguration _configuration;
        private IScheduler _scheduler;
        public MangerScheduler(ILogger<MangerScheduler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _scheduler = getScheduler();
            _configuration = configuration;
        }

        private IScheduler getScheduler()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServer";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "500";
            properties["quartz.threadPool.threadPriority"] = "Normal";


            properties["quartz.scheduler.exporter.port"] = "555";//端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";//名称
            properties["quartz.scheduler.exporter.channelType"] = "tcp";//通道类型
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";
            properties["quartz.jobStore.clustered"] = "true";//集群配置
                                                             //指定quartz持久化数据库的配置
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";//存储类型
            properties["quartz.serializer.type"] = "json";
            properties["quartz.jobStore.tablePrefix"] = "Qrtz_";//表名前缀
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";//驱动类型
            properties["quartz.jobStore.dataSource"] = "myDS";//数据源名称
            properties["quartz.dataSource.myDS.connectionString"] = @"Server=.\sql2008r2;Database=ApiDataBase;uid=sa;pwd=111111;MultipleActiveResultSets=true";
            properties["quartz.dataSource.myDS.provider"] = "SqlServer";//数据库版本
            properties["quartz.scheduler.instanceId"] = "AUTO";

            //if (configs.IsUseproxy)
            //{
            //    var address = $"{configs.quartzchannelType}://{configs.localIp}:{configs.quartzport}/{configs.quartzbindName}";
            //    properties.Add("quartz.scheduler.proxy", "true");
            //    properties.Add("quartz.scheduler.proxy.address", address);
            //}
            //properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            //properties["quartz.scheduler.proxy"] = "true";
            //properties["quartz.scheduler.proxy.address"] = $"{configs.quartzchannelType}://{configs.quartzlocalIp}:{configs.quartzport}/{configs.quartzbindName}";
            var schedulerFactory = new StdSchedulerFactory(properties);

            var Scheduler = schedulerFactory.GetScheduler().Result;

            //Scheduler.ListenerManager.AddJobListener(new MyJobListener(), GroupMatcher<JobKey>.AnyGroup());
            //Scheduler.ListenerManager.AddSchedulerListener(new MySchedulerListener());
            //Scheduler.ListenerManager.AddTriggerListener(new MyTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());

            return Scheduler;
        }

        #region 操作job

        public bool DeleteJob(Customer_JobInfo jobInfo)
        {
            var jobKey = TaskSchedulerHelper.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            //var triggerKey = TaskSchedulerHelper.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            //_scheduler.PauseTrigger(triggerKey);
            //_scheduler.UnscheduleJob(triggerKey);
            _scheduler.DeleteJob(jobKey);
            return true;
        }

        public bool ModifyJobCron(Customer_JobInfo jobInfo)
        {
            var scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
            var triggerKey = TaskSchedulerHelper.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            var trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1)).WithIdentity(triggerKey).WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing()).Build();
            _scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }

        public bool PauseJob(Customer_JobInfo jobInfo)
        {
            var jobKey = TaskSchedulerHelper.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            //var triggerKey = TaskSchedulerHelper.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            _scheduler.PauseJob(jobKey).Wait();
            return true;
        }

        public bool ResumeJob(Customer_JobInfo jobInfo)
        {
            var jobKey = TaskSchedulerHelper.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            //var triggerKey = TaskSchedulerHelper.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            _scheduler.ResumeJob(jobKey).Wait();
            return true;
        }

        public bool RunJob(Customer_JobInfo jobInfo)
        {
            var jobKey = TaskSchedulerHelper.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            var triggerKey = TaskSchedulerHelper.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            var flag = _scheduler.CheckExists(jobKey).Result;
            if (flag)
            {
                //////存在job,先删除
                _scheduler.PauseTrigger(triggerKey).GetAwaiter().GetResult();
                _scheduler.UnscheduleJob(triggerKey).GetAwaiter().GetResult();
                _scheduler.DeleteJob(jobKey).GetAwaiter().GetResult();

                Console.WriteLine("当前job已经存在，无需调度:{0}", jobKey.ToString());
            }

            if (!string.IsNullOrWhiteSpace(jobInfo.DLLName))
            {
                var jobdata = new JobDataMap() {
                        new KeyValuePair<string, object>(TaskSchedulerHelper.JobDataMapKeyJobId, jobInfo.JobId),
                        new KeyValuePair<string, object>("JobArgs", jobInfo.JobArgs),
                        new KeyValuePair<string, object>("RequestUrl", jobInfo.RequestUrl)
                   };

                var type = ReflectHelper.GetType(jobInfo.DLLName, jobInfo.FullJobName);
                IJobDetail jobDetail = JobBuilder.Create(type)
                    .WithIdentity(jobKey)
                    .UsingJobData(jobdata)
                    .RequestRecovery(false)
                    .StoreDurably()
                    .WithDescription("使用quartz进行持久化存储")
                    .Build();

                ////两种不同的写法结果不同
                //IJobDetail jobDetail = JobBuilder.Create<TestJob2>()
                //    .WithIdentity(jobKey)
                //    .UsingJobData(jobdata)
                //    .RequestRecovery(false)
                //    //.StoreDurably()
                //    .WithDescription("使用quartz进行持久化存储")
                //    .Build();

                CronScheduleBuilder cronScheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
                ITrigger trigger = TriggerBuilder.Create()
                  //.StartAt(DateTimeOffset.Now.AddYears(-1))
                  .StartNow()
                 .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                 .ForJob(jobKey)
                 //.WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                 //.WithSchedule(cronScheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                 .WithCronSchedule(jobInfo.Cron)
                 .Build();

                if (!_scheduler.IsStarted)
                {
                    _scheduler.Start().GetAwaiter().GetResult();
                }
                _scheduler.ScheduleJob(jobDetail, trigger).GetAwaiter().GetResult(); ;
            }
            return true;
        }


        #endregion
    }
}
