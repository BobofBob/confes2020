using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectDiff : MonoBehaviour
{
    private string[] NameAlfabet = new string[]
    {
        "Natsu_",
        "Kikyoku_",
        "Gekiun_",
        "Clotho_"
    };
    private float[] OffsetNum = new float[]
    {
        1.25f,
        5.9f,
        1.15f,
        1.7f
    };
    private float[] BPMNum = new float[]
    {
        141,
        150,
        180,
        185
    };

    private float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = DummySpeed.speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        switch (this.gameObject.name)
        {
            case "EasyButton":
                NoteMaker.Difficult = NameAlfabet[MainSelect.Number - 1] + "Easy";
                NoteMaker.Diff = "かんたん";
                break;
            case "NormalButton":
                NoteMaker.Difficult = NameAlfabet[MainSelect.Number - 1] + "Normal";
                NoteMaker.Diff = "まあまあ";
                break;
            case "HardButton":
                NoteMaker.Difficult = NameAlfabet[MainSelect.Number - 1] + "Hard";
                NoteMaker.Diff = "むずかしい";
                break;
            case "OniButton":
                NoteMaker.Difficult = NameAlfabet[MainSelect.Number - 1] + "Oni";
                NoteMaker.Diff = "やばい";
                break;
        }
        WaitAudio.MusicNumber = MainSelect.Number - 1;
        NoteTimming.Off = OffsetNum[MainSelect.Number - 1] * (Speed / (float)-8);
        NoteMaker.Offset = OffsetNum[MainSelect.Number - 1] * (Speed / (float)-8);
        NoteMaker.BPM = BPMNum[MainSelect.Number - 1];
        NoteMaker.MusicNumber = MainSelect.Number;
        //NoteMaker.BalloonTex = NameAlfabet[MainSelect.Number - 1] + "Balloon";
        SceneManager.LoadScene("Game");
    }
}
