using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;

public class ChangeColor : MonoBehaviour
{
    public Material[] currentMaterials; //List of the Materials to use.
    public Color[] oldColor; //Default Color
    Shader tintShader; //If you want to use a shader.
    public InputIO input; //Points to the InputIO in C Controller.

    // Start is called before the first frame update
    void Start()
    {
        currentMaterials = gameObject.GetComponent<Renderer>().materials;
        tintShader = Shader.Find("Tint");
        oldColor = new Color[currentMaterials.Length];
         for(int i = 0;i < currentMaterials.Length;i++) 
        {
            oldColor[i] = currentMaterials[i].color; 
        }

    }

    // Update is called once per frame, if the building is not hovered return it back to the  orignal color.
    void Update()
    {
        if(!input.ConfirmHit)
        {
            revertColor();
        }
    }

    //On Highlight, you change the color of the object.
    public void onHighlight() 
    {
        for(int i = 0; i < currentMaterials.Length;i++) 
        {
            //currentMaterials[i].shader = tintShader;
            //currentMaterials[i].SetColor("Tint",new Color(241.0f/255.0f, 117.0f/255.0f, 117.0f/255f));
            //currentMaterials[i].SetColor("_TintColor", new Color(241.0f/255.0f, 117.0f/255.0f, 117.0f/255f));
            //currentMaterials[i].Tint = Color.red;
            currentMaterials[i].color = Color.gray;
        }
    }

    //On click change the color of the building.
    public void onClick() 
    {/*
        for(int i = 0;i < currentMaterials.Length;i++) 
        {
            //currentMaterials[i].SetColor("_Tint", new Color(0,0,0));
            currentMaterials[i].color = Color.red;
        }*/
    }

    //After the mouse leaves the object, it returns to the default color.
    public void revertColor() 
    {
        for(int i = 0; i < currentMaterials.Length;i++) 
        {
            currentMaterials[i].color = oldColor[i];
        }
    }

}
