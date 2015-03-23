using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

    public void LoadScene(string levelName)
    {
        Application.LoadLevel(levelName);
    }
}