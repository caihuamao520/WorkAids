using BLL;
using Mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    /// <summary>
    /// 数据库控制层
    /// </summary>
    public class TimingTaskControl
    {
        private TimingTaskBLL _TimingTaskBll;
        /// <summary>
        /// 新建一个数据库控制层实例
        /// </summary>
        public TimingTaskControl()
        {
            _TimingTaskBll = new TimingTaskBLL();
        }
        /// <summary>
        /// 获取所有的定时任务信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, TimingTaskInfor> GetTimingTaskInforAll()
        {
            Dictionary<string, TimingTaskInfor> ttiList = new Dictionary<string, TimingTaskInfor>();
            List<TimingTaskInfor> list = _TimingTaskBll.GetTimingTaskInforAll();

            foreach (TimingTaskInfor tti in list)
            {
                ttiList.Add(CreteTimingTaskTitle(tti), tti);
            }

            return ttiList;
        }
        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="tti">任务信息</param>
        /// <returns>同步数据后的任务信息</returns>
        public TimingTaskInfor Add(TimingTaskInfor tti)
        {
            return _TimingTaskBll.AddData(tti);
        }
        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="tti"></param>
        /// <returns></returns>
        public bool Delete(TimingTaskInfor tti)
        {
            if (_TimingTaskBll.DeleteData(tti.dbID) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 创建任务名称
        /// </summary>
        /// <param name="tti"></param>
        /// <returns></returns>
        public string CreteTimingTaskTitle(TimingTaskInfor tti)
        {
            string keyName = string.Empty;
            if (string.IsNullOrEmpty(tti.ReminderContent))
            {
                keyName = string.Format("【定时闹钟】时间:{0}", tti.TriggerTime.ToString("yyyy/MM/dd hh:mm:ss"));
            }
            else
            {
                keyName = string.Format("【备忘提醒 {0}】:{1}", tti.TriggerTime.ToString("yyyy/MM/dd hh:mm:ss"), tti.ReminderContent);
            }
            return keyName;
        }
    }
}
