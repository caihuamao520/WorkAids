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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mode;

namespace WorkAidsTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimingTask : Window
    {
        private Dictionary<string, TimingTaskInfor> TimingTaskLis;
        public TimingTask(Dictionary<string, TimingTaskInfor> ttl)
        {
            TimingTaskLis = ttl;
            InitializeComponent();

            foreach (string s in ttl.Keys)
            {
                ListWorke.Items.Add(s);
            }
        }

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
                MessageBox.Show(ee.Message, "移除异常",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

    }
}
