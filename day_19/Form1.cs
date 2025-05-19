using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace day_19
{
    /*
     * BackgroundWorker
     * - 백그라운드에서 작업을 처리 할 수 있게 해주는 클래스
     * - WinForm, WPF에서 UI 멈춤 없이 무거운 작업을 처리할 때 사용
     * - System.ComponentModel 네임스페이스에 정의되어 있음
     * - 별도 스레드를 생성하고 관리하는 비동기 도우미
     *
     * Method
     * DoWork : 백그라운드에서 실행할 작업 정의
     * ProgressChanged : 작업중 진행률 (0~100)을 UI에 알림
     * RunWorkerCompleted : 작업 완료 후 실행
     * RunWorkerAsync : 비동기 작업 시작 메서드 -> 이후에 DoWork가 일함
     * 
     * Why
     * - 무거운 작업을 UI와 분리시켜 실행 할 수 있음
     * - ReportProgress로 진행률을 UI에 안전하게 전달
     * - Complete 이벤트로 작업이 끝났을 때 후처리 가능
     */

    public partial class Form1 : Form
    {
        BackgroundWorker worker;

        public Form1()
        {
            InitializeComponent();

            // 객체 생성
            worker = new BackgroundWorker();

            // 옵션 설정
            worker.WorkerReportsProgress = true; // ReportProgress() 사용 가능하게 설정
            // ReportProgress()
            // 작업 중간 진행률 보고하는 메서드
            // 진행률 : 0~100 사이의 정수 (백분율)

            worker.WorkerSupportsCancellation = true; // 취소 기능 사용 설정

            worker.DoWork += Worker_DoWork; // 작업 내용 작성할 메서드 연결
            worker.ProgressChanged += Worker_ProgressChanged; // 진행 상황 UI 갱신 메서드
            worker.RunWorkerCompleted += Worker_Completed; // 작업 완료 후 실행할 메서드 연결
        }

        // 시작 버튼 클릭시 작업 진행
    

        // 실제 작업 실행 (UI 스레드와 분리)
        // - 백그라운드 스레드에서 실행
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                progressBar1.Value += 1;
                if (progressBar1.Value >= 100) break;
                // while 루프가 너무 빠르게 실행되면서 UI 스레드를 점유해버림. => 응답없음 오류
            }
            /*
            for (int i = 0;i <= 100; i++)
            {
                Thread.Sleep(100);
                worker.ReportProgress(i); // 현재 진행 상황 i 값을 UI 스레드로 전달한다.
                // UI 스레드? - main Theread
            }
            */
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            /* .IsBusy
            * BackgroundWorker가 현재 실행 중인지 아닌지 bool로 알려줌
            * 실행 중이면 True, 대기 상태면 False
            */
            {
                lblStatus.Text = "작업 시작";
                progressBar1.Value = 0; // 프로그레스바 초기설정
                worker.RunWorkerAsync(); // 비동기 작업 실행
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            // e.ProgressPercentage는 ReportProgress()에서 전달 한 값 (백분율)
            lblStatus.Text = $"{e.ProgressPercentage}% 진행 중";
        }

        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus.Text = "작업 완료";
        }
    }
}
