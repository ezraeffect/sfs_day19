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

/*
 * Thread
 * - C#에서 멀티스레드 작업을 수행할 수 있게 해주는 기본 클래스
 * - System.Threading 네임스페이스에 정의됨
 * 
 * Thread 클래스를 사용하면
 * - 개발자가 직접 스레드를 만들고 제어할 수 있음
 * - 백그라운드에서 별도 작업을 실행 가능
 * 
 * Why
 * 1. 긴 작업(다운로드, 대규모 연산)을 실행할 때 UI가 멈추지않도록
 * 2 여러 작업을 동시에 실행하고 싶을떄 (병렬처리)
 * 
 * Method
 * - Start() : 스레드 실행
 * - Sleep(ms) : 지정시간 동안 현재 스레드 일시 정지
 * - Abort() : 스레드 강제 종료 (권장하지 않음)
 * - Join() : 특정 스레드가 끝날 떄까지 메인 스레드 대기
 * 
 * 너무 많은 스레드를 만들면 오히려 성능 저하
 * UI 스레드와의 충돌은 반드시 Invoke 처리해야 함
 * 
 */
namespace day_19
{
    public partial class Form1 : Form
    {
        // 동기화를 위한 lock 객체
        private static readonly object lockObject = new object();

        static int sharedNum = 0; 
        public Form1()
        {
            InitializeComponent();

            // 멀티 스레드
            // 두개의 스레드 생성
            Thread thread1 = new Thread(UpdateData1);
            Thread thread2 = new Thread(UpdateData2);

            thread1.Start();
            thread2.Start();

        }

        private void UpdateData1()
        {
            lock (lockObject)
            {
                for (int i = 0; i < 10; i++)
                {
                    sharedNum++;
                    Thread.Sleep(10); // CPU 점유 방지 딜레이

                    // UI 스레드 분기 처리

                    if (textBox1.InvokeRequired)
                    {
                        // bool Type
                        // true면 지금 코드가 UI 스레드가 아닌 다른 스레드에서 실행 중이다는 뜻
                        // UI 업데이트는 메인 스레드에서 실행되도록 위임
                        textBox1.Invoke(new MethodInvoker(() => { textBox1.Text += $"1: {sharedNum}\r\n"; }));
                    }
                    else
                    {
                        // 현재 스레드가 UI 스레드 인 경우
                        textBox1.Text += $"1: {sharedNum}\r\n";
                    }
                }
            }
        }

        private void UpdateData2()
        {
            lock (lockObject)
            {
                for (int i = 0; i < 10; i++)
                {
                    sharedNum++;
                    Thread.Sleep(10); // CPU 점유 방지 딜레이

                    // UI 스레드 분기 처리

                    if (textBox1.InvokeRequired)
                    {
                        // bool Type
                        // true면 지금 코드가 UI 스레드가 아닌 다른 스레드에서 실행 중이다는 뜻
                        // UI 업데이트는 메인 스레드에서 실행되도록 위임
                        textBox1.Invoke(new MethodInvoker(() => { textBox1.Text += $"2: {sharedNum}\r\n"; }));
                    }
                    else
                    {
                        // 현재 스레드가 UI 스레드 인 경우
                        textBox1.Text += $"2: {sharedNum}\r\n";
                    }
                }
            }
        }
    }
}
