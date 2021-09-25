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
using System.Threading;

namespace ProducerAndConsumer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _bufferSize, _num, _producerNum, _consumberNum,_runTime;
        private Semaphore _full, _empty;
        private Mutex _mutex;


        public MainWindow()
        {
            InitializeComponent();
        }
        public void createthread()
        {
            Thread[] consumerThread = new Thread[_consumberNum];//create the number of consumer threading.
            Thread[] producerThread = new Thread[_producerNum];
            for(int i = 0; i < _producerNum; i++)
            {
                producerThread[i] = new Thread(new ParameterizedThreadStart(produce));
                producerThread[i].IsBackground = true;
                producerThread[i].Start(i + 1);
                Thread.Sleep(100);
            }
            for(int j = 0; j < _consumberNum; j++)
            {
                consumerThread[j] = new Thread(new ParameterizedThreadStart(consume));
                consumerThread[j].IsBackground = true;
                consumerThread[j].Start(j + 1);
                Thread.Sleep(100);
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            Showlist.Text = " ";
            _bufferSize = int.Parse(bufferSize.Text);
            _producerNum = int.Parse(producer.Text);
            _consumberNum = int.Parse(consumer.Text);
            _full = new Semaphore(0, _bufferSize);
            _empty = new Semaphore(_bufferSize, _bufferSize);
            _mutex = new Mutex();
            createthread();
        }
        private void produce(object id)
        {
            while (true)
            {
                _empty.WaitOne();
                _mutex.WaitOne();
                _num++;
                _runTime++;
                this.Dispatcher.Invoke(() => { Showlist.AppendText(_runTime + "th producer  " + "The position is:" + id.ToString() + "  " + (_bufferSize - _num)+"\n"); });
                _mutex.ReleaseMutex();
                _full.Release();
            }
        }
        private void consume(object id)
        {
            while (true)
            {
                _full.WaitOne();
                _mutex.WaitOne();
                _num--;
                _runTime++;
                this.Dispatcher.Invoke(() => { Showlist.AppendText(_runTime + "th consumer  " + "The free position is:" + id.ToString() + "  " + (_bufferSize - _num) + "\n"); });
                _mutex.ReleaseMutex();
                _empty.Release();
            }
        }
    }
}
