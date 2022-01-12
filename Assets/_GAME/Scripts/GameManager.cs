using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerCodes player;
    public GameObject enemyPrefab;
    private float waveTime = 5f, currenWaveTime;
    private int waveStep = 3, currentWaveStep;
    private float border = 25f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        InGameSystem();
    }

    private void InGameSystem()
    {
        if (currenWaveTime >= waveTime)
        {
            currenWaveTime = 0f;
            currentWaveStep++;
            if (currentWaveStep < waveStep)
            {
                CreateWave(6, 5f, 5f);
            }
            else
            {
                CreateHelpPacks();
                currentWaveStep = 0;
            }
        }
        else
        {
            currenWaveTime += Time.deltaTime;
        }
    }

    private void CreateWave(int enemyCount, float radius, float randomMultiplier)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float angle = i * Mathf.PI * 2f / enemyCount;
            Vector3 pos = player.transform.position +
                          new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius) +
                          Vector3.forward * Random.Range(-randomMultiplier, randomMultiplier) +
                          Vector3.right * Random.Range(-randomMultiplier, randomMultiplier);
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = pos;
            enemy.transform.rotation = Quaternion.identity;

        }
    }

    private void CreateHelpPacks()
    {
        Debug.Log("help packs");
    }

    public Vector3 GetInBorderPos(Vector3 pos)
    {
        Vector3 result = pos;
        result.x = Mathf.Clamp(result.x, -border, border);
        result.z = Mathf.Clamp(result.z, -border, border);
        return result;
    }
}
