using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueUpdate : MonoBehaviour {

    public InputIO inputIO;
    public char param; //which slider, etc. m(masks) r(room capacity) d(daily testing) c(contact tracing) v(vaccinated population) s(stay home) 
    Text percentageText;
    int min;
    int max;
    int sliderValue;
    public GameObject[] helpMenus;

    //sets the upper and lower bound for a slider, as well as the starting slider value
    public void SetSliderParam(int mn, int mx, int sv) { mn = min; max = mx; sliderValue = sv; percentageText.text = sliderValue + "%"; }

	// Use this for initialization
	void Start () {

        if(this.gameObject.tag != "ParentSlider") {
            min = 0;
            max = 100;
            sliderValue = min;
            if (param == 'm' || param == 'd' || param == 'c' || param == 'v') {
                percentageText = GetComponent<Text>();
                percentageText.text = sliderValue + "%";
            }
            if(param == 'r') {
                sliderValue = 100;
                percentageText = GetComponent<Text>();
                percentageText.text = sliderValue + "%";
            }
        }
    }

    public void SetMax(int m) { max = m; }
    public void SetMin(int m) { min = m; }

    //dynamic float in slider, updates display of current % text on screen,
    //also sets the value of the corresponding slider in the panelInput object
    public void TextUpdate(float value) {
        sliderValue = Mathf.RoundToInt(min + value * (max-min));
        percentageText.text = sliderValue + "%";
        inputIO.panelInput.SetAny(param, sliderValue);
    }

    public void ToggleUpdate(bool value) { //s = stay home
        if (value == true)
        {
            inputIO.panelInput.SetAny(param, 1);
        }
        else { inputIO.panelInput.SetAny(param, 2); }
    }

    public void turnOffHelp() {
        if(param == 'm') {
            helpMenus[0].SetActive(false);
        }
        if(param == 'r') {
            helpMenus[1].SetActive(false);
        }
        if(param == 'd') {
            helpMenus[2].SetActive(false);
        }
        if(param == 'c') {
            helpMenus[3].SetActive(false);
        }
        if(param == 'v') {
            helpMenus[4].SetActive(false);
        }
        if(param == 's') {
            helpMenus[5].SetActive(false);
        }
    }

    public void turnOnHelp() {
        if(param == 'm')  {
            helpMenus[0].SetActive(true);
        }
        if(param == 'r') {
            helpMenus[1].SetActive(true);
        }
        if(param == 'd') {
            helpMenus[2].SetActive(true);
        }
        if(param == 'c') {
            helpMenus[3].SetActive(true);
        }
        if(param == 'v') {
            helpMenus[4].SetActive(true);
        }
        if(param == 's') {
            helpMenus[5].SetActive(true);
        }
    }
}
