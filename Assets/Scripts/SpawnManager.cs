using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SpawnManagerInstance;
    void Awake(){
        SpawnManagerInstance = this;
        for (int j =  0; j  <  2; j++)
	        spawnFromPooler(ObjectType.fireGomba);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnFromPooler(ObjectType i){
        // static method access
        GameObject item =  ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item  !=  null){
            //set position, and other necessary states
            item.transform.position  =  new  Vector3(Random.Range(10f, 13f), item.transform.position.y, 0);
            item.SetActive(true);
        }
        else{
            Debug.Log("not enough items in the pool.");
            ObjectPooler.SharedInstance.itemsToPool[(int)i].expandPool = true;
            GameObject pickup =  ObjectPooler.SharedInstance.GetPooledObject(i);
            pickup.transform.position  =  new  Vector3(Random.Range(10f, 13f), pickup.transform.position.y, 0);
            pickup.SetActive(true);
        }
    }
}
