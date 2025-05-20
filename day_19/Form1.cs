using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Remoting.Channels;

namespace day_19
{
    public partial class Form1 : Form
    {
        BackgroundWorker worker = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();


            // 진행률 관련 옵션
            worker.WorkerReportsProgress = true;

            // 프로그래스 바 기본 범위 설정
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;

            // 이벤트 연결
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

        }

        // 작업 완료 시 MessageBox 출력
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show("완료됨");
        }

        // UI Thread (ProgressBar Value Update)
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            progressBar1.Value = e.ProgressPercentage;
        }

        // DoWork가 실행할 메서드
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // throw new NotImplementedException();
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(30); // 30ms 지연
                worker.ReportProgress(i); // 진행 상황 보고
            }
        }

        // Button 클릭 시, BGW 작업을 시작
        private void button_start_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }

       
    }
}
