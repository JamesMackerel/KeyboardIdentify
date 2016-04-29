using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyboardIdentify
{
    /// <summary>
    /// NewExperimentDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NewExperimentDialog : Window
    {
        private string password;
        private MessageBoxResult result;

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public MessageBoxResult Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        public NewExperimentDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            password = PasswordInput.Text;
            this.result = MessageBoxResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.result = MessageBoxResult.Cancel;
            this.Close();
        }
    }
}
