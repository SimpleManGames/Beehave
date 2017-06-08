using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pauser : MonoBehaviour {
    public GameObject pauseText;
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale!=0)
            {
                Debug.Log("pause");
                pauseText.SetActive(true);
                Time.timeScale = 0;
            }
            else { Time.timeScale = 1; pauseText.SetActive(false); }
        }
	}
}
