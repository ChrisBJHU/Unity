using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Window_Graph : MonoBehaviour {

    public RectTransform graphContainer; //Holds all graph dependancies
    private RectTransform labelTemplateX; //Label on X
    private RectTransform labelTemplateY; //Label on Y
    public GameObject graph; //Actual graph
    List<GameObject> temp; //List of Circles
    private bool isGeneric = true; // T = generic graph (big one), F = building graph (smaller in menu).
    public GameObject tempCircle; //Circle Template

    public Text[] text; //Text Object for Graph (0 for generic, 1 for building specific)
    public GameObject[] textBox; //TextBox for Graph (0 for generic, 1 for building specific)


    /**
     * On awake, make the List of Circles.
     */
    private void Awake() {
        temp = new List<GameObject>();
    }

    /** Creates a circle at the correct position.
     * @param anchoredPosition Position to make the Circle.
     * @return the new Circle
     */
    private GameObject CreateCircle(Vector2 anchoredPosition, int value, int pos) {
        GameObject gameObject = Instantiate(tempCircle);
        gameObject.name = "" + pos + " " + value;
        gameObject.SetActive(true);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.AddComponent(typeof(EventTrigger));
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        temp.Add(gameObject);
        Vector3 scale = rectTransform.transform.localScale;
        scale = isGeneric ? scale * 2 : scale * 2;
        rectTransform.transform.localScale = scale;
        return gameObject;
    }


    //Turn on the label for the object currently hovered.
    public void updateTextLabel(GameObject go)
    {
        string[] data = go.name.Split(' ');
        if (isGeneric)
        {
            textBox[0].SetActive(true);
            text[0].text = "       Day: " + data[0] + "\nInfected: " + data[1];
        } else
        {
            textBox[1].SetActive(true);
            text[1].text = "       Day: " + data[0] + "\nInfected: " + data[1];
        }
    }

    //If not hovered, label is turned off.
    public void defaultText()
    {
        if (isGeneric)
        {
            text[0].text = "Hidden";
            textBox[0].SetActive(false);
        } else
        {
            text[1].text = "Hidden";
            textBox[1].SetActive(false);
        }
    }


    //Method that creates the graph.
    private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = isGeneric ? 1500f : 300f;
        float xSize = isGeneric ? 2.45f: 6f; //15 has 25 days available.
        
        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), valueList[i], i);
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    //Creates the lines between the graph points.
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        temp.Add(gameObject);
    }

    //Returns the angle from a line between two points.
    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    
    //Method for other classes to call to add values to the graph.
    public void createGraph(List<int> valueList)
    {
        cleanGraph();
        List<int> valueTest = new List<int>();
        /*Test Start
         Puts in generic values for the graph.
        for (int i = 0; i < valueList.Count; i++) valueTest.Add(i * 5);
       ShowGraph(valueTest, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f)); 
        */
       ShowGraph(valueList, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }

    //Method to clear and shut off the graph.
    public void cleanGraph()
    {
        temp.ForEach(Destroy);
        temp.Clear();
    }

    //Flips the graph type we're using.
    public void switchGraph()
    {
        isGeneric = !isGeneric;
        cleanGraph();
        defaultText();
    }

}

