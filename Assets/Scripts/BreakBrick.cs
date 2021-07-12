using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{
    private bool broken = false;
    public GameObject brick;
    public GameObject coin;
    public GameConstants gameConstants;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  OnTriggerEnter2D(Collider2D col){
	if (col.gameObject.CompareTag("Player") &&  !broken){
        Instantiate(coin, new Vector3(this.transform.position.x, this.transform.position.y + 5f, this.transform.position.z), transform.rotation);
		broken  =  true;
		// assume we have 5 debris per box
		for (int x =  0; x<gameConstants.spawnNumberOfDebris; x++){
			Instantiate(brick, transform.position, Quaternion.identity);
		}
		// gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
		// gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
		// GetComponent<EdgeCollider2D>().enabled  =  false;
        Destroy(transform.parent.gameObject);
	}
}
}
