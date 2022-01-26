using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using System.Net;
using System.Threading;
using System;

public class InputIO : MonoBehaviour {

    public PanelInput panelInput; //Panel
    string serverJson; //JSON we get from server.
    public bool ConfirmHit = true; //Has User Hit Confirm? (Also True at start)
    public bool ConfirmHitButton = false; //Has User Hit Confirm? (False at start)
                            // Use this for initialization
    public Text loadingText;
    public GameObject loadingImage;
	void Start () { 
        panelInput.getFromJson(serverJson);
        ConfirmHit = true;
        ConfirmHitButton = false;
    }




    /** Pushes our information to a JSON and attempts to read from server.
     * 
     */
    public void PushToJson() {
        serverJson = panelInput.ConvToJson();
        //panelInput.PrintAll();
        //Debug.Log(serverJson);
        readFile();
    }

    //used to cancel unpushed changes or to pull it for initialization
    public void PullFromServer() { panelInput.getFromJson(serverJson); }

    public void readFile()
    {
        string url = "https://covidmod.isi.jhu.edu";
        loadingImage.SetActive(true);
        loadingText.gameObject.SetActive(true);
        loadingText.text = "Waiting For Data...";
        ConfirmHit = false;
        ConfirmHitButton = true;
        /*Test with pre-generated Data. Change output.json to location.
        string path = System.IO.File.ReadAllText("Assets/output.json");
        Debug.Log(path);
        interpretJSON(path);
        loadingImage.SetActive(false);
        loadingText.text = "Received!";
        loadingText.gameObject.SetActive(false);
        ConfirmHit = true;
        ConfirmHitButton = true;
        TestEnd */
        StartCoroutine(Post(url,serverJson));
    }


    //POST request and handling.
    IEnumerator Post(string url, string bodyJsonString)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(url, bodyJsonString))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (request.responseCode == (long)System.Net.HttpStatusCode.OK)
            {
                Debug.Log("Data successfully sent to the server, Status Code: " + request.responseCode);
                var data = request.downloadHandler.text;
                //Debug.Log(data);
                interpretJSON(data);
                loadingImage.SetActive(false);
                loadingText.text = "Received!";
                yield return new WaitForSeconds(2);
                loadingText.gameObject.SetActive(false);
                ConfirmHit = true;
                ConfirmHitButton = true;
            }
            else
            {
                Debug.Log("Error sending data to the server, Status Code: " + request.responseCode);
            }
        }
    }


    buildings bigB = null;


    //Turns the JSON into an object we can use.
    private void interpretJSON(string jsonString)
    {
        bigB = JsonUtility.FromJson<buildings>(jsonString);
    }

    //Returns the necessary building information to add it to building's count.
    public int[] getBuildingValues(int pos, int hour)
    {
        if (pos < bigB.Buildings.Length && hour < bigB.Buildings[pos].InfectedDaily.Length)
        {
            int[] data = { bigB.Buildings[pos].InfectedDaily[hour], bigB.Buildings[pos].PeopleDaily[hour] };
            return data;
        }
        return null;
    }

    //Used for iterating through the buildings, and ensuring we don't cause an index out of bounds on accident.
    public int getBuildingsSize()
    {
        if (bigB != null)
        {
            return bigB.Buildings.Length;
        }
        return 0;
    }
    //Used for the inner loop when iterating through the buildings, ensuring we only get data for as much data was provided.
    public int getDaySize()
    {
        if(bigB == null || bigB.Buildings == null)
        {
            return 0;
        }
        return bigB.Buildings[0].InfectedDaily.Length/24;
    }

    //Array of Building. Used for interpretting the JSON.
    [System.Serializable]
    public class buildings
    {
        public building[] Buildings;
    }
    //Class that stores all the data from the JSON. :)
    [System.Serializable]
    public class building
    {
        public string BuildingName;
        public int[] InfectedDaily;
        public int[] PeopleDaily;

    }

}
