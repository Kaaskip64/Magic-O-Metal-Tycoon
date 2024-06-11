using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartQuitScript : MonoBehaviour
{
    public Slider loadingBarrFill;
    
    public void StartGame(int sceneToLoad)
    {
        StartCoroutine(Loadscene(sceneToLoad));
    }

    public IEnumerator Loadscene(int SceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneID);
        
        while (!operation.isDone)
        {
            float progressvalue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarrFill.value = progressvalue;
            yield return null;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
