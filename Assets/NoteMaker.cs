using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NoteMaker : MonoBehaviour
{
    public static float BPM = 180f;
    private float LineLength;                   //一小節に何拍あるか
    public float NoteSpeed;                   //ノーツの速度
    private int NoteCount = 0;                //0を含めたノーツ数
    private int One_NoteCount = 0;        //0を含めないノーツ数         
    private int Combo = 0;
    private int Score = 0;
    private int ComboAnimTri = 100;
    private int[] Result = new int[3];
    private int[] Balloon = new int[20];
    private int BalloonNum = 0;
    private string BalloonTex;
    public static int MusicNumber;
    public static float Offset = 3;                       //曲のスタート待ち時間 1.15      //季曲は5.9
    private float OneBeat;                    //一拍
    private float OneMeasure;              //一小節
    private float WaitTime;                  //待ち時間
    private float SetNum;                    //一拍でいくらずれるか
    private float N_Time = 0;
    private float DestroyTime;
    private float VecX_PlusOne = 9.35307f;       //オブジェクト生成時の値のずれ
    private float FalseVecX;                  //スクリプト上の配置場所
    public float[] Timming = new float[1400];  //ノーツの判定
    public int[] NoteTurn = new int[1400]; //1400ノーツまで格納可能
    private string[] humen;                 //譜面データ
    private string OneLine;                  //これはcharとして扱う  
    private bool BreakTri = false;
    public static string Difficult;
    public static string Diff;
    private Vector3 JugdeText;
    [SerializeField]
    private GameObject[] Note = new GameObject[8];
    [SerializeField]
    private GameObject[] HanteiTex;
    [SerializeField]
    private GameObject[] Particle; 
    [SerializeField]
    private Text ComboTex;          //こういうテキスト関係もすべてクラスにまとめた方がいいね
    [SerializeField]                       //既に手遅れ感あるけど
    private Text ScoreTex;
    [SerializeField]
    private GameObject Full;
    [SerializeField]
    private Text Hantei;
    [SerializeField]
    private Text[] ResultNum = new Text[3];
    private GameObject Canvas;
    private NoteTimming N_timming;
    public AudioClip[] ClipAudio;
    private AudioSource audio;
    public static List<int> EasyList = new List<int>();
    public static List<int> NormalList = new List<int>();
    public static List<int> HardList = new List<int>();
    public static List<int>  OniList = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load(Difficult, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる   
        humen = TextLines.Split('\n');
        NoteSpeed = DummySpeed.speed;
        BalloonTex = humen[0];
        audio = GetComponent<AudioSource>();
        var j = 0;
        if(BalloonTex[0] != '0')                     //バルーンなしの場合、0を置いておくこと
            for (int i = 0; i < BalloonTex.Length -1; i++,j++) {
                var num = 0;
                var m = 0;
                int[] array = new int[3];
                while(BalloonTex[i] != ',')
                {
                    array[num++] = BalloonTex[i] - '0';
                    i++;
                }
                for (; num != 0; num--)
                    Balloon[j] += array[m++] * (int)Math.Pow(10, num - 1);
                Debug.Log(Balloon[j]);
            }
        Timming[0] = Offset;
        Canvas = GameObject.Find("Canvas");
        GameObject.Find("Canvas").transform.Find("Difficult").transform.GetComponent<Text>().text = Diff;
        JugdeText = Hantei.transform.position;
        OneBeat = (float)60 / BPM;
        OneMeasure = OneBeat * 4;
        StartCoroutine("PlayGame");
    }

    // Update is called once per frame
    void Update()
    {
        N_Time += Time.deltaTime;
        if (NoteTimming.AutoTri == false)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
                audio.PlayOneShot(ClipAudio[0]);
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
                audio.PlayOneShot(ClipAudio[1]);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NoteTimming.AutoTri = false;
            switch (Diff)
            {
                case "かんたん":
                    ScoreList(1);
                    break;
                case "まあまあ":
                    ScoreList(2);
                    break;
                case "むずかしい":
                    ScoreList(3);
                    break;
                case "やばい":
                    ScoreList(4);
                    break;
            }
            MainSelect.Number = MusicNumber;
            SceneManager.LoadScene("MusicSelect");
        }
    }
    //private void FixedUpdate()
    //{
    //    Debug.Log(N_Time);
    //}                                                                   

    private IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(Offset);
        //↓ノーツ配置
        for (int i = 1; humen[i] != ""; i++)            //stringはnull？
        {
            OneLine = humen[i];
            LineLength = humen[i].Length - 1;
            WaitTime = OneMeasure / LineLength;
            SetNum = -1 * NoteSpeed * WaitTime;
            for (int j = 0; j < LineLength; j++)
            {
                Timming[One_NoteCount + 1] = Timming[One_NoteCount];
                switch (OneLine[j])
                {
                    case '1':
                        Preparation(0);
                        break;
                    case '2':
                        Preparation(1);
                        break;
                    case '3':
                        Preparation(2);
                        break;
                    case '4':
                        Preparation(3);
                        break;
                    case '5':
                        Preparation(4);
                        int k = j;
                        do
                        {
                            VecXCount();
                            k++;
                            if (k == LineLength)
                            {
                                k = 0;
                                OneLine = humen[++i];
                            }
                            Timming[One_NoteCount] += WaitTime;
                        } while (OneLine[k] != '6');
                        j = k;
                        break;
                }
                VecXCount();                                                                                                  
                Timming[One_NoteCount] += WaitTime;
            }
        }
        //↑配置終わり
        //↓判定
        int num = 0;
        while(num < One_NoteCount)
        {
            DestroyTime = Timming[num] + 1.76f
             * (float)(-8 / NoteSpeed);
            var obj = GameObject.Find("Canvas").transform.Find((num + 1).ToString()).gameObject;
            N_timming = obj.GetComponent<NoteTimming>();
            //くすだまの判定
            while (obj.CompareTag("Kusudama"))
            {
                yield return null;
                if(N_Time > DestroyTime)
                {
                    int RepeatHit = 0;
                    N_timming.SetKusudama(Balloon[BalloonNum]);
                    while (RepeatHit < Balloon[BalloonNum])
                    {
                        yield return null;
                        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
                        {
                            RepeatHit++;
                            N_timming.SetText(Balloon[BalloonNum] - RepeatHit);
                            Score += 300;
                            if (RepeatHit == Balloon[BalloonNum])
                            {
                                Score += 5000;
                                audio.PlayOneShot(ClipAudio[2]);
                                var ParticleEffect = Instantiate(Particle[1]);
                                ParticleEffect.transform.localPosition = new Vector3(-434.8f, 142.77f, -153.8f);
                                ParticleEffect.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);      //ここのfalseがないとあらぬ方向に配置される
                            }
                            ScoreTex.text = Score.ToString();
                        }
                        if (N_Time > Timming[num + 1] + 1.76f * (float)(-8 / NoteSpeed) - (Timming[num + 1] + 1.76f * (float)(-8 / NoteSpeed) - DestroyTime) / 4)
                            break;
                    }
                    BalloonNum++;
                    NoteTurn[num++] = 0;
                    Destroy(obj.gameObject);
                    goto KUSUDAMA;     //ミス判定はないためここに飛ぶ
                }
            }
            //良・可の圏内の時の処理
            while (N_Time > DestroyTime - 0.09f && N_Time < DestroyTime + 0.09f)
            {
                while (N_Time > DestroyTime - 0.035f && N_Time < DestroyTime + 0.035f)
                {
                    if (obj.CompareTag("Don"))
                    { 
                        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
                        {
                            num = InputKey(num, obj, 1, 1);
                            goto LABEL;
                        }
                    }
                    else if (obj.CompareTag("Ka"))
                    {
                        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
                        {
                            num = InputKey(num, obj, 1, 1);
                            goto LABEL;
                        }
                    }
                    else if (obj.CompareTag("Don_L"))
                    {
                        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
                        {
                            num = InputKey(num, obj, 2, 1);
                            audio.PlayOneShot(ClipAudio[0]);
                            goto LABEL;
                        }
                    }
                    else if (obj.CompareTag("Ka_L"))
                    {
                        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
                        {
                            audio.PlayOneShot(ClipAudio[1]);
                            num = InputKey(num, obj, 2, 1);
                            goto LABEL;
                        }
                    }
                    yield return null;
                }
                if (obj.CompareTag("Don"))
                {
                    if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
                    {
                        num = InputKey(num, obj, 1, 2);
                        break;
                    }
                }
                else if (obj.CompareTag("Ka"))
                {
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
                    {
                        num = InputKey(num, obj, 1, 2);
                        break;
                    }
                }
                else if (obj.CompareTag("Don_L"))
                {
                    if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
                    {
                        audio.PlayOneShot(ClipAudio[0]);
                        num = InputKey(num, obj, 2, 2);
                        break;
                    }
                }
                else if (obj.CompareTag("Ka_L"))
                {
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
                    {
                        audio.PlayOneShot(ClipAudio[1]);
                        num = InputKey(num, obj, 2, 2);
                        break;
                    }
                }
                yield return null;
            }
            LABEL:      //通常のノーツはここに飛ぶ
            //ミス判定
            if (N_Time > DestroyTime + 0.09f)
            {
                N_timming.Trigger = false;
                Combo = 0;
                ComboAnimTri = 100;
                ComboTex.text = "";
                var HanteiObj = Instantiate(Hantei);
                HanteiObj.rectTransform.localPosition = new Vector3(0, 0, 0);
                HanteiObj.transform.SetParent(GameObject.Find("Canvas").transform.Find("HanteiVec").gameObject.transform,false);      //ここのfalseがないとあらぬ方向に配置される
                HanteiObj.text = "Miss";
                HanteiObj.fontSize = 20;
                HanteiObj.color = new Color(255, 0, 255, 1);
                Destroy(obj.gameObject);
                NoteTurn[num++] = 0;
                Result[2]++;
                //Debug.Log(num);
            }
            KUSUDAMA:        //くすだまにミス判定はないためここに飛ぶ
            DestroyTime = Timming[num] + 1.76f
                * (float)(-8 / NoteSpeed);              //274:275
            for (int i = 0; i < 3; i++)
                ResultNum[i].text = Result[i].ToString();
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        if (Result[2] == 0)
        {
            Full.SetActive(true);
        }
    }
    private void VecXCount()
    {
        NoteCount++;
        FalseVecX += SetNum;
    }
    private void Preparation(int num)
    {
        var obj = Instantiate(Note[num], new Vector3(10 + FalseVecX, 2.85f, 0), Quaternion.identity);
        obj.transform.SetParent(Canvas.transform);  //こうしないと黄色エラー吐く
        obj.transform.name = (One_NoteCount + 1).ToString();
        NoteTurn[One_NoteCount++] = 1;
    }
    private int InputKey(int num,GameObject obj,int Lsize,int GoodBad)
    {
        NoteTurn[num++] = 0;
        Destroy(obj.gameObject);
        Combo++;
        ComboTex.text = Combo.ToString();
        if (Combo >= ComboAnimTri)
        {
            ComboAnimTri += 100;
            StartCoroutine("ComboAnim");
        }
        else
            StartCoroutine("ComboAnimNormal");    
        if (GoodBad == 1)
        {
            Score += Combo * Lsize * 2;
            var ParticleEffect = Instantiate(Particle[0]);
            ParticleEffect.transform.localPosition = new Vector3(-434.8f, 142.77f, -153.8f);
            ParticleEffect.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);      //ここのfalseがないとあらぬ方向に配置される
            var HanteiObj = Instantiate(Hantei);
            HanteiObj.rectTransform.localPosition = new Vector3(0, 0, 0);
            HanteiObj.transform.SetParent(GameObject.Find("Canvas").transform.Find("HanteiVec").gameObject.transform, false);
            HanteiObj.text = "Good";
            HanteiObj.fontSize = 20;
            HanteiObj.color = new Color(255, 255, 255, 1);
            Result[0]++;
        }
        else if(GoodBad == 2)
        {
            Score += Combo * Lsize;
            var ParticleEffect = Instantiate(Particle[0]);
            ParticleEffect.transform.localPosition = new Vector3(-434.8f, 142.77f, -153.8f);
            ParticleEffect.transform.SetParent(GameObject.Find("Canvas").gameObject.transform, false);      //ここのfalseがないとあらぬ方向に配置される
            var HanteiObj = Instantiate(Hantei);
            HanteiObj.rectTransform.localPosition = new Vector3(0, 0, 0);
            HanteiObj.transform.SetParent(GameObject.Find("Canvas").transform.Find("HanteiVec").gameObject.transform,false);
            HanteiObj.text = "OK";
            HanteiObj.fontSize = 20;
            HanteiObj.color = new Color(0, 255, 0, 1);
            Result[1]++;
        }
        ScoreTex.text = Score.ToString();
        return num;
    }
    private void ScoreList(int DiffcultNum)
    {         
        if(Score > PlayerPrefs.GetInt((12 * (MainSelect.Number - 1) + DiffcultNum * 3 - 1).ToString()))
            PlayerPrefs.SetInt((12 * (MainSelect.Number - 1) + DiffcultNum * 3 - 1).ToString(), Score);
    }
    private IEnumerator ComboAnim()
    {
        for (int i = 0; i < 7; i++) {
            ComboTex.fontSize += 1;
            ComboTex.color -= new Color(0,0,0,0.04f);
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < 7; i++)
        {
            ComboTex.fontSize -= 1;
            ComboTex.color += new Color(0, 0, 0, 0.04f);
            yield return new WaitForSeconds(0.02f);
        }
    }
    private  IEnumerator ComboAnimNormal()
    {
        for (int i = 0; i < 10; i++)
        {
            ComboTex.transform.position += new Vector3(0,0.2f,0);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++)
        {
            ComboTex.transform.position -= new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}