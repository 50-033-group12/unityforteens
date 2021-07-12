using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour
{
    public GameObject powerup;
    private Animator boxAnimator;
    private bool onHit;
    // Start is called before the first frame update
    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.CompareTag("Player") && !onHit){
            onHit = true;
            boxAnimator.SetBool("onHit", onHit);
            // spawn mushroom
            Debug.Log("Spawn mushroomies");
            Instantiate(powerup, new Vector3(this.transform.position.x,this.transform.position.y + 1f, 0), this.transform.rotation);
        }
    }
}
