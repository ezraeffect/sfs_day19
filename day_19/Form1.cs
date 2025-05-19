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
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

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

/*• Thread 및 Thread.Sleep 메소드를 사용하여 아래 레이스 경기 코드를 제작 
 * 1. 총 5명의 참가자(차량)가 동시에 경주 시작. (차량마다 스레드 생성)
 * 2. 각 차량은 랜덤한 시간 간격으로 전진함. (0.1초 ~ 1초)
 * 3. 차량이 결승선에 도달하면 차량 이름 및 시간을 출력
 * 4. 모든 차량이 결승선에 도달하면 레이스 종료 메시지 출력 및 경기 종료
 * 5.   (Hint) DateTime, TimeSpan, List<Thread> 활용.
 */
namespace day_19
{
    public partial class Form1 : Form
    {

        private static int finishCount = 0;
        private static int nextRank = 1;
        private const int totalCars = 5;
        private static int winnerNumber = 0;

        // 동기화를 위한 lock 객체
        private static object lockObj = new object();

        private Car car1, car2, car3, car4, car5;

        public Form1()
        {
            InitializeComponent();

            // 차량 객체 생성
            car1 = new Car(1, progressBar1, label1, this);
            car2 = new Car(2, progressBar2, label2, this);
            car3 = new Car(3, progressBar3, label3, this);
            car4 = new Car(4, progressBar4, label4, this);
            car5 = new Car(5, progressBar5, label5, this);
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            // 멀티 스레드
            List<Thread> threads = new List<Thread>
            {
                new Thread(car1.Run),
                new Thread(car2.Run),
                new Thread(car3.Run),
                new Thread(car4.Run),
                new Thread(car5.Run),
            };

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            button_start.Enabled = false;
            label_result.Text = "경주 시작!";
        }

        private void ShowResult()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => label_result.Text = $"경주 종료! 우승자: 차량 {winnerNumber}"));
            }
            else
            {
                label_result.Text = $"경주 종료! 우승자: 차량 {winnerNumber}";
            }
            button_start.Enabled = true;
        }

        public class Car
        {
            public int Distance { get; set; }
            public int Rank { get; set; }
            public int Number {  get; set; }
            public System.Windows.Forms.ProgressBar ProgressBar { get; set; }
            
            public System.Windows.Forms.Label Label { get; set; }

            public Form1 Form { get; set; }

            public Car(int carNumber ,System.Windows.Forms.ProgressBar progressBar, System.Windows.Forms.Label label, Form1 form)
            {
                Distance = 0;
                Rank = 0;
                Number = carNumber;
                ProgressBar = progressBar;
                Label = label;
                Form = form;
            }

            // 경주 Method
            // - 경주
            // - 승부 결과 계산

            public void Run()
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int distance = 0;

                while (distance < 100)
                {
                    Thread.Sleep(rnd.Next(100, 1001)); // 0.1~1.0초
                    distance++;

                    UpdateUI(distance); // UI 전용 메서드 호출
                }

                lock (lockObj)
                {
                    Rank = nextRank++;
                    finishCount++;

                    if (finishCount == 1)
                        winnerNumber = this.Number;

                    if (finishCount == totalCars)
                        Form.ShowResult();
                }
            }

            // progressBar, Label Update Method
            public void UpdateUI(int distance)
            {
                if (ProgressBar.InvokeRequired)
                {
                    ProgressBar.Invoke(new MethodInvoker(() =>
                    {
                        ProgressBar.Value = Math.Min(distance, 100);
                        Label.Text = $"{distance}/100";
                    }));
                }
                else
                {
                    ProgressBar.Value = Math.Min(distance, 100);
                    Label.Text = $"{distance}/100";
                }
            }
        }
    }
}
