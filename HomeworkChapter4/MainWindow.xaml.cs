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
using System.Windows.Forms;
using System.IO;

namespace HomeworkChapter4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog select = new System.Windows.Forms.FolderBrowserDialog();
            filePath.IsEnabled = true;
            if (select.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                filePath.Text = select.SelectedPath;
            }
            filePath.IsEnabled = false;
        }

        private void Combine_Click(object sender, RoutedEventArgs e)
        {
            //string[] fileEntries = Directory.GetFiles(filePath.Text, "*.txt");
            CombineFile(Directory.GetFiles(filePath.Text));                  
        }
        public void CombineFile(String[] infileName)
        {
            int b;
            int n = infileName.Length;
            FileStream[] fileIn = new FileStream[n];
            using (FileStream fileOut = new FileStream(filePath.Text+@"\result.txt", FileMode.Create))
            {
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        fileIn[i] = new FileStream(infileName[i], FileMode.Open);
                        while ((b = fileIn[i].ReadByte()) != -1)
                            fileOut.WriteByte((byte)b);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        fileIn[i].Close();
                    }

                }
            }
        }
    }
}
