using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    private float N_Time;
    private bool ParticleTri = false;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.name == "GoodParticle(Clone)")
            ParticleTri = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ParticleTri)
        {
            N_Time += Time.deltaTime;
            if (N_Time > 0.2f)
                Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
    }
}
