using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraScreen : MonoBehaviour
{
    public Material mat;
    public RenderTexture rt;
    private GameObject drone;
    private List<Material> mats = new List<Material>();
    private List<GameObject> drones = new List<GameObject>();
    private List<RenderTexture> rts = new List<RenderTexture>();
    public void getDrone(GameObject drone)
    {
        //Debug.Log("drone: " + drone.name);
        this.drone = drone;
        Camera cam = drone.GetComponentInChildren<Camera>();
        RenderTexture newRenderTexture = new RenderTexture(rt);
        Material newMaterial = new Material(mat);
        newMaterial.mainTexture = newRenderTexture;
        newMaterial.name = drone.name + "Material";
        mats.Add(newMaterial);
        cam.targetTexture = newRenderTexture;
        //cam.targetTexture = rt;
        mat.mainTexture = rt;
        drones.Add(drone);
        rts.Add(newRenderTexture);
    }

    public void changeCamera(int i)
    {
        //Debug.Log("New camera is " + i + " and material is " + mats[i].name);
        gameObject.transform.parent.GetComponentInChildren<TextMeshPro>().text = "Drone " + i;
        gameObject.GetComponentInChildren<MeshRenderer>().material = mats[i];
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
