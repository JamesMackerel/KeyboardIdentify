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

        private bool isStart;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
        }
    }
}
