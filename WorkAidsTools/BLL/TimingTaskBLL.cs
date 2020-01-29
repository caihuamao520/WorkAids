using DAL;
using Mode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Wat数据库业务逻辑
    /// </summary>
    public class TimingTaskBLL
    {
        private SQLiteHelper _myDB;
        /// <summary>
        /// 新建一个实例
        /// </summary>
        public TimingTaskBLL()
        {
            _myDB = new SQLiteHelper();
        }
        /// <summary>
        /// 获取所有定时任务
        /// </summary>
        public List<TimingTaskInfor> GetTimingTaskInforAll()
        {
            List<TimingTaskInfor> TimingTaskLis = new List<TimingTaskInfor>();
            string strSQL = "select * from TimingTask order by TriggerTime,autoid";
            DataTable dt = _myDB.ExecuteQueryToDataTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                TimingTaskInfor tti = new TimingTaskInfor() {
                    dbID = Convert.ToInt32(dr["autoid"]),
                    TriggerTime = Convert.ToDateTime(dr["TriggerTime"]),
                    ReminderContent = dr["ReminderContent"].ToString()
                };

                TimingTaskLis.Add(tti);
            }
            return TimingTaskLis;
        }
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="tti"></param>
        /// <returns></returns>
        public TimingTaskInfor AddData(TimingTaskInfor tti)
        {
            string SQL = "insert into TimingTask(TriggerTime,ReminderContent)values(@triggerTime,@reminderContent);select last_insert_rowid();";
            SQLiteParameter[] ps = new SQLiteParameter[]{
            new SQLiteParameter("triggerTime",tti.TriggerTime),
            new SQLiteParameter("reminderContent",tti.ReminderContent)
            };

            tti.dbID = Convert.ToInt32(_myDB.InsertDataBrakeAutoid(SQL, ps));

            return tti;
        }
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="autoid"></param>
        /// <returns></returns>
        public int DeleteData(int autoid)
        {
            string SQL = string.Format("delete from TimingTask where autoid={0}", autoid);
            return _myDB.ExecuteComad(SQL);
        }
        /// <summary>
        /// 清空所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteALLData()
        {
            string SQL = "delete table TimingTask";
            return _myDB.ExecuteComad(SQL);
        }

    }
}
