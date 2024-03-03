using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PointMaker : MonoBehaviour
{
    public GameObject camera, point3D, line3D, firstPoint, pointLast;
    public Material correctMaterial;
    private List<Path> allPaths, connectingPaths;
    private Path first = null, second = null;
    private Path singlePath;
    private Path connectingPath; 
    private bool currentlyCreatingPath = false, currentlyCreatingConnectingPath = false;
    private float time = 0.5f, timer;
    // Start is called before the first frame update
    void Start()
    {
        allPaths = new List<Path>();
        connectingPaths = new List<Path>();
        timer = Time.time;
    }

    public List<Path> getPaths()
    {
        return allPaths;
    }

    public Path selectedPath()
    {
        for(int i = 0; i < allPaths.Count; i++)
        {
            if (allPaths[i].isSelected)
            {
                return allPaths[i];
            }
        }
        return null;
    }
    // Update is called once per frame

    private void createConnectingPath()
    {
        connectingPath = new Path();
        for (int i = 0; i < first.points.Count; i++)
        {
            if (first.points[i].GetComponentInChildren<PointScript>().isSelected)
            {
                connectingPath.addPoint(first.points[i], new GameObject());
                firstPoint = first.points[i];
                break;
            }
        }
        for (int i = 0; i < second.points.Count; i++)
        {
            if (second.points[i].GetComponentInChildren<PointScript>().isSelected)
            {
                pointLast = second.points[i];
                break;
            }
        }
        if (firstPoint != null && pointLast != null)
        {
            currentlyCreatingConnectingPath = true;
            camera.transform.GetChild(1).transform.gameObject.SetActive(true);
            firstPoint.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void create3DPointConnecting(Vector3 position)
    {
        GameObject newObject = Instantiate(point3D, position, new Quaternion(0, 0, 0, 0));
        newObject.GetComponentInChildren<PointScript>().coordinates = position;
        newObject.GetComponentInChildren<PointScript>().selectable = false;
        connectingPath.addPoint(newObject.gameObject, line3D);
    }
    private void finishNewConnectingPath()
    {
        connectingPath.finishConnectingPath(line3D, pointLast);
        connectingPath.paths.Add(first);
        connectingPath.paths.Add(second);
        connectingPath.isAConnectingPath = true;
        connectingPath.name = "Connecting path " + connectingPaths.Count.ToString();
        connectingPaths.Add(connectingPath);
        camera.transform.GetChild(1).transform.gameObject.SetActive(false);
        currentlyCreatingConnectingPath = false;
        first.addConnectingPath(connectingPath, second);
        second.addConnectingPath(connectingPath, first);
        pointLast.GetComponentInChildren<PointScript>().connectingPath = connectingPath;
        firstPoint.GetComponentInChildren<PointScript>().connectingPath = connectingPath;
        firstPoint.transform.GetChild(1).gameObject.SetActive(false);
        firstPoint = null;
        pointLast = null;
    }
    private void createNewPath()
    {
        singlePath = new Path();
        camera.transform.GetChild(1).transform.gameObject.SetActive(true);
        currentlyCreatingPath = true;
    }


    private void deletePathInCreation()
    {
        singlePath.destroyPath();
        camera.transform.GetChild(1).transform.gameObject.SetActive(false);
        currentlyCreatingPath = false;
    }

    private void finishNewPath()
    {
        singlePath.finishPath(line3D);
        singlePath.name = "Path " + allPaths.Count.ToString();
        allPaths.Add(singlePath);
        camera.transform.GetChild(1).transform.gameObject.SetActive(false);
        currentlyCreatingPath = false;
    }

    private void create3DPoint(Vector3 position)
    {
        GameObject newObject = Instantiate(point3D, position, new Quaternion(0,0,0,0));
        newObject.GetComponentInChildren<PointScript>().coordinates = position;
        singlePath.addPoint(newObject.gameObject, line3D);
    }

    public Path onePathSelected()
    {
        Path selPath = null;
        int j = 0;
        for (int i = 0; i < allPaths.Count; i++)
        {
            if (allPaths[i].isSelected && j == 0)
            {
                selPath = allPaths[i];
                j++;
            }
            else if (allPaths[i].isSelected && j == 1)
            {
                return null;
            }
        }
        return selPath;
    }

        private bool twoPathsSelected()
    {
        int j = 0;
        for(int i = 0; i < allPaths.Count; i++)
        {
            if (allPaths[i].isSelected && j == 0)
            {
                first = allPaths[i];
                j++;
            }
            else if (allPaths[i].isSelected && j == 1)
            {
                second = allPaths[i];
                j++;
            }
        }
        if(j == 2)
        {
            return true;
        }
        first = null; second = null;
        return false;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.L) && twoPathsSelected() && !currentlyCreatingPath && !currentlyCreatingConnectingPath)
        {
            
            createConnectingPath();
        }
        if (Input.GetMouseButton(1) && !currentlyCreatingPath && currentlyCreatingConnectingPath)
        {
            if (Time.time - timer >= time)
            {
                timer = Time.time;
                Vector3 newPointPosition = camera.transform.GetChild(1).position;
                create3DPointConnecting(newPointPosition);
            }
        }
        if (Input.GetKey(KeyCode.Return) && !currentlyCreatingPath && currentlyCreatingConnectingPath)
        {
            finishNewConnectingPath();
        }
        if (Input.GetKey(KeyCode.U) && !currentlyCreatingPath && !currentlyCreatingConnectingPath)
        {
            for(int i = 0; i < allPaths.Count; i++)
            {
                allPaths[i].deSelectPath();
                GetComponent<PlayerOptions>().deselectAllPaths();
            }
        }
        if (Input.GetKey(KeyCode.P) && !currentlyCreatingPath && !currentlyCreatingConnectingPath)
        {
            if (Time.time - timer >= time)
            {
                timer = Time.time;
                createNewPath();
            }
        }
        if (Input.GetKey(KeyCode.P) && currentlyCreatingPath && !currentlyCreatingConnectingPath)
        {
            if (Time.time - timer >= time)
            {
                timer = Time.time;
                deletePathInCreation();
            }
        }
        if (Input.GetMouseButton(1) && currentlyCreatingPath && !currentlyCreatingConnectingPath)
        {
            if (Time.time - timer >= time)
            {
                timer = Time.time;
                Vector3 newPointPosition = camera.transform.GetChild(1).position;
                create3DPoint(newPointPosition);
            }
        }
        if (Input.GetKey(KeyCode.Return) && currentlyCreatingPath && !currentlyCreatingConnectingPath && singlePath.points.Count >= 3)
        {
            finishNewPath();
        }
        if (Input.GetKey(KeyCode.U) && currentlyCreatingPath && singlePath.points.Count > 0 && !currentlyCreatingConnectingPath)
        {
            if (Time.time - timer >= time)
            {
                timer = Time.time;
                singlePath.removeLastPoint();
            }
        }
    }
}
