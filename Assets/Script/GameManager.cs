using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;

    public Player player;
    public Boss boss;
    public GameObject HpShop;
    public GameObject itemShop;
    public GameObject startZone;

    public int stage;
    public float playTime;

    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    public GameObject menuPanel;
    public GameObject gamePanel;

    public Text maxScoreText;
    public Text scoreTxt;

    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    public HealthBar BossHealthBar;

    void Awake()
    {
        maxScoreText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    #region Stage
    public void StageStart()
    {
        HpShop.SetActive(false);
        itemShop.SetActive(false);
        startZone.SetActive(false);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        // player 위치 초기화
        player.transform.position = Vector3.up * 0.8f;

        HpShop.SetActive(true);
        itemShop.SetActive(true);
        startZone.SetActive(true);

        isBattle = false;
        stage++;
    } 
    
    IEnumerator InBattle()
    {
        yield return new WaitForSeconds(5);
        StageEnd();
    }
    #endregion

    void Update()
    {
        if (isBattle)
        {
            playTime += Time.deltaTime;
        }
    }

    public void BossHpUI()
    {
        // 보스 체력 UI
        BossHealthBar.UpdateHealth(boss.curHealth, boss.maxHealth);
    }

    void LateUpdate()
    {
        // 상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE" + stage;
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600)/ 60);
        int second = (int)(playTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        // 플레이어 UI
        playerHealthTxt.text = player.health + "/" + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
        {
            playerAmmoTxt.text = "- /" + player.ammo;
        }
        else if (player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerAmmoTxt.text = "- /" + player.ammo;
        }
        else
        {
            playerAmmoTxt.text = player.equipWeapon.curAmmo + "/" + player.ammo;
        }
        
        // 무기 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0]? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // 몬스터 숫자 UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();


        // 보스 체력 UI
        BossHpUI();
    }
}
