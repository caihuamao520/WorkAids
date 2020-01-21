using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mode
{
    /// <summary>
    /// 定时任务信息
    /// </summary>
    public class TimingTaskInfor
    {
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { get; set; }
        /// <summary>
        /// 提醒内容
        /// </summary>
        public string ReminderContent { get; set; }
    }
}
