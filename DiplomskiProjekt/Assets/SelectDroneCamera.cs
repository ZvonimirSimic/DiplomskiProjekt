using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDroneCamera : MonoBehaviour
{
    public GameObject player;
    public string arrow;

    public void arrowHit()
    {
        player.GetComponentInChildren<PlayerOptions>().nextCamera(arrow);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
