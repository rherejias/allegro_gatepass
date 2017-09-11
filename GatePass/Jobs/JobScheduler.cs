using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.Jobs
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Foo>().Build();

            //default
            ITrigger trigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(30)
                .RepeatForever())
            .Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //.StartNow()
            //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(11, 11)) // execute job daily at 9:30
            //.Build();

            scheduler.ScheduleJob(job, trigger);
            //scheduler.Shutdown();
        }
    }
}