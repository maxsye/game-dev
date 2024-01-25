using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerastuff : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject drill; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(drill.transform.position.x, drill.transform.position.y, transform.position.z);
           
    }
}
