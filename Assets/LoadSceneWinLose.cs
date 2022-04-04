using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWinLose : MonoBehaviour
{
    public string winSceneName = "";
    public string loseSceneName = "";

    public Health player;
    public Characters ennemiesInMap;

    // Start is called before the first frame update
    void Start()
    {
        player.onDieCallback.AddListener(LoadLoseScene);
        ennemiesInMap.onEmptySetCallback.AddListener(LoadWinScene);
    }

    public void LoadLoseScene()
    {
        player.onDieCallback.RemoveListener(LoadLoseScene);
        ennemiesInMap.onEmptySetCallback.RemoveListener(LoadWinScene);
        //Debug.Log("Load lose Level " + loseSceneName);
        SceneManager.LoadScene(loseSceneName);
    }

    public void LoadWinScene()
    {
        player.onDieCallback.RemoveListener(LoadLoseScene);
        ennemiesInMap.onEmptySetCallback.RemoveListener(LoadWinScene);
        //Debug.Log("Load win Level " + winSceneName);

        SceneManager.LoadScene(winSceneName);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        //player.onDieCallback.RemoveListener(LoadLoseScene);
        //ennemiesInMap.onEmptySetCallback.RemoveListener(LoadWinScene);
    }
}
