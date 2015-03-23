using UnityEngine;
using System.Collections;

public class returnToLevelSelect : MonoBehaviour {

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("levelSelect");
        }
	}
}
