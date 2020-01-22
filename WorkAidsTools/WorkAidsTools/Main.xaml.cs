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

namespace WorkAidsTools
{
    /// <summary>
    /// Mian.xaml 的交互逻辑
    /// </summary>
    public partial class Mian : Window
    {
        private Dictionary<string, TimingTaskInfor> TimingTaskLis;
        public Mian()
        {
            InitializeComponent();
            TimingTaskLis = new Dictionary<string, TimingTaskInfor>();
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
                string strItenmName = string.Empty;
                TimingTaskInfor tti = ttadd.TaskInfor;
                string str = tti.ReminderContent;

                if (string.IsNullOrEmpty(str))
                {
                    strItenmName = string.Format("【定时闹钟】时间:{0}", tti.TriggerTime.ToString("yyyy/MM/dd hh:mm:ss"));
                }
                else
                {
                    strItenmName = string.Format("【备忘提醒 {0}】:{1}", tti.TriggerTime.ToString("yyyy/MM/dd hh:mm:ss"), tti.ReminderContent);
                }
                this.ListWorke.Items.Add(strItenmName);
                TimingTaskLis.Add(strItenmName, tti);
            }
        }

        private void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object obj = this.ListWorke.SelectedValue;

                if (obj == null) return;
                string strSelectItemName = obj.ToString();

                TimingTaskLis.Remove(strSelectItemName);

                this.ListWorke.Items.Remove(strSelectItemName);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "移除异常", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


    }
}
