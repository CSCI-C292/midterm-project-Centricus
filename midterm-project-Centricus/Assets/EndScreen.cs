using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + StatTracker.score;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
