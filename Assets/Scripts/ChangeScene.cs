using System.Collections;
using UnityEngine;

public  class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string targetScene;
    public  AudioSource changeSceneSound;
    void  OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("yeeet");
            changeSceneSound.PlayOneShot(changeSceneSound.clip);
            StartCoroutine(LoadYourAsyncScene(targetScene));
        }
    }

    IEnumerator  LoadYourAsyncScene(string sceneName)
    {
        yield  return  new  WaitUntil(() =>  !changeSceneSound.isPlaying);
        CentralManager.centralManagerInstance.changeScene(sceneName);
    }
}