using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class MoveRobot : MonoBehaviour
{
    public GameObject path;
    private Vector3 nextPoint, direction;
    private List<GameObject> points = new List<GameObject>();
    private bool rotation = true, reverse = false;
    private int index = 1;
    private Quaternion nextRotation;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < path.transform.childCount; i++)
        {
            points.Add(path.transform.GetChild(i).gameObject);
        }
        Vector3 nextPosition = new Vector3(points[0].transform.position.x, points[0].transform.position.y - 1.5f, points[0].transform.position.z); 
        this.transform.position = nextPosition;
        nextPoint = points[1].transform.position;
        nextPoint.y = nextPoint.y - 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == nextPoint)
        {
            if (index < points.Count-1 && reverse == false)
            {
                index++;
            }
            else if (reverse == false)
            {
                reverse = true;
            }
            if (index > 0 && reverse == true)
            {
                index--;
            }
            else if (reverse == true)
            {
                reverse = false;
                index = 1;
            }
            nextPoint = points[index].transform.position;
            nextPoint.y = nextPoint.y - 1.5f;
            direction = (nextPoint - transform.position).normalized;
            nextRotation = Quaternion.LookRotation(direction);
            nextRotation = Quaternion.Euler(0f, nextRotation.eulerAngles.y, 0f);
            rotation = false;
        }
        else if (!rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 5.0f);
            float angle = Quaternion.Angle(transform.rotation, nextRotation);
            if (angle < 1f)
            {
                rotation = true;
                transform.rotation = nextRotation;
            }
        }
        else
        {
            float step = (float)2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, step);
        }
    }
}
