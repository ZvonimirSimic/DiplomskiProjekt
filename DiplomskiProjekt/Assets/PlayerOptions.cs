using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerOptions : MonoBehaviour
{
    private float time = 1.0f, timer;
    public List<GameObject> drones = new List<GameObject>();
    public GameObject drone, screen1, selectedDrone = null, droneBtn, pathBtn;
    private bool pause = true;
    private int screen = -1, numberOfButtons = 0, dronesNumber = 0, droneCount = 0;
    // Start is called before the first frame update

    public void ToggleControlsBox()
    {
        if (transform.GetChild(transform.childCount - 3).gameObject.activeSelf)
        {
            transform.GetChild(transform.childCount - 3).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(transform.childCount - 3).gameObject.SetActive(true);
        }
    }
    public void ExitApplication()
    {
        Debug.Log("Exited app!");
        Application.Quit();
    }

    public void SelectDroneButton()
    {
        for (int i = 3; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf)
            {
                selectDrone(drones[i - 3]);
                break;
            }
        }
    }

    public void IncPath()
    {
        if(selectedDrone != null)
        {
            string msg;
            int droneIndex = 0;
            GameObject button = null;
            for (int i = 0; i < drones.Count; i++)
            {
                if (drones[i] == selectedDrone)
                {
                    droneIndex = i;
                    break;
                }
            }
            for (int i = 3; i < transform.GetChild(0).childCount; i++)
            {
                if ((i - 3) == droneIndex)
                {
                    button = transform.GetChild(0).transform.GetChild(i).gameObject;
                    break;
                }
            }
            if (button.transform.GetChild(0).transform.childCount <= 0 || !button.activeSelf)
            {
                return;
            }
                msg = selectedDrone.GetComponentInChildren<MoveDrone>().pathCircInc();
            if(msg == "")
            {
                return;
            }
            Debug.Log(msg);
            string[] splitStr = msg.Split(' ');
            int index = int.Parse(splitStr[2]);
            button.transform.GetChild(0).transform.GetChild(index).GetComponentInChildren<TextMeshPro>().text = splitStr[0] + " " + splitStr[1] + "\n" + drones[droneIndex].GetComponentInChildren<MoveDrone>().pathCirc[index].ToString();
        }
    }

    public void DecPath()
    {
        if (selectedDrone != null)
        {
            string msg;
            int droneIndex = 0;
            GameObject button = null;
            for (int i = 0; i < drones.Count; i++)
            {
                if (drones[i] == selectedDrone)
                {
                    droneIndex = i;
                    break;
                }
            }
            for (int i = 3; i < transform.GetChild(0).childCount; i++)
            {
                if ((i - 3) == droneIndex)
                {
                    button = transform.GetChild(0).transform.GetChild(i).gameObject;
                    break;
                }
            }
            if (button.transform.GetChild(0).transform.childCount <= 0)
            {
                return;
            }
            msg = selectedDrone.GetComponentInChildren<MoveDrone>().pathCircDec();
            if (msg == "")
            {
                return;
            }
            Debug.Log(msg);
            string[] splitStr = msg.Split(' ');
            int index = int.Parse(splitStr[2]);
            button.transform.GetChild(0).transform.GetChild(index).GetComponentInChildren<TextMeshPro>().text = splitStr[0] + " " + splitStr[1] + "\n" + drones[droneIndex].GetComponentInChildren<MoveDrone>().pathCirc[index].ToString();
        }
    }
    
    private void CheckIfButtonExists(int index)
    {
        if(transform.GetChild(0).transform.childCount > (index + 3))
        {
            transform.GetChild(0).transform.GetChild(index + 3).GetComponent<SelectDroneButton>().ToggleSelect();
        }
    }

    public void selectDrone(GameObject newDrone)
    {
        int selectedDroneIndex = 0;
        if (selectedDrone != null) {
            for (int i = 0; i < drones.Count; i++)
            {
                if (selectedDrone == drones[i])
                {
                    selectedDroneIndex = i;
                    break;
                }
            } 
        }
        for(int i = 0; i < drones.Count; i++)
        {
            if(drones[i] == newDrone)
            {
                if (selectedDrone != newDrone)
                {
                    if (selectedDrone != null)
                    {
                        CheckIfButtonExists(selectedDroneIndex);
                        selectedDrone.GetComponentInChildren<MoveDrone>().toggleSelected();
                    }
                    CheckIfButtonExists(i);
                    selectedDrone = newDrone;
                    selectedDrone.GetComponentInChildren<MoveDrone>().toggleSelected();
                }
                else
                {
                    CheckIfButtonExists(selectedDroneIndex);
                    selectedDrone.GetComponentInChildren<MoveDrone>().toggleSelected();
                    selectedDrone = null;
                }
            }
        }
    }
    public void selectPressedPath(Path p)
    {
        for(int i = 0; i < drones.Count; i++)
        {
            List<Path> allPaths = drones[i].GetComponentInChildren<MoveDrone>().getAllPaths();
            for (int j = 0; j < allPaths.Count; j++)
            {
                if(p == allPaths[j])
                {
                    GameObject currentButton = transform.GetChild(0).transform.GetChild(j + 3).gameObject;
                    if(currentButton.transform.GetChild(0).childCount > 0)
                    {
                        for(int k = 0; k < currentButton.transform.GetChild(0).childCount; k++)
                        {
                            if (currentButton.transform.GetChild(0).transform.GetChild(k).GetComponentInChildren<TextMeshPro>().text.Contains(p.name))
                            {
                                currentButton.transform.GetChild(0).transform.GetChild(k).GetComponentInChildren<SelectedQuad>().SelectQuad();
                            }
                        }
                    }
                }
            }
        }
    }

    public void deselectAllPaths()
    {
        for(int i = 3; i < transform.GetChild(0).childCount; i++)
        {
            for(int j = 0; j < transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).childCount; j++)
            {
                transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(j).GetComponentInChildren<SelectedQuad>().DeselectQuad();
            }
        }
    }
    public void togglePause()
    {
        if (pause)
        {
            pause = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(transform.childCount-1).gameObject.SetActive(false);
            returnFromPathList();
            hideDroneList();
        }
        else
        {
            pause = true;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
        }
    }

    public void returnFromPathList()
    {
        transform.GetChild(transform.childCount - 2).gameObject.SetActive(false);
        transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        for (int i = 3; i < transform.GetChild(0).transform.childCount; i++)
        {
            transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);
        }
    }
    public void showPathList(int droneNum)
    {
        Debug.Log("Drone " + droneNum + " paths:");
        updateDronePaths();
        GameObject droneButton = null;
        transform.GetChild(transform.childCount - 2).gameObject.SetActive(true);
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            if(i != droneNum + 3)
            {
                transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                droneButton = transform.GetChild(0).transform.GetChild(i).gameObject;
            }
        }
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
        droneButton.transform.GetChild(0).transform.gameObject.SetActive(true);
        List<Path> allPaths = drones[droneNum].GetComponentInChildren<MoveDrone>().getAllPaths();
        int numOfPaths = allPaths.Count;
        droneButton.transform.GetChild(droneButton.transform.childCount - 1).transform.gameObject.SetActive(true);
        Debug.Log("Child count: " + droneButton.transform.GetChild(0).childCount);
        Debug.Log("NumOfPaths: " + numOfPaths);
        for (int i = 0; i < droneButton.transform.GetChild(0).childCount; i++)
        {
            droneButton.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
            if (droneButton.transform.GetChild(0).transform.GetChild(i).GetComponentInChildren<SelectedQuad>().path.isSelected)
            {
                droneButton.transform.GetChild(0).transform.GetChild(i).GetComponentInChildren<SelectedQuad>().SelectQuad();
            }
            else
            {
                droneButton.transform.GetChild(0).transform.GetChild(i).GetComponentInChildren<SelectedQuad>().DeselectQuad();
            }
        }
        for (int i = droneButton.transform.GetChild(0).childCount; i < numOfPaths; i++)
        {
            Vector3 position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z);
            GameObject newPathButton = Instantiate(pathBtn, position, pathBtn.transform.rotation);
            droneButton.transform.GetChild(0).transform.localPosition = new Vector3(0, -0.12f, 0.22f);
            newPathButton.transform.parent = droneButton.transform.GetChild(0).transform;
            newPathButton.transform.localPosition = new Vector3(0, 0, 0);
            int num = dronesNumber;
            int cpi = drones[droneNum].GetComponentInChildren<MoveDrone>().createdPathsIndex;
            newPathButton.transform.localPosition = drones[droneNum].GetComponentInChildren<MoveDrone>().nextPathPosition;

            drones[droneNum].GetComponentInChildren<MoveDrone>().nextPathPosition = new Vector3(drones[droneNum].GetComponentInChildren<MoveDrone>().nextPathPosition.x,
                drones[droneNum].GetComponentInChildren<MoveDrone>().nextPathPosition.y - 0.12f, drones[droneNum].GetComponentInChildren<MoveDrone>().nextPathPosition.z);
            drones[droneNum].GetComponentInChildren<MoveDrone>().createdPathsIndex++;
            newPathButton.transform.rotation = transform.GetChild(0).transform.GetChild(0).transform.rotation;
            drones[droneNum].GetComponentInChildren<MoveDrone>().pathCirc.Add(1);
            newPathButton.GetComponentInChildren<TextMeshPro>().text = allPaths[i].name + "\n" + drones[droneNum].GetComponentInChildren<MoveDrone>().pathCirc[i].ToString();
            Path selectedPath = allPaths[i];
            newPathButton.GetComponentInChildren<SelectedQuad>().setPath(selectedPath);
            if (selectedPath.isSelected)
            {
                newPathButton.GetComponentInChildren<SelectedQuad>().SelectQuad();
            }
            else
            {
                newPathButton.GetComponentInChildren<SelectedQuad>().DeselectQuad();
            }
            //
            newPathButton.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(delegate { selectedPath.ToggleSelectPath();
                if (selectedPath.isSelected)
                {
                    newPathButton.GetComponentInChildren<SelectedQuad>().SelectQuad();
                }
                else
                {
                    newPathButton.GetComponentInChildren<SelectedQuad>().DeselectQuad();
                }
            });
            //
            newPathButton.gameObject.SetActive(true);
            

        }
    }

    public void showDroneList()
    {
        int br = 0;
        transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        for (int i = 0; i < numberOfButtons; i++)
        {
            transform.GetChild(0).transform.GetChild(i + 3).gameObject.SetActive(true);
            if (transform.GetChild(0).transform.GetChild(i + 3).GetComponent<SelectDroneButton>().selected)
            {
                transform.GetChild(0).transform.GetChild(i + 3).GetComponent<SelectDroneButton>().SelectButton();
            }
        }
        for(int i = numberOfButtons; i < drones.Count; i++)
        {
            Vector3 position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z);
            GameObject newDroneButton = Instantiate(droneBtn, position, droneBtn.transform.rotation);
            newDroneButton.transform.parent = transform.GetChild(0);
            newDroneButton.AddComponent<SelectDroneButton>();
            newDroneButton.GetComponent<SelectDroneButton>().selectedMaterial = GetComponent<SelectDroneButton>().selectedMaterial;
            newDroneButton.GetComponent<SelectDroneButton>().defaultMaterial = GetComponent<SelectDroneButton>().defaultMaterial;
            if (drones[i] == selectedDrone)
            {
                newDroneButton.GetComponent<SelectDroneButton>().SelectButton();
            }
            int num = dronesNumber;
            //newDroneButton.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(delegate { selectDrone(drones[num]); });
            newDroneButton.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { showPathList(num); });
            dronesNumber++;


            //newDroneButton.transform.localPosition = new Vector3(0, newDroneButton.transform.localPosition.y - (float)((i+1) * 0.12), 0);
            newDroneButton.transform.localPosition = new Vector3(0, -0.12f, newDroneButton.transform.localPosition.z - (float)((i + 1) * 0.22));

            newDroneButton.transform.rotation = transform.GetChild(0).transform.GetChild(0).transform.rotation;
            newDroneButton.GetComponentInChildren<TextMeshPro>().text = "Drone " + i;
            newDroneButton.gameObject.SetActive(true);
            br++;
            newDroneButton.transform.GetChild(4).transform.localPosition = new Vector3(0, newDroneButton.transform.GetChild(4).transform.localPosition.y, newDroneButton.transform.GetChild(4).transform.localPosition.z + (droneCount) * 0.22f);
            droneCount++;
        }
        numberOfButtons += br;
        Debug.Log("Number of buttons: " + numberOfButtons);
    }
    public void updateDronePaths()
    {
        Debug.Log("Drones:");
        for(int i = 0; i < drones.Count; i++)
        {
            drones[i].GetComponentInChildren<MoveDrone>().printAllPaths();
        }
        Debug.Log("Done!");
    }

    public void hideDroneList()
    {
        transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        for (int i = 0; i < numberOfButtons; i++)
        {
            transform.GetChild(0).transform.GetChild(i+3).gameObject.SetActive(false);
        }
    }

    public void nextCamera(string arrow)
    {
        if (screen == -1)
        {
            screen = drones.Count - 1;
        }
        else
        {
            if (arrow == "Left")
            {
                if (screen == 0)
                {
                    screen = drones.Count - 1;
                }
                else
                {
                    screen--;
                }
            }
            else if (arrow == "Right")
            {
                if (screen == drones.Count - 1)
                {
                    screen = 0;
                }
                else
                {
                    screen++;
                }
            }
            screen1.GetComponentInChildren<CameraScreen>().changeCamera(screen);
        }
    }

    private void createBtn()
    {
        GameObject go = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        timer = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timer >= time)
        {
            if (Input.GetKey(KeyCode.N))
            {
                timer = Time.time;
                Debug.Log("L");
                Path path = GetComponent<PointMaker>().selectedPath();
                if (path != null)
                {
                    Vector3 coordinates = path.points[0].GetComponentInChildren<PointScript>().coordinates;
                    Quaternion rotation = path.lines[0].gameObject.transform.rotation;
                    GameObject droneObject = Instantiate(drone, coordinates, rotation);
                    droneObject.name = "Drone" + (screen + 1).ToString();
                    droneObject.GetComponentInChildren<TextMeshPro>().text = (screen+1).ToString();
                    droneObject.GetComponent<MoveDrone>().player = this.gameObject;
                    droneObject.GetComponent<MoveDrone>().addNewPath(path);
                    drones.Add(droneObject);
                    screen1.GetComponentInChildren<CameraScreen>().getDrone(droneObject);
                    screen++;

                }
            }
            if (Input.GetKey(KeyCode.I))
            {
                if(selectedDrone != null)
                {
                    timer = Time.time;
                    int selectedDroneIndex = 0;
                    if (selectedDrone != null)
                    {
                        for (int i = 0; i < drones.Count; i++)
                        {
                            if (selectedDrone == drones[i])
                            {
                                selectedDroneIndex = i;
                                break;
                            }
                        }
                    }
                    CheckIfButtonExists(selectedDroneIndex);
                    selectedDrone.GetComponentInChildren<MoveDrone>().toggleSelected();
                    selectedDrone = null;
                }
            }
            if (Input.GetKey(KeyCode.M))
            {
                if (selectedDrone != null)
                {
                    timer = Time.time;
                    selectedDrone.GetComponentInChildren<MoveDrone>().toggleMove();
                }
            }
            if (Input.GetKey(KeyCode.C))
            {
                timer = Time.time;
                ToggleControlsBox();
            }
        }
    }
}
