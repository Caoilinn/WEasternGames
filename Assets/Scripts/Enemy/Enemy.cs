using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CharacterController enemyController;
    public float HP;
    public HP3D hpUI;
    public float stamina;
    public Stamina staminaUI;
    private float maxStamina;
    private float restorePerSecond;
    public float speed;
    public float hitStunValue;
    public float hitStunRestoreSecond;
    public bool isStunRestoreTimeFinished = true;

    #region Trigger
    public float readyToRestoreStaminaTime = 0;
    private float RestoreStaminaTime = 0;
    private bool isRestoreStamina = false;
    #endregion

    void Start()
    {
        HP = 100;
        stamina = 100;
        maxStamina = stamina;
        restorePerSecond = maxStamina * 1 / 50;
        hpUI.SetMaxHP(HP);
        //staminaUI.SetMaxStaminaSlider(stamina);
        speed = 4;
        hitStunValue = 0;
        hitStunRestoreSecond = 0f;
    }

    void Update()
    {
        IsKilled();
        setEnemyHP();
        restoreStamina();
        setStaminaUI();
        Stun();
    }

    private void Stun()
    {
        GettingStun();
        RestoreStunValueAfterTime();
    }

    private void GettingStun()
    {
        if (hitStunValue <= 0)
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

    private void setEnemyHP()
    {
        hpUI.setHealth(HP);
    }

    void restoreStamina()
    {
        if (readyToRestoreStaminaTime > 0) // Time preparation before restore stamina
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
                RestoreStaminaTime = setRestoreStaminaTime(0.1f);
            }
        }

        if (stamina <= 0)
        {
            stamina = 0;
            speed = 4;
        }
    }

    private void IsKilled()
    {
        if(HP <= 0)
        {
            this.gameObject.SetActive(false);
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

    void setStaminaUI()
    {
        //staminaUI.setStaminaSlider(stamina);
    }

    public void DecreaseHPStamina(float hp, float st)
    {
        HP -= hp;
        stamina -= st;
    }
}

