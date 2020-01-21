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

        private void btnTimingTask_Click(object sender, RoutedEventArgs e)
        {
            TimingTask tt = new TimingTask(TimingTaskLis);
            tt.ShowDialog();
        }
    }
}
