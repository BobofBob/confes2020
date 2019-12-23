using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JudgeAnim : MonoBehaviour
{

    private Text tex;

    // Start is called before the first frame update
    void Start()
    {
        tex = this.gameObject.GetComponent<Text>();
        StartCoroutine("Anim");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Anim()
    {
        for(int  i = 0;i < 10; i++)
        {
            transform.position += new Vector3(0, 0.2f, 0);
            tex.color -= new Color(0, 0, 0, 0.05f);
            yield return new WaitForSeconds(0.08f);
        }
        Destroy(this.gameObject);
    }
}
