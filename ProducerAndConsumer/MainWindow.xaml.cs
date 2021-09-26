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
        //definite bufferSize, the number of product, the number of consumer & producer, the runnoing times.
        private int _bufferSize, _productNum, _producerNum, _consumerNum, _runTime;
        //definite Semaphore and Mutex.
        private Semaphore _full, _empty;
        private Mutex _mutex;


        public MainWindow()
        {
            InitializeComponent();
        }
        public void createthread()
        {
            Thread[] consumerThread = new Thread[_consumerNum];//create the number of consumer threading.
            Thread[] producerThread = new Thread[_producerNum];
            for(int i = 0; i < _producerNum; i++)
            {
                producerThread[i] = new Thread(new ParameterizedThreadStart(produce));//create the threading
                producerThread[i].IsBackground = true;//when the windows threading is killed, the background threading will be killed too.
                producerThread[i].Start(i + 1);//start running    
            }
            for(int j = 0; j < _consumerNum; j++)
            {
                consumerThread[j] = new Thread(new ParameterizedThreadStart(consume));
                consumerThread[j].IsBackground = true;
                consumerThread[j].Start(j + 1);
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            Showlist.Text = " ";
            _bufferSize = int.Parse(bufferSize.Text);
            _producerNum = int.Parse(producer.Text);
            _consumerNum = int.Parse(consumer.Text);
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
                _runTime++;
                _productNum++;
                this.Dispatcher.Invoke(() => { Showlist.AppendText(_runTime + "th running times     " + "The producer is:" + id.ToString() + "     You have " + (_bufferSize - _productNum) + " free position" + "\n"); });
                Thread.Sleep(1000);//sleep a while or you will feel too fast to look at the process               
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
                _productNum--;
                _runTime++;
                this.Dispatcher.Invoke(() => { Showlist.AppendText(_runTime + "th running times     " + "The consumer is:" + id.ToString() + "     You have " + (_bufferSize - _productNum) + " free position" + "\n"); });
                Thread.Sleep(1000);
                _mutex.ReleaseMutex();
                _empty.Release();
            }
        }
    }
}
