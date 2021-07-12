using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetShroom : MonoBehaviour, ConsumableInterface
{
    public Sprite s;
    private int direction = 1;
    private Rigidbody2D shroomBody;
    // Start is called before the first frame update
    void Start()
    {
        shroomBody = GetComponent<Rigidbody2D>();
        shroomBody.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Move right
        if (direction == 1){
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else {
            transform.Translate(Vector3.left * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        //Collides with smth, change direction
        if (col.gameObject.CompareTag("Obstacle")){
            direction *= -1;
        }
        // JUMPPPPP yeeehaw
        else if(col.gameObject.CompareTag("Ground")){
            shroomBody.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
        }
        
    }

    void OnTriggerEnter2D(Collider2D col){
        // gets picked up
        if (col.gameObject.CompareTag("Player")){
            // update UI
            CentralManager.centralManagerInstance.addPowerup(s, 0, this);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void consumedBy(GameObject player){
        player.GetComponent<PlayerController>().jumpMultiplier = 1.5f;
        StartCoroutine(removeEffect(player));
    }

    IEnumerator  removeEffect(GameObject player){
		yield  return  new  WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().jumpMultiplier = 1f;
        Destroy(gameObject);
	}
}
