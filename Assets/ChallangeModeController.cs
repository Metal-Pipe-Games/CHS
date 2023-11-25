using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallangeModeController : MonoBehaviour
{
    public GameObject crawler;
    public GameObject strongerCrawler;

    public Transform[] spawnPositions;
    public float spawnDelay = 20;
    public int enemyBonus = 0;
    public float strongerEnemyMultiplier = 0.005f;
    public float strongerEnemyPercentage = 0;
    public float t = 0;
    float clock = 0;
    public int killedDuringLastWave = 0;
    public int totalKilled = 0;
    public int score = 0;

    public TMP_Text timer;
    public TMP_Text timer2;

    public TMP_Text scoreText;

    public CanvasGroup group1;
    public CanvasGroup group2;

    public bool active = true;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            t += Time.deltaTime;
            clock += Time.deltaTime;
            int s = Mathf.FloorToInt(t % 60);
            int m = Mathf.FloorToInt(t / 60);
            float ms = (t % 1) * 100;

            timer.text = string.Format("{0:00}:{1:00}.{2:00}", m, s, ms);

            if (clock >= spawnDelay)
            {
                clock = 0;
                int count;
                if (killedDuringLastWave == 0) count = 1 + enemyBonus;
                else count = Random.Range(1 + enemyBonus, killedDuringLastWave);
                if (count >= spawnPositions.Length) count = spawnPositions.Length - 1;
                killedDuringLastWave = 0;

                for(int i = 0; i < count; i++)
                {
                    var pos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                    GameObject enemy;
                    bool stronger = Random.value < strongerEnemyPercentage;
                    if (stronger) { 
                        enemy = Instantiate(strongerCrawler);
                        i++;
                    }
                    else enemy = Instantiate(crawler);

                    enemy.transform.SetPositionAndRotation(pos.position, pos.rotation);
                    enemy.SetActive(true);
                }

            }
        }
        /*
        Varje 20 sekunder spawnas x fiender
        x = ett random nummer mellan 1 och totala mängden dödade fiender (under den förra waven) (där maximum nya enemies är  spawnpoints -1)
        Var 15'e kill ökar minimum enemies med 1
        y% chans för röd enemy, y = dödade enemies * 0.5 (20 dödade enemies för 10%)
         */
    }

    public void EnemyKill(int v)
    {
        score += v;
        killedDuringLastWave++;
        totalKilled++;
        enemyBonus = 15 % totalKilled;
        Debug.Log(enemyBonus);
        strongerEnemyPercentage = totalKilled * strongerEnemyMultiplier;
    }

    public void End()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        active = false;
        timer2.text = timer.text;
        scoreText.text = score.ToString();
        group1.alpha = 0;
        group1.interactable = false;
        group2.alpha = 1;
        Invoke("EndEnableButton", 3);
        foreach (var enemy in enemies)
        {
            if (enemy.activeInHierarchy) {
                var hc = enemy.GetComponent<EnemyHealth>();
                if(hc != null && hc.enabled)hc.Kill(); 
            }
        }
    }

    private void EndEnableButton()
    {
        group2.interactable = true;
    }

    public void ReturnToTitle()
    {
        SceneTransporter.GoToScene(2);
    }
}
