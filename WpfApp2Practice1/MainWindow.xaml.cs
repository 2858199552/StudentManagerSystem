using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;

namespace WpfApp2Practice1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Student> studentsList
        {
            get; set;
        } = new ObservableCollection<Student>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            switch((sender as Button).Name)
            {
                case "btnMainAdd":
                    pnlStudentList.Visibility = Visibility.Visible;
                    AddPad.Visibility = Visibility.Visible;
                    break;
                case "btnMainShow":
                    pnlStudentList.Visibility = Visibility.Visible;
                    break;
                case "btnMainRead":
                    pnlStudentList.Visibility = Visibility.Visible;
                    Menu_Load();
                    break;
                case "btnMainTotal":
                    Menu_Total();
                    pnlStudentList.Visibility = Visibility.Visible;
                    TotalPad.Visibility = Visibility.Visible;
                    break;
                case "btnMainFind":
                    pnlStudentList.Visibility = Visibility.Visible;
                    topTBFind.Visibility = Visibility.Visible;
                    break;
                case "btnMainModification":
                    pnlStudentList.Visibility = Visibility.Visible;
                    MessageBox.Show("请先选择学生");
                    break;
                case "btnMainDelete":
                    pnlStudentList.Visibility = Visibility.Visible;
                    MessageBox.Show("请先选择学生");
                    break;
                case "btnMainExit":
                    Close();
                    break;
            }
        }

        private void Top_Click(object sender, RoutedEventArgs e)
        {
            switch((sender as MenuItem).Header)
            {
                case "_Add":
                    AddPad.Visibility = Visibility.Visible;
                    break;
                case "_Refresh":
                    CollectionViewSource.GetDefaultView(NowStudentList.ItemsSource).Refresh();
                    //按理来说，其实是不用刷新的，因为这两者已经用xaml绑定
                    //发现一处异常，触发这个以后将无法使用find,因此将NowStudentList.ItemsSource = studentsList;改为现代码
                    break;
                case "S_ave":
                    Menu_Save();
                    break;
                case "_Read":
                    Menu_Load();
                    break;
                case "_Total":
                    Menu_Total();
                    TotalPad.Visibility = Visibility.Visible;
                    break;
                case "_Find":
                    if (topTBFind.Visibility == Visibility.Visible)
                    {
                        topTBFind.Text = "";
                        topTBFind.Visibility = Visibility.Collapsed;
                    }
                    else
                        topTBFind.Visibility = Visibility.Visible;
                    break;
                case "_Modification":
                    if (NowStudentList.SelectedItem != null)
                    {
                        tBName1.Text = (NowStudentList.SelectedItem as Student).Name;
                        tBStuNo1.Text = (NowStudentList.SelectedItem as Student).StuNo.ToString();
                        tBScore1_0.Text = (NowStudentList.SelectedItem as Student).Score_0.ToString();
                        tBScore1_1.Text = (NowStudentList.SelectedItem as Student).Score_1.ToString();
                        tBScore1_2.Text = (NowStudentList.SelectedItem as Student).Score_2.ToString();
                        tBScore1_3.Text = (NowStudentList.SelectedItem as Student).Score_3.ToString();
                        tBScore1_4.Text = (NowStudentList.SelectedItem as Student).Score_4.ToString();
                        tBScore1_5.Text = (NowStudentList.SelectedItem as Student).Score_5.ToString();
                        tBScore1_6.Text = (NowStudentList.SelectedItem as Student).Score_6.ToString();

                        tBName1.FontStyle = FontStyles.Italic;
                        tBName1.Foreground = Brushes.Silver;
                        tBStuNo1.FontStyle = FontStyles.Italic;
                        tBStuNo1.Foreground = Brushes.Silver;
                        tBScore1_0.FontStyle = FontStyles.Italic;
                        tBScore1_0.Foreground = Brushes.Silver;
                        tBScore1_1.FontStyle = FontStyles.Italic;
                        tBScore1_1.Foreground = Brushes.Silver;
                        tBScore1_2.FontStyle = FontStyles.Italic;
                        tBScore1_2.Foreground = Brushes.Silver;
                        tBScore1_3.FontStyle = FontStyles.Italic;
                        tBScore1_3.Foreground = Brushes.Silver;
                        tBScore1_4.FontStyle = FontStyles.Italic;
                        tBScore1_4.Foreground = Brushes.Silver;
                        tBScore1_5.FontStyle = FontStyles.Italic;
                        tBScore1_5.Foreground = Brushes.Silver;
                        tBScore1_6.FontStyle = FontStyles.Italic;
                        tBScore1_6.Foreground = Brushes.Silver;

                        ModificationPad.Visibility = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("请先选择学生后再修改");
                    break;
                case "_Delete":
                    if (NowStudentList.SelectedItem != null)
                    {
                        studentsList.Remove(NowStudentList.SelectedItem as Student);
                        StatusbarSave_Check(false);
                    }
                    else
                        MessageBox.Show("请先选择学生后再删除");
                    break;
                case "_Exit":
                    Close();
                    break;
                case "T_hank You":
                    MessageBox.Show("   系统正在茁壮成长.zZ" + "\n" + "   开发者Q:2858199552" + "        ♪(´▽｀)", "与开发者对话", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
                    break;
                case "H_ome":
                    topTBFind.Text = "";
                    topTBFind.Visibility = Visibility.Collapsed;
                    pnlStudentList.Visibility = Visibility.Collapsed;
                    MainMenu.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void Pad_Button_Return_Click(object sender, RoutedEventArgs e)
        {
            (((sender as Button).Parent as StackPanel).Parent as Border).Visibility = Visibility.Collapsed;
        }

        public class Student
        {
            public string Name { get; set; }

            public int StuNo { get; set; }

            public float Score_0 { get; set; }

            public float Score_1 { get; set; }

            public float Score_2 { get; set; }

            public float Score_3 { get; set; }

            public float Score_4 { get; set; }

            public float Score_5 { get; set; }

            public float Score_6 { get; set; }

            public float Sumscore => Score_0 + Score_1 + Score_2 + Score_3 + Score_4 + Score_5 + Score_6;
        }

        public void Button_AddStu_Click(object sender, RoutedEventArgs e)
        {
            if (tBName.Text.Length > 0 && tBStuNo.Text.Length > 0 && tBScore_0.Text.Length > 0 && tBScore_1.Text.Length > 0 && tBScore_2.Text.Length > 0 && tBScore_3.Text.Length > 0 && tBScore_4.Text.Length > 0 && tBScore_5.Text.Length > 0 && tBScore_6.Text.Length > 0)
            {
                int newIndex = 0;
                Student currentStudent = new Student()
                {
                    Name = tBName.Text,
                    StuNo = Convert.ToInt32(tBStuNo.Text),
                    Score_0 = (float)Convert.ToDouble(tBScore_0.Text),
                    Score_1 = (float)Convert.ToDouble(tBScore_1.Text),
                    Score_2 = (float)Convert.ToDouble(tBScore_2.Text),
                    Score_3 = (float)Convert.ToDouble(tBScore_3.Text),
                    Score_4 = (float)Convert.ToDouble(tBScore_4.Text),
                    Score_5 = (float)Convert.ToDouble(tBScore_5.Text),
                    Score_6 = (float)Convert.ToDouble(tBScore_6.Text),
                };
                if (studentsList.Count > 0 && currentStudent.Sumscore < studentsList.Max(x => x.Sumscore))
                {
                    newIndex = studentsList.IndexOf((studentsList.OrderByDescending(x => x.Sumscore).First(x => x.Sumscore >= currentStudent.Sumscore))) + 1;
                }
                studentsList.Insert(newIndex, currentStudent);
                tBName.Text = tBStuNo.Text = tBScore_0.Text = tBScore_1.Text = tBScore_2.Text = tBScore_3.Text = tBScore_4.Text = tBScore_5.Text = tBScore_6.Text = "";
                //MessageBox.Show("成功");
                StatusbarSave_Check(false);
            }
            else
                MessageBox.Show("请将信息填写完整后再“添加”");
        }

        public void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            NowStudentList.ItemsSource = studentsList;
        }

        public void Menu_Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Student>));
            using(Stream write=new FileStream("student_Score_Manager_List.xml", FileMode.Create))
            {
                serializer.Serialize(write, studentsList);
                StatusbarSave_Check(true);
            }
        }

        public void Menu_Load()
        {
            if (File.Exists("student_Score_Manager_List.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                using(Stream reader = new FileStream("student_Score_Manager_List.xml", FileMode.Open))
                {
                    List<Student> tempList = (List<Student>)serializer.Deserialize(reader);
                    studentsList.Clear();
                    foreach (var item in tempList.OrderByDescending(x => x.Sumscore)) 
                    {
                        studentsList.Add(item);
                    }
                }
                //MessageBox.Show("读取成功");
            }
        }

        public void Menu_Total()
        {
            tBTotalShowNone.Visibility = Visibility.Collapsed;
            sPnlTotalShow.Visibility = Visibility.Visible;
            if (studentsList.Count <= 0)
            {
                tBTotalShowNone.Visibility = Visibility.Visible;
                sPnlTotalShow.Visibility = Visibility.Collapsed;
                return;
            }
            float[] tempEachSunScore=new float[8];
            int tempSumMan = studentsList.Count;
            foreach (var item in studentsList)
            {
                tempEachSunScore[0] += item.Score_0;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[1] += item.Score_1;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[2] += item.Score_2;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[3] += item.Score_3;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[4] += item.Score_4;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[5] += item.Score_5;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[6] += item.Score_6;
            }
            foreach (var item in studentsList)
            {
                tempEachSunScore[7] += item.Sumscore;
            }
            tBTotalShow_0.Text = studentsList.Count.ToString();
            tBTotalShow_1.Text = (tempEachSunScore[0] / tempSumMan).ToString();
            tBTotalShow_2.Text = (tempEachSunScore[1] / tempSumMan).ToString();
            tBTotalShow_3.Text = (tempEachSunScore[2] / tempSumMan).ToString();
            tBTotalShow_4.Text = (tempEachSunScore[3] / tempSumMan).ToString();
            tBTotalShow_5.Text = (tempEachSunScore[4] / tempSumMan).ToString();
            tBTotalShow_6.Text = (tempEachSunScore[5] / tempSumMan).ToString();
            tBTotalShow_7.Text = (tempEachSunScore[6] / tempSumMan).ToString();
            tBTotalShow_8.Text = (tempEachSunScore[7] / tempSumMan).ToString();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if(((e.Item as Student).Name.IndexOf(topTBFind.Text,StringComparison.OrdinalIgnoreCase) >= 0))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void TopTBFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(NowStudentList.ItemsSource).Refresh();
        }

        public void Button_Modification_Click(object sender, RoutedEventArgs e)
        {
            if (tBName1.Text.Length > 0 && tBStuNo1.Text.Length > 0 && tBScore1_0.Text.Length > 0 && tBScore1_1.Text.Length > 0 && tBScore1_2.Text.Length > 0 && tBScore1_3.Text.Length > 0 && tBScore1_4.Text.Length > 0 && tBScore1_5.Text.Length > 0 && tBScore1_6.Text.Length > 0)
            {
                studentsList.Remove(NowStudentList.SelectedItem as Student);
                int newIndex = 0;
                Student currentStudent = new Student()
                {
                    Name = tBName1.Text,
                    StuNo = Convert.ToInt32(tBStuNo1.Text),
                    Score_0 = (float)Convert.ToDouble(tBScore1_0.Text),
                    Score_1 = (float)Convert.ToDouble(tBScore1_1.Text),
                    Score_2 = (float)Convert.ToDouble(tBScore1_2.Text),
                    Score_3 = (float)Convert.ToDouble(tBScore1_3.Text),
                    Score_4 = (float)Convert.ToDouble(tBScore1_4.Text),
                    Score_5 = (float)Convert.ToDouble(tBScore1_5.Text),
                    Score_6 = (float)Convert.ToDouble(tBScore1_6.Text),
                };
                if (studentsList.Count > 0 && currentStudent.Sumscore < studentsList.Max(x => x.Sumscore))
                {
                    newIndex = studentsList.IndexOf((studentsList.OrderByDescending(x => x.Sumscore).First(x => x.Sumscore >= currentStudent.Sumscore))) + 1;
                }
                studentsList.Insert(newIndex, currentStudent);
                tBName1.Text = tBStuNo1.Text = tBScore1_0.Text = tBScore1_1.Text = tBScore1_2.Text = tBScore1_3.Text = tBScore1_4.Text = tBScore1_5.Text = tBScore1_6.Text = "";
                //MessageBox.Show("成功");
                StatusbarSave_Check(false);

                ModificationPad.Visibility = Visibility.Collapsed;
            }
            else
                MessageBox.Show("请将信息填写完整后再“修改”");
        }

        private void TBName1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if((sender as TextBox).Visibility == Visibility.Visible)
            {
                (sender as TextBox).FontStyle = FontStyles.Normal;
                (sender as TextBox).Foreground = Brushes.Black;
            }
        }

        private void StatusbarSave_Check(bool isSave)
        {
            if(isSave)
            {
                statusIGIsSave.Source = new BitmapImage(new Uri("/Image/save.jpg", UriKind.Relative));
                statusTBIsSave.Text = "已保存";
            }
            else
            {
                statusIGIsSave.Source = new BitmapImage(new Uri("/Image/change.jpg", UriKind.Relative));
                statusTBIsSave.Text = "未保存";
            }
        }
    }
}
