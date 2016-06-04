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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Collections.ObjectModel;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KeyboardIdentify
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            PropertyChanged += UpdateComboBoxItemsSource;

            UpdateExperimentCollection();
        }

        private ObservableCollection<ExperimentModel> experimentCollection;
        private ObservableCollection<ExperimentDataModel> experimentDataCollection;
        private ObservableCollection<ExperimentDataModel> editExperimentDataCollection;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ExperimentModel> ExperimentCollection
        {
            get
            {
                return experimentCollection;
            }

            set
            {
                experimentCollection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExperimentDataModel> ExperimentDataCollection
        {
            get
            {
                return experimentDataCollection;
            }

            set
            {
                experimentDataCollection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExperimentDataModel> EditExperimentDataCollection
        {
            get
            {
                return editExperimentDataCollection;
            }

            set
            {
                editExperimentDataCollection = value;
                OnPropertyChanged();
            }
        }

        private void NewExperiment_Click(object sender, RoutedEventArgs e)
        {
            NewExperimentDialog dlg = new NewExperimentDialog();
            dlg.ShowDialog();

            if(dlg.Result == MessageBoxResult.OK)
            {
                ExperimentModel exp = new ExperimentModel();
                exp.Password = dlg.Password;
                exp.Save();
                UpdateExperimentCollection();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void UpdateComboBoxItemsSource(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ExperimentCollection": ExperimentCombobox.ItemsSource = ExperimentCollection; break;
            }
        }

        private void UpdateExperimentCollection()
        {
            ExperimentCollection = DatabaseManager.Instance.GetExperimentModels() as ObservableCollection<ExperimentModel>;
        }

        private void UpdateExperimentDataCollection()
        {
            ExperimentDataCollection = DatabaseManager.Instance.GetExperimentDataModels((int)ExperimentCombobox.SelectedValue) 
                as ObservableCollection<ExperimentDataModel>;
        }

        private void ExperimentCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            ExperimentGrid.DataContext = cb.SelectedItem as ExperimentModel;
        }

        private bool recordStart = false;
        private bool verifyStart = false;

        KeyboardTimeline timeline;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //如果没有开始记录过程或者验证过程，就直接返回
            if (!(recordStart || verifyStart))
                return;
            //过滤掉这些键
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                return;

            if(e.Key == Key.Enter)
            {
                if(InputBox.Text == PasswordShow.Text)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to save?", "Save", MessageBoxButton.YesNo);
                    if(result == MessageBoxResult.Yes)
                    {
                        SaveData();
                    }
                    InputBox.Clear();
                }
                else
                {
                    MessageBox.Show("Incorrect password!");
                }
            }
            timeline.MarkDown(e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(recordStart || verifyStart))
                return;
            //过滤掉这些键
            if (e.Key == Key.Enter || e.Key == Key.LeftShift || e.Key == Key.RightShift)
                return;

            timeline.MarkUp(e.Key);

            ////记录数据
            //TimeSpan t = DateTime.Now - (DateTime)timer;
            //tmpVector.Add(t.TotalMilliseconds);

            //string CurrentPassword = ((ExperimentModel)ExperimentCombobox.SelectedItem).Password;
            ////对于有n个字母的串，按下时间记录的数量与按键间隔时间记录的熟练的和等于n-1
            ////如果进入if语句块则表示已经记录完毕了
            //if(tmpVector.Count == CurrentPassword.Length - 1)
            //{
            //    //记录数据的逻辑
            //    if (recordStart)
            //    {
            //        MessageBoxResult result = MessageBox.Show("A record has been recorded, do you want to save?", "Save", MessageBoxButton.YesNo);
            //        if(result == MessageBoxResult.Yes)
            //        {
            //            SaveData();
            //        }
            //    }

            //    //验证的逻辑
            //    if (verifyStart)
            //    {
            //        VerifyPassword();
            //    }
            //}

            //timer = DateTime.Now;
        }

        private void SaveData()
        {
            int ExpID = ((ExperimentModel)ExperimentCombobox.SelectedItem).ID;
            Vector v = timeline.ToVector();
            ExperimentDataModel edm = new ExperimentDataModel(null, v.ToString(), ExpID);
            edm.Save();
        }

        private bool VerifyPassword()
        {
            //TODO:add verify password logic
            verifyStart = false;
            return true;
        }

        #region initialize variables. button start, end and verify.
        private void InitializeVariable()
        {
            timeline = new KeyboardTimeline();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeVariable();
            recordStart = true;
        }

        private void OverButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeVariable();
            recordStart = false;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeVariable();
            verifyStart = true;
        }
        #endregion
    }
}
