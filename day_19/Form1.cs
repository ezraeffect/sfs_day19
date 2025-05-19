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

    public delegate void EventDelegate();
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // EventManager Class의 인스턴스를 생성
            EventManager eventManager = new EventManager();

            // Form의 메소드들을 이름과 함께 EventManager에 등록
            eventManager.Registry("자동차", Car);
            eventManager.Registry("비행기", Airplane);
            eventManager.Registry("헬리콥터", Helicopter);
            eventManager.Registry("기차", Train);

            eventManager.Remove("헬리콥터");

            eventManager.Excute("자동차");
        }

        public void Car()
        {
            Console.WriteLine("자동차 : 부릉부릉");
        }

        public void Airplane()
        {
            Console.WriteLine("비행기 : 휘유웅");
        }

        public void Helicopter()
        {
            Console.WriteLine("헬리콥터 : 푸드드득");
        }

        public void Train()
        {
            Console.WriteLine("기차 : 철커덕");
        }

        public class EventManager
        {
            public string EventName { get; set; }

            // Event 이름, EventDelegate가 각각 Key, Value로 된 Dictionary 생성
            public Dictionary<string, EventDelegate> dict = new Dictionary<string, EventDelegate>();

            public void Registry(string eventName, EventDelegate eventDelegate)
            {
                // Dictionary에 이벤트를 추가
                if (!string.IsNullOrEmpty(eventName) && !string.IsNullOrWhiteSpace(eventName))
                {
                    dict.Add(eventName, eventDelegate);
                }
            }

            public void Remove(string eventName)
            {
                // Dictionary에서 이벤트를 제거
                dict.Remove(eventName);
            }

            public void Excute(string eventName)
            {
                // Dictionary 안에 있는 이벤트를 선택하여 Invoke()
                dict[eventName]?.Invoke();
            }
        }
    }
}
