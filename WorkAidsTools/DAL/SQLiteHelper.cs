using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace DAL
{
    public class SQLiteHelper
    {
        private static string ConnectionString = "Data Source=wat.dat;Pooling=true;FailIfMissing=false";
        /// <summary>
        /// 新建一个SQLite实例
        /// </summary>
        public SQLiteHelper()
        {
            InitDB();
        }

        public void InitDB()
        {
            try
            {
                string strDBFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wat.dat");
                if (!File.Exists(strDBFilePath))//数据库文件不存在时重新新建数据库
                {
                    SQLiteConnection.CreateFile(strDBFilePath);

                    //创建数据表
                    List<string> CreateTableCmdlist = new List<string>();
                    //定时任务列表
                    CreateTableCmdlist.Add("CREATE TABLE TimingTask (" +
                                                            "autoid          INTEGER        PRIMARY KEY AUTOINCREMENT," +
                                                            "TriggerTime     DATETIME," +
                                                            "ReminderContent NVARCHAR (200) " +
                                                        ");");
                    //音乐播放列表
                    CreateTableCmdlist.Add("CREATE TABLE MusicPlayList (" +
                                                            "autoid   INTEGER        PRIMARY KEY AUTOINCREMENT," +
                                                            "plName   NVARCHAR (200)," +
                                                            "FilePath NVARCHAR (255)," +
                                                            "FileSize INTEGER" +
                                                        ");");
                    foreach (string CreateTableCmd in CreateTableCmdlist)
                    {
                        ExecuteComad(CreateTableCmd);
                    }
                }
            }
            catch (Exception ee)
            {
                throw new Exception("新建数据库失败,错误消息：" + ee.Message);
            }
        }
        /// <summary>
        /// 获取一个SQLite连接对象
        /// </summary>
        /// <returns>SQLite连接对象</returns>
        public SQLiteConnection GetCoon()
        {
            SQLiteConnection coon = new SQLiteConnection(ConnectionString);
            if (coon.State != ConnectionState.Open)
            {
                coon.Open();
            }

            return coon;
        }
        /// <summary>
        /// 执行cmdText命令
        /// </summary>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="ps">参数集合</param>
        /// <returns>影响行数</returns>
        public int ExecuteComad(string cmdText, params SQLiteParameter[] ps)
        {
            SQLiteConnection conn = null;
            try
            {
                conn = GetCoon();
                SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
                if (ps != null)
                {
                    cmd.Parameters.AddRange(ps);
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                throw new Exception("执行cmdText命令失败,错误消息：" + ee.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        /// <summary>
        /// 同时执行多个cmdText命令（事务）
        /// </summary>
        /// <param name="cmdTexts">SQL命令集合</param>
        /// <param name="ps">参数集合的集合</param>
        /// <returns>是否执行成功，错误则抛异常</returns>
        public bool ExecuteComad(string[] cmdTexts, params SQLiteParameter[][] ps)
        {
            
            SQLiteConnection conn = null;
            SQLiteTransaction Trans = null;
            try
            {
                conn = GetCoon();
                Trans= conn.BeginTransaction();
                for (int i = 0; i < cmdTexts.Length; i++)
                {
                    string cmdText = cmdTexts[i];
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
                    if (ps != null)
                    {
                        if (cmdTexts.Length != ps.Length)
                        {
                            throw new Exception("执行cmdText命令失败,错误消息：cmdTexts与ps的数组长度不一致");
                        }
                        cmd.Parameters.AddRange(ps[i]);
                    }
                    cmd.ExecuteNonQuery();
                }

                Trans.Commit();
                return true;
            }
            catch (Exception ee)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("执行cmdTexts命令失败,错误消息：" + ee.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="ps">参数集合</param>
        /// <returns>数据集</returns>
        public DataTable ExecuteQueryToDataTable(string cmdText, params SQLiteParameter[] ps)
        {
            SQLiteConnection coon = null;

            try
            {
                coon = GetCoon();
                DataTable dt = new DataTable();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmdText, coon);
                if (ps != null)
                {
                    da.SelectCommand.Parameters.AddRange(ps);
                }
                da.Fill(dt);

                return dt;
            }
            catch (Exception ee)
            {
                throw new Exception("查询数据集失败,错误消息：" + ee.Message);
            }
            finally
            {
                if (coon != null)
                {
                    coon.Close();
                    coon.Dispose();
                }
            }
        }


    }
}