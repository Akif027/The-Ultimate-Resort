using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScreen;




    private void Start()
    {
        StartCoroutine(LoadSceneAsync(1));
    }



    IEnumerator LoadSceneAsync(int SceneId)
    {
        yield return new WaitForSeconds(0.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float ProgressValue = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }


}
