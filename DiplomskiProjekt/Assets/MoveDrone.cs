using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MoveDrone : MonoBehaviour
{
    //lista i onda gledat jel za taj connecting path koji ide na neki iduci path bio
    public GameObject player;
    private int droneLevel = -1;
    private Path lastPath;
    private List<bool> pathVisited = new List<bool>();
    public List<Path> paths = new List<Path>();
    private List<Path> allPaths = new List<Path>(), tempPaths;
    private Vector3 nextPoint, direction;
    public Vector3 nextPathPosition =  new Vector3(0,0,0);
    private Quaternion nextRotation;
    private int index = 1, pathIndex, increment = 1, pathCircles = 1, pathCirclesToDo = 0;
    public int createdPathsIndex = 0;
    public List<int> pathCirc = new List<int>();
    bool rotation = true, move = false, connection = false;

    

    public string pathCircInc()
    {
        string str;
        Path selPath = player.GetComponentInChildren<PointMaker>().onePathSelected();
        if (selPath != null)
        {
            for(int i = 0; i < allPaths.Count; i++) {
                if (allPaths[i] == selPath)
                {
                    //neki if?
                    pathCirc[i]++;
                    Debug.Log("PathCirc(Inc)[" + allPaths[i].name + "]:" + pathCirc[i]);
                    str = allPaths[i].name + " " + i;
                    return str;
                }
            }
        }
        return "";
    }

    public string pathCircDec()
    {
        string str;
        Path selPath = player.GetComponentInChildren<PointMaker>().onePathSelected();
        if (selPath != null)
        {
            for (int i = 0; i < allPaths.Count; i++)
            {
                if (allPaths[i] == selPath)
                {
                    if(pathCirc[i] <= 1) {
                        break;
                    }
                    pathCirc[i]--;
                    Debug.Log("PathCircDec):" + pathCirc[i]);
                    str = allPaths[i].name + " " + i;
                    return str;
                }
            }
        }
        return "";
    }

    public void printAllPaths()
    {
        tempPaths = new List<Path>();
        PopulateAllPaths(paths[0]);
        /*Debug.Log("All paths for drone[" + this.name + "]:");
        foreach(Path path in allPaths)
        {
            Debug.Log(path.name);
        }*/
        //allPaths = allPaths.OrderBy(path => path.name).ToList();
    }

    public List<Path> getAllPaths()
    {
        return allPaths;
    }

    public void toggleMove()
    {
        move = !move;
    }

    public void toggleSelected()
    {
        GameObject selektor = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
        if (selektor.activeSelf)
        {
            selektor.SetActive(false);
        }
        else
        {
            selektor.SetActive(true);
        }
        
    }

    void ChangeLayerRecursively(Transform obj, int layer)
    {
        // Postavi Layer na trenutnom objektu
        obj.gameObject.layer = layer;

        // Iteriraj kroz svu djecu i pozovi istu funkciju za svako dijete
        foreach (Transform child in obj)
        {
            ChangeLayerRecursively(child, layer);
        }
    }

    private void changeLayerInAll(int layer)
    {
        gameObject.layer = layer;
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.layer = layer;
        }
    }
    private void changeLayer()
    {
        if (droneLevel != -1 && player != null)
        {
            if (droneLevel == 0)
            {
                ChangeLayerRecursively(gameObject.transform, 0);
            }
            else if (droneLevel == 1)
            {
                ChangeLayerRecursively(gameObject.transform, 11);
            }
            else if (droneLevel == 2)
            {
                ChangeLayerRecursively(gameObject.transform, 10);
            }
            else if (droneLevel == 3)
            {
                ChangeLayerRecursively(gameObject.transform, 9);
            }
            else if (droneLevel == 4)
            {
                ChangeLayerRecursively(gameObject.transform, 8);
            }
        }
        Physics.SyncTransforms();
    }

    private int checkLayer()
    {
        if(transform.position.y >= 0 && transform.position.y <= 4.0)
        {
            return 0;
        }
        else if(transform.position.y >= 4.0 && transform.position.y <= 8.0)
        {
            return 1;
        }
        else if (transform.position.y >= 8.0 && transform.position.y <= 12.0)
        {
            return 2;
        }
        else if (transform.position.y >= 12.0 && transform.position.y <= 16.0)
        {
            return 3;
        }
        else if (transform.position.y >= 16.0)
        {
            return 4;
        }
        return -1;
    }
    public void addNewPath(Path path)
    {
        paths.Add(path);
        nextPoint = paths[0].points[1].GetComponentInChildren<PointScript>().coordinates;
        //transform.rotation = Quaternion.LookRotation((paths[0].points[1].GetComponentInChildren<PointScript>().coordinates - paths[0].points[0].GetComponentInChildren<PointScript>().coordinates).normalized);
        Vector3 directionToTarget = nextPoint - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        droneLevel = checkLayer();
        changeLayer();
    }

    private void changePath()
    {
        GameObject currentPoint = null;
        for(int i = 0; i < paths[0].points.Count; i++)
        {
            if(transform.position == paths[0].points[i].GetComponentInChildren<PointScript>().coordinates)
            {
                currentPoint = paths[0].points[i];
                break;
            }
        }
        paths[0] = currentPoint.GetComponentInChildren<PointScript>().path;
        Debug.Log("Ruta: " + currentPoint.GetComponentInChildren<PointScript>().path.name);
        Debug.Log("Connecting ruta: " + currentPoint.GetComponentInChildren<PointScript>().connectingPath.name);
        for (int i = 0; i < paths[0].points.Count; i++)
        {
            if(currentPoint == paths[0].points[i])
            {
                index = i;
                break;
            }
        }
        if (getPathIndex() < pathCirc.Count && getPathIndex() > -1) {
            pathCircles = pathCirc[getPathIndex()] + 1;
        }
        else
        {
            pathCircles = 1;
        }
        pathCirclesToDo = 0;
    }

    private float CalculateYawAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
    }

    private void PopulateAllPaths(Path currentPath)
    {
        if (!tempPaths.Contains(currentPath))
        {
            if (!allPaths.Contains(currentPath) && !currentPath.isAConnectingPath)
            {
                allPaths.Add(currentPath);
                pathVisited.Add(false);
            }
            tempPaths.Add(currentPath);

            foreach (Path connectingPath in currentPath.connectingPaths)
            {
                foreach (Path connectedPath in connectingPath.paths)
                {
                    PopulateAllPaths(connectedPath);
                }
            }
        }
    }

    public int getPathIndex()
    {
        for(int i = 0; i < allPaths.Count; i++)
        {
            if (allPaths[i] == paths[0])
            {
                return i;
            }
        }
        return -1;
    }

    private bool CheckIfAllPathsVisited()
    {
        for(int i = 0; i < pathVisited.Count; i++)
        {
            if(pathVisited[i] == false)
            {
                Debug.Log("Path not visited:" + allPaths[i].name);
                return false;
            }
        }
        Debug.Log("All paths visited.");
        return true;
    }

    private bool CheckOtherPoints()
    {
        for(int i = 0; i < paths[0].points.Count; i++) //prodi kroz sve pointove trenutnog patha
        {
            if (paths[0].points[i] != paths[0].points[index]) //ako se ne radi o trenutnom pointu
            {
                if (paths[0].points[i].GetComponentInChildren<PointScript>().connectingPath != null) //pogledaj je li taj connecting
                {
                    for(int j = 0; j < allPaths.Count; j++) //prodi kroz sve pathove
                    {
                        if (paths[0].points[i].GetComponentInChildren<PointScript>().connectingPath.paths[0] == paths[0]) //odaberi onaj od connecting pathova koji nije ovaj na kojem si trenutno
                        {
                            if (allPaths[j] == paths[0].points[i].GetComponentInChildren<PointScript>().connectingPath.paths[1])//pogledaj je li taj ovaj na kojem si sad
                            {
                                if (pathVisited[j] == false) //ako nije proden vrati true
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (allPaths[j] == paths[0].points[i].GetComponentInChildren<PointScript>().connectingPath.paths[0])
                            {
                                if (pathVisited[j] == false)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                        
                }
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Rotacija: " + transform.GetChild(0).transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        if (getPathIndex() < pathCirc.Count && getPathIndex() > -1) 
        {
            if (pathCirc[getPathIndex()] != pathCircles)
            {
                pathCircles = pathCirc[getPathIndex()];
            }
        }
        if (move)
        {
            if (paths.Count > 0)
            {
                if (transform.position == nextPoint)
                {
                    if (paths[0].points[index].GetComponentInChildren<PointScript>().connectingPath != null && !connection && pathCirclesToDo >= pathCircles)
                    {
                        player.GetComponent<PlayerOptions>().updateDronePaths();
                        pathVisited[getPathIndex()] = true;
                        Path p = paths[0].points[index].GetComponentInChildren<PointScript>().connectingPath;
                        if (p.paths[0] == paths[0])
                        {
                            p = p.paths[1];
                        }
                        else
                        {
                            p = p.paths[0];
                        }
                        int indexPath = 0;
                        for(int i = 0; i < allPaths.Count; i++)
                        {
                            if(p == allPaths[i])
                            {
                                indexPath = i;
                            }
                        }
                        Debug.Log("Prosao path(" + allPaths[indexPath].name + ")? " + pathVisited[indexPath]);
                        if (pathVisited[indexPath] == true)
                        {
                            if (CheckIfAllPathsVisited())
                            {
                                for (int i = 0; i < pathVisited.Count; i++)
                                {
                                    pathVisited[i] = false;
                                }
                            }
                        }
                        bool otherPointChecker = CheckOtherPoints();
                        if (pathVisited[indexPath] == false || (pathVisited[indexPath] == true && pathVisited[getPathIndex()] == true && lastPath != allPaths[indexPath] && otherPointChecker == false))
                        {
                            lastPath = paths[0];
                            paths[0] = paths[0].points[index].GetComponentInChildren<PointScript>().connectingPath;
                            if (paths[0].points[0].GetComponentInChildren<PointScript>().coordinates == transform.position)
                            {
                                index = 0;
                                increment = 1;
                            }
                            else
                            {
                                index = paths[0].points.Count - 1;
                                increment = -1;
                            }
                            connection = true;
                            Debug.Log("Mijenjam rutu: " + paths[0].name);
                        }
                    }
                    else if (paths[0].points[index].GetComponentInChildren<PointScript>().connectingPath != null && connection)
                    {
                        changePath();
                        connection = false;
                        Debug.Log("Idem iducom rutom: " + paths[0].name);
                    }
                    if (!connection)
                    {
                        if (index + 1 == paths[0].points.Count)
                        {
                            index = 0;
                            pathCirclesToDo++;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    else
                    {
                        index += increment;
                    }
                    nextPoint = paths[0].points[index].GetComponentInChildren<PointScript>().coordinates;
                    direction = (nextPoint - transform.position).normalized;
                    nextRotation = Quaternion.LookRotation(direction);
                    nextRotation = Quaternion.Euler(0f, nextRotation.eulerAngles.y, 0f);
                    rotation = false;
                }
                else if (!rotation)
                {
                    transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.GetChild(0).transform.rotation, nextRotation, Time.deltaTime * 2.0f);
                    float angle = Quaternion.Angle(transform.GetChild(0).transform.rotation, nextRotation);
                    if (angle < 0.15f)
                    {
                        rotation = true;
                    }
                }
                else
                {
                    int layer = checkLayer();
                    if (layer != droneLevel)
                    {
                        droneLevel = layer;
                        changeLayer();
                    }
                    float step = (float)2 * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, nextPoint, step);
                }
            }
        }
    }
}
