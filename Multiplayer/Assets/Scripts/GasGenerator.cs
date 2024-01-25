using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasGenerator : MonoBehaviour
{

    public GameObject gasCan;
    public float generationRate = 10;
    private int frameCount;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        transform.position = new Vector2(Random.Range (cam.transform.position.x-55, cam.transform.position.x+55), transform.position.y);
        if (frameCount % generationRate==0)
        {
            Instantiate(gasCan, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }
}
