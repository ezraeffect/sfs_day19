using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace day_19
{
    // 일반 델리게이트
    public delegate void Notify();

    // 1. event 키워드 사용
    public delegate void Notify2();
    public partial class Form1 : Form
    {
        // 메서드 정의
        static void AlarmMessage()
        {
            Console.WriteLine("알람 울림");
        }
        public Form1()
        {
            InitializeComponent();

            Alarm alarm = new Alarm();

            Alarm2 alarm2 = new Alarm2();

            alarm.OnRing += AlarmMessage;


            // 4. Event Delegate에 Method 할당
            alarm2.OnRing += AlarmMessage;

            alarm.OnRing();

            // 5. Event Delegate 실행
            // alarm2.OnRing(); <= Compile Error
            // event는 외부에서 직접 호출할 수 없도록 제한된 접근 권한을 가지기 때문
            alarm2.Trigger();
        }

        // 이벤트 메서드
        // - VS에서 자동으로 "delegate 형식(EventHandler delegate)"에 맞춰 만들어준 이벤트 핸들러 메서드.
        public void button_Click(object sender, EventArgs e) { }
        /*
         * Button 클래스 내부
         * public event EventHandler Click; // delegate 기반 이벤트
         * button.Click += new EventHandler(button_Click);
         * 
         * button_Click() 함수는 EventHandler Delegate에 연결되는 핸들러 함수임.
         * 
         * EventHandler : .NET에서 미리 정의된 delegate Type
         * 
         * object sender: 이벤트를 발생시킨 객체
         * EventArgs e: 이벤트에 대한 정보
         * 
         * 이미 정의 된 이벤트(Button.Click)을 사용할때 이벤트에 연결할 메서드만 만들면 됨.
         * 직접 이벤트를 만들고 싶다면 event 키워드 필요.
         */

        public class Alarm
        {
            public Notify OnRing; // Notify 타입 변수 선언
        }

        // 3. event 클래스 정의

        public class Alarm2
        {
            public event Notify2 OnRing; // Notify 타입 변수 선언

            public void Trigger()
            {
                if (OnRing != null)
                {
                    OnRing();
                }
            }
        }
    }
}
