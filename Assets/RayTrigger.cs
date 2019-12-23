using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTrigger : MonoBehaviour
{ 
    private MainSelect NumChange;
    // Start is called before the first frame update
    void Start()
    {
        NumChange = GameObject.Find("Camera").GetComponent<MainSelect>();
    }

    public void RayClick()
    {
        if (this.gameObject.name == "RayLeft")
            NumChange.NumPlusOrMinus(true);
        else if (this.gameObject.name == "RayRight")
            NumChange.NumPlusOrMinus(false);
    }
}
