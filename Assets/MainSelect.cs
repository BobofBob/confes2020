using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainSelect : MonoBehaviour
{
    [SerializeField]
    private Text[] ScoreTex = new Text[12];
    private int[,] ScoreNum = new int[4,3];
    private int[] Score = new int[3];
    [SerializeField]
    private AudioClip[] audio;
    private AudioSource DemoBGM;
    public static string[] Name = new string[]
    {
        "夏祭り",
        "季曲 -Season of Asia-",
        "激運！七福ハッピークルー",
        "Clotho_クロートー"
    };
    public static int Number = 1;

    [SerializeField]
    private Text NumTex;
    [SerializeField]
    private Text MusicTex;
    // Start is called before the first frame update
    void Start()
    {
        MusicTex.text = Name[Number - 1];
        NumTex.text = Number.ToString() + ".";
        DemoBGM = this.gameObject.GetComponent<AudioSource>();
        DemoBGM.clip = audio[Number - 1];
        //for(int i = 0;i < Name.Length * 12; i++)
        //    PlayerPrefs.SetInt(i.ToString(), 0);
        SetNum();
        StartCoroutine("PlayBGM");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))      
            NumPlusOrMinus(true);        
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NumPlusOrMinus(false);       
    }
    public void NumPlusOrMinus(bool Trigger)
    {
        if (Trigger)
        {
            if (Number == 1)
                Number = Name.Length;
            else
                Number--;
        }
        else
        {
            if (Number == Name.Length)
                Number = 1;
            else
                Number++;
        }
        SetUp();
    }
    private void SetUp()
    {
        DemoBGM.Pause();
        NumTex.text = Number.ToString() + ".";
        MusicTex.text = Name[Number - 1];
        DemoBGM.clip = audio[Number - 1];
        SetNum();
    }
    private void SetNum()
    {
        var num = 0;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 3; j++)
                ScoreNum[i, j] = PlayerPrefs.GetInt(((Number - 1) * 12 + num++).ToString());
        num = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
                Score[j] = ScoreNum[i, j];
            Array.Sort(Score);
            Array.Reverse(Score);
            for (int j = 0; j < 3; j++)
            {
                PlayerPrefs.SetInt(((Number - 1) * 12 + num).ToString(), Score[j]);
                ScoreTex[num++].text = Score[j].ToString();
            }
        }
    }
    private IEnumerator PlayBGM()
    {
        while (true)
        {
            if(DemoBGM.isPlaying == false)
            {
                yield return new WaitForSeconds(1f);
                DemoBGM.Play();
            }
            yield return null;     
        }
    }
}
