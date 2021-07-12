using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "GameConstants", menuName =  "ScriptableObjects/GameConstants", order =  1)]
public class GameConstants : ScriptableObject
{
    // for Scoring system
    int currentScore;
    int currentPlayerHealth;

    // for Reset values
    public Vector3 gombaSpawnPointStart = new Vector3(8.5f, -3.25f, 0); // hardcoded location
    // .. other reset values 
    
    // for Break.cs
    public  int breakTimeStep =  30;
    public  int breakDebrisTorque =  10;
    public  int breakDebrisForce =  10;
    
    // for SpawnDebris.cs
    public  int spawnNumberOfDebris =  5;

    public float groundSurface = -2.9f;
    public int horizontalKnockback = 15;
    
    // Mario basic starting values
    public int playerMaxSpeed = 5;
    public int playerMaxJumpSpeed = 30;
    public int playerDefaultForce = 150;
}
