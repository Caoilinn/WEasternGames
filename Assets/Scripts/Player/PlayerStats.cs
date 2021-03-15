using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Player Stats
    public float health;
    public float stamina;
    public float speed;
    public float baseAtk = 10;
    public float criticalCoefficient = 1;
    public float comboCoefficient;
    public float criticalResetTime = 0;
    public HP hpUI;
    public Stamina staminaUI;
    private float maxStamina;
    private float restorePerSecond;
    public float hitStunValue;
    public float hitStunRestoreSecond;
    public bool isHitStun;
    public bool isStunRestoreTimeFinished = true;
    public bool isBlockStun;
    public bool isDeath;
    public bool playDeathOnce;
    #endregion

    #region Trigger
    public float readyToRestoreStaminaTime = 0;
    private float RestoreStaminaTime = 0;
    private bool isRestoreStamina = false;
    #endregion

    void Start()
    {
        health = 100;
        stamina = 100;
        speed = 4;
        comboCoefficient = 0;
        hitStunValue = 100;
        hitStunRestoreSecond = 0f;
        maxStamina = stamina;
        restorePerSecond = maxStamina * 1 / 50;
        isHitStun = false;
        isDeath = false;
        playDeathOnce = false;
        #region UI
        hpUI.SetMaxHP(health);
        staminaUI.SetMaxStaminaSlider(stamina);
        #endregion
    }

    void Update()
    {
        restoreStamina();
        loseCondition();
        Stun();
        setHealthUI();
        setStaminaUI();
        isDead();
        resetCriticalCoefficient();
    }

    private void resetCriticalCoefficient()
    {
        if(criticalResetTime > 0)
        {
            criticalResetTime -= Time.deltaTime;
        }
        if(criticalResetTime <= 0)
        {
            criticalCoefficient = 1.0f;
        }
    }

    private void isDead()
    {
        if(health <= 0 && isDeath == false)
        {
            playDeathOnce = true;
            isDeath = true;
        }
    }

    private void Stun()
    {
        GettingStun();
        RestoreStunValueAfterTime();
    }

    private void GettingStun()
    {
        if(hitStunValue <= 0)
        {
            hitStunValue = 100;
        }
    }

    private void RestoreStunValueAfterTime()
    {
        if (hitStunRestoreSecond > 0)
        {
            hitStunRestoreSecond -= Time.deltaTime;
            isStunRestoreTimeFinished = false;
        }
        if (hitStunRestoreSecond <= 0 && !isStunRestoreTimeFinished)
        {
            hitStunValue = 100;
            isStunRestoreTimeFinished = true;
        }
    }

    void setStaminaUI()
    {
        staminaUI.setStaminaSlider(stamina);
    }

    void setHealthUI()
    {
        hpUI.setHealth(health);
    }

    void loseCondition()
    {
        if (health <= 0)
        {
            GetComponent<PlayerMovementV2>().enableMovement = false;
            GetComponent<PlayerAction>().enabled = false;
        }
    }

    void restoreStamina()
    {
        if (GetComponent<PlayerMovementV2>().isRunning == false)
        {
            if(readyToRestoreStaminaTime > 0) // Time preparation before restore stamina
            {
                readyToRestoreStaminaTime -= Time.deltaTime;
                isRestoreStamina = false;
            }
            if (readyToRestoreStaminaTime <= 0) // Time preparation before restore stamina
            {
                isRestoreStamina = true;
            }

            if (isRestoreStamina)
            {
                if (RestoreStaminaTime > 0)
                {
                    RestoreStaminaTime -= Time.deltaTime;
                }
                if (RestoreStaminaTime <= 0 && stamina <= maxStamina)
                {
                    stamina += restorePerSecond;
                    if (stamina >= maxStamina)
                    {
                        stamina = maxStamina;
                    }
                    RestoreStaminaTime = setRestoreStaminaTime(0.1f );
                }
            }      
        }
        if (stamina <= 0)
        {
            stamina = 0;
            speed = 4;
            GetComponent<PlayerMovementV2>().OnRunningKeyReleased();

        }
    }

    public float setReadyToRestoreStaminaTime(float num)
    {
        return num;
    }
    private float setRestoreStaminaTime(float num)
    {
        return num;
    }

    public void DecreaseHPStamina(float hp, float st)
    {
        health -= hp;
        stamina -= st;
    }
}
