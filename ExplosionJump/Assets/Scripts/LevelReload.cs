using UnityEngine;

namespace Assets.Scripts
{
    public class LevelReload : MonoBehaviour {

        void Update () {
            // If player presser R, reload current level
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
