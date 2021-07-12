using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    ORANGEMUSHROOM = 0,
    REDMUSHROOM = 1
}
public class PowerupManagerEV : MonoBehaviour
{
    // reference of all player stats affected
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    private bool[] inUse = {false, false};

    void Awake()
    {
        Debug.Log("PowerupManager Awake");
    }
    
    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null)
                {
                    AddPowerupUI(i, p.powerupSprite);
                }
            }
        }
        powerupIcons.Clear();
        foreach (Transform icon in GameObject.Find("/Canvas/Powerups").transform)
        {
            powerupIcons.Add(icon.gameObject);
        }
    }
    
    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }
    
    void AddPowerupUI(int index, Sprite t)
    {
        
        powerupIcons[index].GetComponent<Image>().sprite = t;
        powerupIcons[index].SetActive(true);
    }

    public void AddPowerup(Powerup p)
    {
        Debug.Log("Adding powerup");
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupSprite);
    }

    public void AttemptConsumePowerup(KeyCode K)
    {
        Debug.Log("Consuming powerup");
        int index;
        if (K == KeyCode.Z)
        {
            index = 0;
        }else if (K == KeyCode.X)
        {
            index = 1;
        }
        else
        {
            return;
        }

        if (inUse[index])
        {
            return;
        }
        var powerup = powerupInventory.Get(index);
        marioMaxSpeed.ApplyChange(powerup.absoluteSpeedBooster);
        marioJumpSpeed.ApplyChange(powerup.absoluteJumpBooster);
        StartCoroutine(removePowerupEffect(index, powerup));
        inUse[index] = true;
        powerupIcons[index].SetActive(false);
        powerupInventory.Remove(index);
    }

    private IEnumerator removePowerupEffect(int index, Powerup p)
    {
        yield return new WaitForSeconds(p.duration);
        marioMaxSpeed.ApplyChange(p.absoluteSpeedBooster * -1);
        marioJumpSpeed.ApplyChange(p.absoluteJumpBooster * -1);
        inUse[index] = false;
    }

    public void OnApplicationQuit()
    {
        powerupInventory.Clear();
    }
}