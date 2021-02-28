using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInit : MonoBehaviour
{
    bool setPos = false;
    void Start()
    {
        this.transform.position = new Vector3(6.6f, 1.5f, -2.7f);       
    }

    private void Update() {
        if(this.transform.position != new Vector3(6.6f, 1.5f, -2.7f) && !setPos)
        {
            this.transform.position = new Vector3(6.6f, 1.5f, -2.7f);
            setPos = true;
        }
    }
}
