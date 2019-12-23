using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoteTimming : MonoBehaviour
{
    public float NoteSpeed;
    public float DestTime;
    private AudioSource SEaudio;
    private float auto = 0;
    public static float Off = 0;
    private NoteMaker note;
    public bool Trigger = true;
    public bool KusudamaTri = true;
    public static bool AutoTri;
    private Text Kusu;
    public int KusuNum;
    // Start is called before the first frame update
    void Start()
    {
        SEaudio = GameObject.Find("Camera").transform.GetComponent<AudioSource>();
        note = GameObject.Find("Camera").transform.GetComponent<NoteMaker>();
        NoteSpeed = note.NoteSpeed;
        if (AutoTri)
            Trigger = false;
        if (this.CompareTag("Kusudama"))
            Kusu = this.gameObject.GetComponentInChildren<Text>();
        DestTime = note.Timming[int.Parse(this.name) - 1] - Off + 1.703f * (-8 / NoteSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        auto += Time.deltaTime;
        this.transform.position += new Vector3(NoteSpeed * Time.deltaTime, 0, 0);
        if(AutoTri)
            if (auto > DestTime)                                                    
                Destroy(this.gameObject);
        if (auto > DestTime + 0.2 * (-8 / NoteSpeed) && KusudamaTri)
        {
            Debug.Log("noteDestroy");
            Trigger = false;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Trigger)
        {
            if (this.CompareTag("Don_L"))
                SEaudio.PlayOneShot(note.ClipAudio[0]);
            else if (this.CompareTag("Ka_L"))
                SEaudio.PlayOneShot(note.ClipAudio[1]);
        }
        else if (AutoTri)
        {
            if (this.CompareTag("Don_L"))
                SEaudio.PlayOneShot(note.ClipAudio[3]);
            else if (this.CompareTag("Ka_L"))
                SEaudio.PlayOneShot(note.ClipAudio[4]);
            else if (this.CompareTag("Don"))
                SEaudio.PlayOneShot(note.ClipAudio[0]);
            else if (this.CompareTag("Ka"))
                SEaudio.PlayOneShot(note.ClipAudio[1]);
            else if (this.CompareTag("Kusudama"))
                SEaudio.PlayOneShot(note.ClipAudio[2]);
        }
    }

    public void SetKusudama(int num)
    {
        KusudamaTri = false;
        NoteSpeed = 0;
        var obj = this.gameObject;
        var Vec = obj.transform.localScale;
        obj.transform.localScale = new Vector3(Vec.x * 2, Vec.y * 2, Vec.z * 2);
        SetText(num);
    }

    public void SetText(int num)
    {
        Kusu.text = num.ToString();
    }
}
