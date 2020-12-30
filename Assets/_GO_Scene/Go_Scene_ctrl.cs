using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Go_Scene_ctrl : MonoBehaviour
{ 
    public delegate void TASK_GS();

    public struct TASKDATA
    {
        public TASK_GS task;
        public float percent;
        public string text;
    }

    [SerializeField]
    Slider bar;

    [SerializeField]
    Text text_head, text_percent, text_discription;
     
    ulong vv;
    int cnt;
    public TASKDATA[] taskList;
     
    void Start()
    {
        SetTASK();
        HEADs();
    }

    void HEADs()
    {
        vv++;
        text_head.text = "Now Loading" + "".ToString().PadLeft((int)(vv % 4), '.');
        Invoke("HEADs", 0.4f);
    }
     
     
    void Update()
    {
        if (cnt == taskList.Length)
        {
            NextScene();
        }
        else
        {
            taskList[cnt].task();
            var c = taskList[cnt].percent;
            var discription = taskList[cnt].text;
            bar.value = c < 0 ? 0 : (c > 1 ? 1 : c); 
            text_percent.text = (int)((c < 0f ? 0f : (c > 1f ? 1f : c)) * 100f) + "%";
            text_discription.text = discription;
            ++cnt;
        }
    }




    //TASK==========================================
    void SetTASK()
    {
        taskList = new TASKDATA[]
       {
            new TASKDATA() { task = TASK00, percent = 0.1f, text = "" },
            new TASKDATA() { task = TASK00, percent = 0.3f, text = "" },
            new TASKDATA() { task = TASK00, percent = 0.5f, text = "" },
            new TASKDATA() { task = TASK00, percent = 0.9f, text = "" },
       };
    }
    void NextScene()
    {
        //シーン遷移
    }

    void TASK00() { }
    void TASK01() { }
    void TASK02() { }
    void TASK03() { }
    void TASK04() { }
}
