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
using Mode;

namespace WorkAidsTools
{
    /// <summary>
    /// TimingTask_Add.xaml 的交互逻辑
    /// </summary>
    public partial class TimingTask_Add : Window
    {
        #region 属性
        public TimingTaskInfor TaskInfor { get; set; }
        #endregion

        public TimingTask_Add()
        {
            InitializeComponent();

            InitData();
        }
        /// <summary>
        /// 初始化页面数据
        /// </summary>
        private void InitData()
        {
            this.txtTimeHouse.Text = DateTime.Now.Hour.ToString();
            this.txtTimeMinutes.Text = DateTime.Now.Minute.ToString();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            int iHours, iMinutes;

            if (int.TryParse(this.txtTimeHouse.Text.Trim(), out iHours))
            {
                if (iHours >= 24)
                {
                    MessageBox.Show("无效的时间", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.txtTimeHouse.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("无效的时间", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.txtTimeHouse.Focus();
                return;
            }
            if (int.TryParse(this.txtTimeMinutes.Text.Trim(), out iMinutes))
            {
                if (iMinutes >= 60)
                {
                    MessageBox.Show("无效的时间", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.txtTimeMinutes.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("无效的时间", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.txtTimeMinutes.Focus();
                return;
            }
            //构造时间
            DateTime? TriggerTime = this.dpDate.SelectedDate;

            if (!TriggerTime.HasValue)
            {
                MessageBox.Show("请先选择一个日期", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TriggerTime = TriggerTime.Value.AddHours(iHours);
            TriggerTime = TriggerTime.Value.AddMinutes(iMinutes);

            //时间是否小于当前系统时间
            int iCompare = TriggerTime.Value.CompareTo(DateTime.Now);
            if (iCompare <= 0)
            {
                MessageBox.Show("设定的时间不能小于或等于当前计算机时间。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.txtTimeMinutes.Focus();
                return;
            }

            TaskInfor = new TimingTaskInfor() {
                TriggerTime = (DateTime)TriggerTime,
                ReminderContent = this.txtContent.Text
            };
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
