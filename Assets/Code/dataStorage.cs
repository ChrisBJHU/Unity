using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Unused, now in GlobalValues
public class dataStorage : MonoBehaviour
{
  // Start is called before the first frame update
    public int InfectedToday = 0; // People Infected Who Walked In Today
    public int InfectedTotal = 0; // People Infected Total
    public int PeopleTotal = 0; // People Walked in Total
    public int PeopleToday = 0; // People Walked in Today
    private bool isVisible; // Boolean if this object should be visible.
    private GameObject[] Lazers; //List of Lazers
    private GameObject[] LazerParent; //List of two Lazer Objects Templates
    private float zCoordinate; //zCoordinate of this object
    private float timer; //WaitTime (time between each day cycle)

    private List<GameObject> createdLazers;
    public GlobalValues GlobalValuesScript;
    private int totalLazers;
    private List<int> totalInfected;
    private GameObject parent;


    void Start()
    {
        InfectedToday = InfectedTotal = PeopleToday = PeopleTotal = 0;
        totalInfected = new List<int>();
        Lazers = GameObject.FindGameObjectsWithTag("Lazer");
        LazerParent = GameObject.FindGameObjectsWithTag("LazerHead");
        zCoordinate = this.transform.position.z;
        createdLazers = new List<GameObject>();
        timer = GlobalValuesScript.getWaitTime();
        totalLazers = 0;
        parent = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Updates this objects values and creates explosions when a day starts.
    // @param pause Checks if we just unpaused
    int time = 0;
    public void updateValues(int[] values, bool pause)
    {
        InfectedToday = values[0];
        InfectedTotal += values[0];
        PeopleToday = values[1];
        PeopleTotal += PeopleToday;
        if (time % 24 == 0)
        {
            totalInfected.Add(InfectedTotal);
        }
        
        time++;


        if (!pause)
        {
            totalLazers = InfectedToday;
        }
        StartCoroutine(waiter());
    }

    //get If this panel should be visible.
    public bool getVisible()
    {
        return isVisible;
    }

    //Inverses Visibility of panel.
    public void changeVisible()
    {
        isVisible = !isVisible;

    }

    public List<int> getInfectedList()
    {
        return totalInfected;
    }

    //Creates a new exploision on the object.
    private void newExplosion()
    {
        GameObject newGameObject;
        Renderer parentRender = parent.GetComponent<Renderer>();
        Vector3 randPosition = new Vector3(Random.Range(parentRender.bounds.min.x, parentRender.bounds.max.x), parentRender.bounds.max.y - 5, Random.Range(parentRender.bounds.min.z, parentRender.bounds.max.z));
        Quaternion rotation =  Quaternion.Euler(this.transform.localRotation.eulerAngles);
        rotation.y = 0;
        //rotation.z += 2;
        newGameObject = Instantiate(Lazers[0], randPosition, rotation);
        newGameObject.name = this.name;
        newGameObject.transform.parent = LazerParent[0].transform;
        newGameObject.transform.localScale = Lazers[0].transform.localScale;
        newGameObject.GetComponent<deleteObjectAfterTime>().waitTime = 1; 
        createdLazers.Add(newGameObject);
    } 

    //Disperses explosion into the day cycle.
    IEnumerator waiter()
    {
        while(totalLazers > 0)
        {
            yield return new WaitForSeconds(timer / InfectedToday);
            if (GlobalValuesScript.onSimulation)
            {
                newExplosion();
                totalLazers--;
            } else
            {
                break;
            }
        }
    }

    public void clearValues()
    {
        InfectedToday = 0;
        InfectedTotal = 0;
        PeopleToday = 0;
        PeopleTotal = 0;
        totalLazers = 0;
        totalInfected.Clear();
    }

    private bool IsInBounds(Bounds obj, Vector3 area) {
        return obj.min.x > area.x && obj.max.x < area.x && obj.min.y > area.y && obj.max.y < area.y;
    }
}
