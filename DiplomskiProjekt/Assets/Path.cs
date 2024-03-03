using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path
{
    public List<GameObject> points;
    public List<GameObject> lines;
    public bool isSelected = false, isAConnectingPath = false;
    private bool isFinished = false;
    public List<Path> connectingPaths;
    public List<Path> paths;
    //public int timesToRepeat = 1;
    public string name;
    /*public void increaseTimesToRepeat()
    {
        timesToRepeat++;
    }

    public void decreaseTimesToRepeat()
    {
        if (timesToRepeat > 0) {
            timesToRepeat--;
        }
    }*/
    public void addConnectingPath(Path connectingPath, Path otherPath)
    {
        connectingPaths.Add(connectingPath);
        paths.Add(otherPath);
    }

    public Path()
    {
        points = new List<GameObject>();
        lines = new List<GameObject>();
        connectingPaths = new List<Path>();
        paths = new List<Path>();
    }

    public void addPoint(GameObject point, GameObject line3D)
    {
        if (point.GetComponentInChildren<PointScript>().path == null)
        {
            point.GetComponentInChildren<PointScript>().setPath(this);
        }
        points.Add(point);
        if (points.Count > 1)
        {
            GameObject newLine = Object.Instantiate(line3D, points[points.Count - 2].transform.position, new Quaternion(0, 0, 0, 0));
            newLine.GetComponent<LineRenderer>().positionCount = 2;
            newLine.GetComponent<LineRenderer>().SetPosition(0, points[points.Count - 2].transform.position);
            newLine.GetComponent<LineRenderer>().SetPosition(1, points[points.Count - 1].transform.position);
            Debug.Log("Prvi: " + points[points.Count - 2].transform.position);
            Debug.Log("Drugi: " + points[points.Count - 1].transform.position);
            Debug.Log("Linija 1: " + newLine.GetComponent<LineRenderer>().GetPosition(0));
            Debug.Log("Linija 2: " + newLine.GetComponent<LineRenderer>().GetPosition(1));
            lines.Add(newLine);
        }
    }
    public void removeLastPoint()
    {
        GameObject point = points[points.Count - 1];
        points.Remove(point);
        if (lines.Count != 0)
        {
            GameObject line = lines[lines.Count - 1];
            lines.Remove(line);
            Object.Destroy(line);
        }
        Object.Destroy(point);
    }

    public void selectPath()
    {
        if (isFinished)
        {
            isSelected = true;
            for (int i = 0; i < points.Count; i++)
            {
                points[i].GetComponentInChildren<PointScript>().setSelectedMaterial();
            }
        }
    }

    public void ToggleSelectPath()
    {
        if (isFinished)
        {
            if (isSelected != true)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i].GetComponentInChildren<PointScript>().setSelectedMaterial();
                }
                isSelected = true;
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i].GetComponentInChildren<PointScript>().setDefaultMaterial();
                    points[i].GetComponentInChildren<PointScript>().isSelected = false;
                }
                isSelected = false;
            }
        }
    }

    public void deselectAllPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i].GetComponentInChildren<PointScript>().isSelected = false;
        }
    }

    public void deSelectPath()
    {
        isSelected = false;
        for (int i = 0; i < points.Count; i++)
        {
            points[i].GetComponentInChildren<PointScript>().setDefaultMaterial();
            points[i].GetComponentInChildren<PointScript>().isSelected = false;
        }
    }


    public void destroyPath()
    {
        foreach(GameObject obj in points)
        {
            Object.Destroy(obj.gameObject);
        }
        foreach (GameObject obj in lines)
        {
            Object.Destroy(obj.gameObject);
        }
    }

    public void finishPath(GameObject line3D)
    {
        isFinished = true;
        GameObject newLine = Object.Instantiate(line3D, points[points.Count - 1].transform.position, new Quaternion(0, 0, 0, 0));
        newLine.GetComponent<LineRenderer>().positionCount = 2;
        newLine.GetComponent<LineRenderer>().SetPosition(0, points[points.Count - 1].transform.position);
        newLine.GetComponent<LineRenderer>().SetPosition(1, points[0].transform.position);
        /*Debug.Log("Prvi: " + points[points.Count - 2].transform.position);
        Debug.Log("Drugi: " + points[points.Count - 1].transform.position);
        Debug.Log("Linija 1: " + newLine.GetComponent<LineRenderer>().GetPosition(0));
        Debug.Log("Linija 2: " + newLine.GetComponent<LineRenderer>().GetPosition(1));*/
        lines.Add(newLine);
        for (int i = 0; i < points.Count; i++)
        {
            points[i].GetComponentInChildren<PointScript>().setDefaultMaterial();
        }
    }
    public void finishConnectingPath(GameObject line3D, GameObject lastPoint)
    {
        isFinished = true;
        GameObject newLine = Object.Instantiate(line3D, points[points.Count - 1].transform.position, new Quaternion(0, 0, 0, 0));
        newLine.GetComponent<LineRenderer>().positionCount = 2;
        newLine.GetComponent<LineRenderer>().SetPosition(0, points[points.Count - 1].transform.position);
        newLine.GetComponent<LineRenderer>().SetPosition(1, lastPoint.transform.position);
        points.Add(lastPoint);
        /*Debug.Log("Prvi: " + points[points.Count - 2].transform.position);
        Debug.Log("Drugi: " + points[points.Count - 1].transform.position);
        Debug.Log("Linija 1: " + newLine.GetComponent<LineRenderer>().GetPosition(0));
        Debug.Log("Linija 2: " + newLine.GetComponent<LineRenderer>().GetPosition(1));*/
        lines.Add(newLine);
        for (int i = 1; i < points.Count-1; i++)
        {
            points[i].GetComponentInChildren<PointScript>().setConnectingMaterial();
        }

    }
}
