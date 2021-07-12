using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CentralManager : MonoBehaviour
{
	public  GameObject powerupManagerObject;
	private  PowerupController powerUpManager;
    public  GameObject gameManagerObject;
	private  GameManager gameManager;
	public  static  CentralManager centralManagerInstance;
	
	void  Awake(){
		centralManagerInstance  =  this;
	}

	void  Start()
	{
		powerUpManager  =  powerupManagerObject.GetComponent<PowerupController>();
		gameManager = gameManagerObject.GetComponent<GameManager>();
	}

	public  void  increaseScore(){
		gameManager.increaseScore();
	}

    public void damagePlayer(){
        gameManager.damagePlayer();
    }

	public  void  consumePowerup(KeyCode k, GameObject g){
		powerUpManager.consumePowerup(k,g);
	}

	public  void  addPowerup(Sprite s, int i, ConsumableInterface c){
		powerUpManager.addPowerup(s, i, c);
	}

	public void changeScene(string sceneName)
	{
		StartCoroutine(LoadYourAsyncScene(sceneName));
	}


	IEnumerator LoadYourAsyncScene(string sceneName)
	{
		// The Application loads the Scene in the background as the current Scene runs.
		// This is particularly good for creating loading screens.
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			Debug.Log("changing scene??");
			yield return null;
		}
	}
}
