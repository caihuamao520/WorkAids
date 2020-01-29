using Control;
using Mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WorkAidsTools
{
    /// <summary>
    /// Mian.xaml 的交互逻辑
    /// </summary>
    public partial class Mian : Window
    {
        private Dictionary<string, TimingTaskInfor> TimingTaskLis;
        private DispatcherTimer timeMain;
        private TimingTaskControl _TimingTaskControl;
        public Mian()
        {
            InitData();
            InitializeComponent();
            InitShow();


            timeMain = new DispatcherTimer();
            timeMain.Interval = TimeSpan.FromSeconds(1);
            timeMain.Tick += timeMain_Tick;
            timeMain.Start();
        }
        //初始化数据
        private void InitData()
        {
            _TimingTaskControl = new TimingTaskControl();
            TimingTaskLis = _TimingTaskControl.GetTimingTaskInforAll();
        }
        //初始化显示数据
        private void InitShow()
        {
            foreach (string keyName in TimingTaskLis.Keys)
            {
                this.ListWorke.Items.Add(keyName);
            }
        }

        void timeMain_Tick(object sender, EventArgs e)
        {
            if (TimingTaskLis.Count <= 0) return;
            List<string> RemoveKeyName = new List<string>();
            foreach (string keyName in TimingTaskLis.Keys)
            {
                TimingTaskInfor tti = TimingTaskLis[keyName];

                int iCompareValue = tti.TriggerTime.CompareTo(DateTime.Now);
                if (iCompareValue <= 0)
                {
                    string strContent = tti.ReminderContent;

                    if (string.IsNullOrEmpty(strContent))
                    {
                        MessageBox.Show("你设置的闹钟时间到了！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(strContent, "备忘录", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    RemoveKeyName.Add(keyName);
                }
            }

            foreach (string key in RemoveKeyName)
            {
                RemoveTimingTask(key);
            }
        }

        private void ShowGridContent(string grindName)
        {
            this.grTimingTask.Visibility = Visibility.Collapsed;
            this.grMusic.Visibility = Visibility.Collapsed;
            this.grNotepad.Visibility = Visibility.Collapsed;

            switch (grindName)
            {
                case "timingTask":
                    this.grTimingTask.Visibility = Visibility.Visible;
                    break;
                case "music":
                    this.grMusic.Visibility = Visibility.Visible;
                    break;
                case "notepad":
                    this.grNotepad.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void btnTimingTask_Click(object sender, RoutedEventArgs e)
        {
            ShowGridContent("timingTask");
        }

        private void btnMusic_Click(object sender, RoutedEventArgs e)
        {
            ShowGridContent("music");
        }

        private void btnNotepad_Click(object sender, RoutedEventArgs e)
        {
            ShowGridContent("notepad");
        }

        #region 定时任务
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TimingTask_Add ttadd = new TimingTask_Add();
            if (ttadd.ShowDialog() == true)
            {
                TimingTaskInfor tti = ttadd.TaskInfor;
                string strItenmName = _TimingTaskControl.CreteTimingTaskTitle(tti);

                if (this.ListWorke.Items.Contains(strItenmName))
                {
                    MessageBox.Show(string.Format("已存在 {0} 不能重复添加！", strItenmName), "重复添加", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                else
                {
                    if (_TimingTaskControl.Add(tti).dbID > 0)//添加数据直数据库
                    {
                        this.ListWorke.Items.Add(strItenmName);
                        TimingTaskLis.Add(strItenmName, tti);
                    }
                    else //获取信息失败 更新整个列表
                    {
                        UpdatTimingTaskList();                        
                    }
                }
            }
        }

        private void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object obj = this.ListWorke.SelectedValue;
                if (obj == null) return;

                RemoveTimingTask(obj.ToString());
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "移除异常", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //移除一个任务
        public bool RemoveTimingTask(string timingTaskTitle)
        {
            if (_TimingTaskControl.Delete(TimingTaskLis[timingTaskTitle]))
            {
                TimingTaskLis.Remove(timingTaskTitle);
                this.ListWorke.Items.Remove(timingTaskTitle);

                return true;
            }
            else
            {
                return false;
            }
        }
        //更新任务列表
        public void UpdatTimingTaskList()
        {
            TimingTaskLis = _TimingTaskControl.GetTimingTaskInforAll();
            this.ListWorke.Items.Clear();
            foreach (string keyName in TimingTaskLis.Keys)
            {
                this.ListWorke.Items.Add(keyName);
            }
        }

        #endregion


    }
}
