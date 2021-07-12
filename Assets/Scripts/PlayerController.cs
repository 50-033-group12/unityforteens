using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameConstants gameConstants;
    public float speed;
    public float upSpeed;
    public float maxSpeed;
    
    // Power Up
    public float speedMultiplier = 1f;
    public float jumpMultiplier = 1f;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;
    private bool onGround = true;
    private bool faceRight = true;
    private bool isHurt = false;

    // Restart (text and panel and button)
    public Button restartButton;
    public Text gameOverText;
    public Image panel;

    // SFX
    public AudioSource bgm;
    private AudioSource marioSound;
    public AudioClip jumpSound;
    public AudioClip collideSound;
    public AudioClip gameOverSound;

    // Particle System
    public ParticleSystem dustCloud;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnPlayerDeath += PlayerDiesSequence;
        // Set to 30 FPS
        Application.targetFrameRate = 30;

        // Get Components
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioSound = GetComponent<AudioSource>();
        marioAnimator = GetComponent<Animator>();

        // Start Theme Song
        marioSound.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHurt)
        {
            return;
        }
        // Movement
        int dir = GetInputDirection();
        if (dir != 0)
        {
            var impulse = dir * speed * speedMultiplier;
            var newSpeed = marioBody.velocity.x + impulse;
            if (Math.Abs(newSpeed) < (maxSpeed * speedMultiplier))
            {
                marioBody.velocity += new Vector2(impulse, 0);
            }
        }
        else
        {
            marioBody.velocity = new Vector2(0, marioBody.velocity.y);
        }
        
        // marioBody.velocity = new Vector2(UnityEngine.Mathf.Clamp(marioBody.velocity.x, -maxSpeed, maxSpeed),
        //     marioBody.velocity.y);
        // Jump
        if (Input.GetKeyDown("space") && onGround){
            marioBody.AddForce(Vector2.up * upSpeed * jumpMultiplier, ForceMode2D.Impulse);

            // Play sfx
            marioSound.PlayOneShot(jumpSound, 0.7F);

            onGround = false;
            marioAnimator.SetBool("onGround", onGround);
        }
    }

    int GetInputDirection()
    {
        if (Input.GetKey("a"))
        {
            return -1;
        } else if (Input.GetKey("d"))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void Update()
    {
        // Toggling Sprite Left
        if (Input.GetKeyDown("a") && faceRight){
            if (Mathf.Abs(marioBody.velocity.x) >  1.0){
                marioAnimator.SetTrigger("onSkid");
            }
            faceRight = false;
            marioSprite.flipX = true;
        }

        // Toggling Sprite Right
        if (Input.GetKeyDown("d") && !faceRight){
            if (Mathf.Abs(marioBody.velocity.x) >  1.0){
                marioAnimator.SetTrigger("onSkid");
            }
            faceRight = true;
            marioSprite.flipX = false;
        }
        
        // flash red if hurt
        // maybe change this to animation
        if (isHurt)
        {
            if (Time.frameCount % 5 < 3)
            {
                marioSprite.color = Color.red;
            }
            else
            {
                marioSprite.color = Color.white;
            }
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        //To consume powerup
        if (Input.GetKeyDown("z")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }

        if (Input.GetKeyDown("x")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }
    }

    // For collision with the ground or obstacles
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle")){
            dustCloud.Play();
            onGround = true;
            if (isHurt)
            {
                isHurt = false;
                marioSprite.color = Color.white;
            }
            marioAnimator.SetBool("onGround", onGround);
        }
    }

    void PlayerDiesSequence(){
        Debug.Log("Mario ded");
        if (bgm.isPlaying){
            //Stop theme song
            bgm.Stop();
        }
        marioSound.PlayOneShot(gameOverSound, 0.7F);
        // yeet
        marioBody.constraints = RigidbodyConstraints2D.None;
        marioBody.AddForce(new Vector2(gameConstants.horizontalKnockback * 1.5f,50), ForceMode2D.Impulse);
        marioBody.AddTorque(10f, ForceMode2D.Impulse);
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        // Enable panel and gameover text
        panel.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        enabled = false;
    }

    // void OnTriggerEnter2D(Collider2D other){
    //     if (other.gameObject.CompareTag("Enemy")){
    //         // Play SFX
    //         marioSound.PlayOneShot(collideSound, 1F);
    //         life--;
    //         // Update the life ui
    //         Destroy(marioLife[life].gameObject);
    //         // knock back mario
    //         marioBody.velocity = Vector2.zero;
    //         int horizontalKnockback = 15;
    //         isHurt = true;
    //         marioSprite.color = Color.red;
    //         if (other.ClosestPoint((Vector2) transform.position).x > transform.position.x)
    //         {
    //             horizontalKnockback *= -1;
    //         }
    //         if (life <1){
    //             if (bgm.isPlaying){
    //                 //Stop theme song
    //                 bgm.Stop();
    //             }
    //             marioSound.PlayOneShot(gameOverSound, 0.7F);
    //             // yeet
    //             marioBody.constraints = RigidbodyConstraints2D.None;
    //             marioBody.AddForce(new Vector2(horizontalKnockback * 1.5f,50), ForceMode2D.Impulse);
    //             marioBody.AddTorque(10f, ForceMode2D.Impulse);
    //             this.GetComponent<BoxCollider2D>().isTrigger = true;
    //             // Enable panel and gameover text
    //             panel.gameObject.SetActive(true);
    //             gameOverText.gameObject.SetActive(true);
    //             restartButton.gameObject.SetActive(true);
    //             enabled = false;
    //         }
    //         else
    //         {
    //             marioBody.AddForce(new Vector2(horizontalKnockback, 30), ForceMode2D.Impulse);
    //         }
    //     }
    // }
}
