using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public Material selectedMaterial, defaultMaterial, creatingMaterial, transitionMaterial, selectedPointMaterial;
    public Path path;
    public Path connectingPath = null;
    public Vector3 coordinates;
    public bool isSelected, selectable = true;


    public Path setSelected()
    {
        return path;
    }
    public void setPath(Path path)
    {
        this.path = path;
    }
    public void setDefaultMaterial()
    {
        GetComponentInChildren<Renderer>().enabled = true;
        GetComponentInChildren<Renderer>().material = defaultMaterial;
    }

    public void setSelectedMaterial()
    {
        if (selectable)
        {
            GetComponentInChildren<Renderer>().enabled = true;
            GetComponentInChildren<Renderer>().material = selectedMaterial;
        }
    }

    public void setSelectedPointMaterial()
    {
        if (selectable)
        {
            GetComponentInChildren<Renderer>().enabled = true;
            GetComponentInChildren<Renderer>().material = selectedPointMaterial;
            isSelected = true;
        }
    }

    public void setConnectingMaterial()
    {
        GetComponentInChildren<Renderer>().enabled = true;
        GetComponentInChildren<Renderer>().material = transitionMaterial;
        isSelected = false;
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
