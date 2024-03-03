using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedQuad : MonoBehaviour
{
    public Material selectedMaterial, defaultMaterial;
    public Path path;

    public void setPath(Path path)
    {
        this.path = path;
    }
    public void SelectQuad()
    {
        GetComponentInChildren<Renderer>().enabled = true;
        GetComponentInChildren<Renderer>().material = selectedMaterial;
    }

    public void DeselectQuad()
    {
        GetComponentInChildren<Renderer>().enabled = true;
        GetComponentInChildren<Renderer>().material = defaultMaterial;
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
