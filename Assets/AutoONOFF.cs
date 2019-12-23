using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoONOFF : MonoBehaviour
{
    private int AutoNum = 0;
    [SerializeField]
    private Text AutoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (AutoNum % 2 == 0)
        {
            NoteTimming.AutoTri = true;
            AutoText.text = "オートモード:ON";
            AutoNum++;
        }
        else if (AutoNum % 2 == 1)
        {
            NoteTimming.AutoTri = false;
            AutoText.text = "オートモード:OFF";
            AutoNum++;
        }
    }
}
