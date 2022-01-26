using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GlobalValues : MonoBehaviour
{
    public bool onSimulation; //Is the simulation running? If true, time is counting up!
    public Text dayStorage; //day Text Object
    private int time; //Day counter
    private float timer; //Time counter
    public float waitTime; //Seconds between each Day Change
    public InputIO inputIO;
    public GameObject[] buildings;
    public GameObject Image;
    public GameObject genericGraph;
    public GameObject InformationUI;
    private GameObject last;
    public Window_Graph graph;
    public Window_Graph genericGraphGraph;
    public Text Name;// = transform.Find("Name").GetComponent<Text>();
    public Text InfectedTotal;// = transform.Find("Infected Total").GetComponent<Text>();
    public Text InfectedToday;// = transform.Find("Infected Today").GetComponent<Text>();
    public Text PeopleToday;//= transform.Find("People Today").GetComponent<Text>();
    public Text PeopleTotal;// = transform.Find("People Total").GetComponent<Text>();
    public List<int> totalInfected;
    private int previousTime; //Used only for the big graph, I need to know where to start getting the totals.
    public Button Play;
    public Button Pause;

    // Start is called before the first frame update
    void Start()
    {
        onSimulation = false;
        time = 0;
        Image.SetActive(false);
        totalInfected = new List<int>();
    }

    /* Update is called once per frame
        Checks if a Day has passed, and if so, updates values.*/ 
    void Update()
    {
        if(onSimulation == true && inputIO.ConfirmHit) 
        {
            timer += Time.deltaTime;
            
            if(timer > waitTime)
            {
                time++;
                timer = timer - waitTime;
                UpdateVals(false);
            }
            if (time == 0 && timer == 0) {
                totalInfected.Add(0);
            }
        }
        if (!inputIO.ConfirmHit)
        {
            changeStatus(false);
        }
    }

    //Simple switch to make the simulation start.
    public void changeStatus(bool onStatus) 
    {
        if (inputIO.ConfirmHitButton)
        {
            onSimulation = onStatus;
            if (onStatus)
            {
                Play.GetComponent<Image>().color = Color.gray;
                Pause.GetComponent<Image>().color = Color.white;
            }
            else
            {
                Pause.GetComponent<Image>().color = Color.gray;
                Play.GetComponent<Image>().color = Color.white;
            }
        }
    }

    //Returns WaitTime
    public float getWaitTime()
    {
        return waitTime;
    }

    //Updates All Values to the new date values.
    // @param pauseBuffer check if we just unpaused.
    public void UpdateVals(bool pauseBuffer)
    {
        int totalInfected = 0;
        GameObject data = null;
        for (int i = 0; i < inputIO.getBuildingsSize(); i++)
        {
            int[] values = inputIO.getBuildingValues(i, time);
            if (values != null)
            {
                buildings[i].GetComponent<dataStorage>().updateValues(values, pauseBuffer);
                if (buildings[i].GetComponent<dataStorage>().getVisible())
                {
                    updateData(buildings[i]);
                    data = buildings[i];
                }
                dataStorage DS = buildings[i].GetComponent<dataStorage>();
                totalInfected += DS.InfectedTotal;
            }
        }

        if (time % 24 == 0 & time/24 < inputIO.getDaySize())
        {
            this.totalInfected.Add(totalInfected);
            dayStorage.text = "Day: " + time / 24;
            if (data != null && Image.activeSelf)
            {
                graph.createGraph(data.GetComponent<dataStorage>().getInfectedList());
            } else
            {
                genericGraphGraph.createGraph(this.totalInfected);
            }
        }
    }

    // Change all the text of the children Text objects to the values held in data.
    // Updates once per frame
    public void updateData(GameObject data)
    {
        dataStorage DS = data.GetComponent<dataStorage>();
        Name.text = DS.name;
        InfectedTotal.text = "Infected Total:\n" + DS.InfectedTotal;
        InfectedToday.text = "Infected Today:\n" + DS.InfectedToday;
        PeopleToday.text = "People Today:\n" + DS.PeopleToday;
        PeopleTotal.text = "People Total:\n" + DS.PeopleTotal;
    }


    //If the building is pressed this shows the data and graph for that building.
    public void updateDataButton(GameObject data)
    {
        if (inputIO.ConfirmHit)
        {
            Image.SetActive(true);
            genericGraph.SetActive(false);
            genericGraphGraph.switchGraph();
            graph.switchGraph();
            graph.createGraph(data.GetComponent<dataStorage>().getInfectedList());
            if (last == null)
            {
                last = data;
            }
            if (last != data)
            {
                last.GetComponent<dataStorage>().changeVisible();
                last = data;
                graph.createGraph(data.GetComponent<dataStorage>().getInfectedList());
            }
            data.GetComponent<dataStorage>().changeVisible();
            updateData(data);
        }
    }

    public void closeData() //Sets all of the children to inactive.
    {
        Image.SetActive(false);
        genericGraph.SetActive(true);
        graph.cleanGraph();
        graph.switchGraph();
        genericGraphGraph.switchGraph();
        genericGraphGraph.createGraph(this.totalInfected);
    }

    public void closeInformationUI()
    {
        InformationUI.SetActive(false);
    }

    public void resetSimulator()
    {
        for (int i = 0; i < inputIO.getBuildingsSize(); i++)
        {
            buildings[i].GetComponent<dataStorage>().clearValues();
         
        }
        time = 0;
        timer = 0;
        totalInfected.Clear();
        dayStorage.text = "Day: " + time / 24;
        InfectedTotal.text = "Infected Total:\n" + 0;
        InfectedToday.text = "Infected Today:\n" + 0;
        PeopleToday.text = "People Today:\n" + 0;
        PeopleTotal.text = "People Total:\n" + 0;
        if (Image.activeSelf)
        {
            graph.createGraph(this.totalInfected);
        }
        else
        {
            genericGraphGraph.createGraph(this.totalInfected);
        }
        changeStatus(false);

    }

    public void openInformationUI()
    {
        if (inputIO.ConfirmHitButton)
        {
            InformationUI.SetActive(true);
        }
    }

}
