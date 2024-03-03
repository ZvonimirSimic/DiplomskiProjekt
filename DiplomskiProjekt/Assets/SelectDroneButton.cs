using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDroneButton : MonoBehaviour
{
    public Material selectedMaterial, defaultMaterial;
    public bool selected = false;
    public void ToggleSelect()
    {
        transform.GetChild(2).GetComponent<Renderer>().enabled = true;
        if (selected)
        {
            transform.GetChild(2).GetComponent<Renderer>().material = defaultMaterial;
            selected = false;
        }
        else
        {
            selected = true;
            transform.GetChild(2).GetComponent<Renderer>().material = selectedMaterial;
        }
    }

    public void SelectButton()
    {
        transform.GetChild(2).GetComponent<Renderer>().enabled = true;
        transform.GetChild(2).GetComponent<Renderer>().material = selectedMaterial;
        selected = true;
    }

    public void DeselectButton()
    {
        transform.GetChild(2).GetComponent<Renderer>().enabled = true;
        transform.GetChild(2).GetComponent<Renderer>().material = defaultMaterial;
        selected = false;
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
