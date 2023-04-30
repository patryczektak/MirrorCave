using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    public Transform target;
    public Transform back;

    public int Pjaki;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == Pjaki)
        {
            target.transform.position = new Vector3(back.transform.position.x, target.transform.position.y, back.transform.position.z);
        }
    }
}
