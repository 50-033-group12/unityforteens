using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            // score add
            CentralManager.centralManagerInstance.increaseScore();
            // spawn enemy
            SpawnManager.SpawnManagerInstance.spawnFromPooler((ObjectType)Random.Range(0,2));
            Destroy(gameObject);
        }   
    }
}
