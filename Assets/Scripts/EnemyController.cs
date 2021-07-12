using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameConstants gameConstants;
    private float originalX;
    private float maxOffset = 8.0f;
    private float enemyPatrolTime = 5.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
    private int rotator = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        enemyBody = GetComponent<Rigidbody2D>();
        originalX = transform.position.x;
        ComputeVelocity();
        GameManager.OnPlayerDeath += EnemyRejoice;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset){
            MoveGomba();
        }
        else{
            // Flip movement
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
        transform.Rotate (0,0,rotator*Time.deltaTime); //rotates 50 degrees per second around z axis
    }

    void ComputeVelocity(){
        velocity = new Vector2((moveRight)*maxOffset / enemyPatrolTime, 0);
    }

    void MoveGomba(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            if(col.transform.position.y - this.transform.position.y > 0.75f){
                // increase score
                CentralManager.centralManagerInstance.increaseScore();

                // flatten
                StartCoroutine(flatten());
                SpawnManager.SpawnManagerInstance.spawnFromPooler((ObjectType)Random.Range(0,2));
            }
            else{
                //hurt player
                CentralManager.centralManagerInstance.damagePlayer();
            }
        }
    }

    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
        //reset gameobject
        
		Debug.Log("Enemy returned to pool");
		yield  break;
	}

    void EnemyRejoice(){
        Debug.Log("Enemy killed mario");
        // do smth to sprite
        rotator = 60;
    }

}
