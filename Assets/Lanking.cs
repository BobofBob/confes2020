using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lanking : MonoBehaviour
{
    [SerializeField]
    private Text[] Easy = new Text[3];
    [SerializeField]
    private Text[] Normal = new Text[3];
    [SerializeField]
    private Text[] Hard = new Text[3];
    [SerializeField]
    private Text[] Oni = new Text[3];
    // Start is called before the first frame update
    void Start()
    {
        if (NoteMaker.EasyList.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                NoteMaker.EasyList.Add(0);
                NoteMaker.NormalList.Add(0);
                NoteMaker.HardList.Add(0);
                NoteMaker.OniList.Add(0);
            }
        }
        for (int i = 0;i < 3; i++)
        {
            Easy[i].text = NoteMaker.EasyList[i].ToString();
            Normal[i].text = NoteMaker.NormalList[i].ToString();
            Hard[i].text = NoteMaker.HardList[i].ToString();
            Oni[i].text = NoteMaker.OniList[i].ToString();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
