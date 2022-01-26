using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteObjectAfterTime : MonoBehaviour
{
    // Start is called before the first frame update

    public int waitTime; //Time between destroying each object.

    //When this class is called, start the timer to destroy the object.
    void Start()
    {
     StartCoroutine(SelfDestruct());
    }

    //Delete's the Object.
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(waitTime);
        if(waitTime > 0) {

            Destroy(gameObject);
        }
    }
}

