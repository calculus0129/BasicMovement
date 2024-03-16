using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public bool lockx = false;
    public bool locky = true;
    public bool lockz = false;
    Vector3 posDifference;
    // Start is called before the first frame update
    void Start()
    {
        posDifference = this.GetComponent<Transform>().position - player.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rawpos = player.GetComponent<Transform>().position + posDifference;
        if(lockx) rawpos.x = this.GetComponent<Transform>().position.x;
        if(locky) rawpos.y = this.GetComponent<Transform>().position.y;
        if(lockz) rawpos.z = this.GetComponent<Transform>().position.z;
        this.GetComponent<Transform>().position = rawpos;
    }
}
