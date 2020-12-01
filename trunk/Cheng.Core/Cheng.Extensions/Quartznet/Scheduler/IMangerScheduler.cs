using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cheng.Extensions.Quartznet
{
    public interface IMangerScheduler<T> where T : class
    {
        /// <summary>
        /// 删除job
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        bool DeleteJob(T jobInfo);

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        bool PauseJob(T jobInfo);

        /// <summary>
        /// 重启任务
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        bool ResumeJob(T jobInfo);

        /// <summary>
        /// 修改任务周期
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        bool ModifyJobCron(T jobInfo);

        /// <summary>
        /// 运行job
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        bool RunJob(T jobInfo);
    }
}
