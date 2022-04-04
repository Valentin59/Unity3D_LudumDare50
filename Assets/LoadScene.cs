using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneById(int id)
    {
        SceneManager.LoadScene(id);
    }

}
