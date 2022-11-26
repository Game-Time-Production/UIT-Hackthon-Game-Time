using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skiping : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))// && isSkiping == false)
        {
            Debug.Log("Skip");
            //isDone = true;
            //isSkiping=true;
            //Time.timeScale = 3f;
        }
    }
}
