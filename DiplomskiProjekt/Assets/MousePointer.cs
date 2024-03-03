using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePointer : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out RaycastHit hitData))
            {
                worldPosition = hitData.point;
                Debug.Log("Point: " + hitData.collider.name);
                if (hitData.collider != null)
                {
                    if (hitData.collider.transform.parent != null)
                    {
                        if (hitData.collider.transform.parent.GetComponentInChildren<PointScript>() != null)
                        {
                            if (hitData.collider.transform.parent.GetComponentInChildren<PointScript>().selectable)
                            {
                                Path path = hitData.collider.transform.parent.GetComponentInChildren<PointScript>().setSelected();
                                path.selectPath();
                                path.deselectAllPoints();
                                hitData.collider.transform.parent.GetComponentInChildren<PointScript>().setSelectedPointMaterial();
                                GetComponentInChildren<PlayerOptions>().selectPressedPath(path);
                            }
                        }
                        if (hitData.collider.transform.parent.GetComponentInChildren<UnityEngine.UI.Button>() != null)
                        {
                            hitData.collider.transform.parent.GetComponentInChildren<UnityEngine.UI.Button>().onClick.Invoke();
                        }
                    }
                    else
                    {
                        if (hitData.collider.gameObject.GetComponentInChildren<MoveDrone>() != null)
                        {
                            gameObject.GetComponentInChildren<PlayerOptions>().selectDrone(hitData.collider.gameObject);
                        }
                        if (hitData.collider.gameObject.GetComponentInChildren<SelectDroneCamera>() != null)
                        {
                            hitData.collider.gameObject.GetComponentInChildren<SelectDroneCamera>().arrowHit();
                        }
                        
                    }
                }
            }
        }

        //Cursor.visible = true;

    }
}
