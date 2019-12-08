using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public int playerScore;
    public float lightRadius = 5;
    public GameObject player;
    public GameObject enemyPrefab;
    public Terrain terrain;

    private static float tLeft, tRight, tTop,
       tBottom, tWidth, tLength, tHeight;

    public void Awake()
    {
        print("game manager was initialized");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void processTerrain()
    {
        tLeft = terrain.transform.position.x;
        tBottom = terrain.transform.position.z;
        tWidth = terrain.terrainData.size.x;
        tLength = terrain.terrainData.size.z;
        tHeight = terrain.terrainData.size.y;
        tRight = tLeft + tWidth;
        tTop = tBottom + tLength;
    }

    public static void init()
    {
        GameManager.manager = new GameManager();
    }

    public void initializeNewEnemy(int count=1)
    {
        print(enemyPrefab);
        CreateRandomObject(enemyPrefab, count, 0);
    }

    public void enemyWasKilled()
    {
        this.playerScore += 1;
        this.lightRadius += 1f;
        initializeNewEnemy();
    }

    public float getSearchRadius()
    {
        return this.lightRadius * 4;
    }

    public bool canShoot()
    {
        return this.lightRadius > 0;
    }

    public void didShoot()
    {
        this.lightRadius -= 1;
    }

    public Vector3 CreatePosition(GameObject resource, float addedHeight)
    {

        float rX = 0, rY = 0, rZ = 0;

        rX = (int)Random.Range(tLeft + 20, tRight - 20);
        rZ = (int)Random.Range(tBottom + 20, tTop - 20);

        print(terrain);
        rY = terrain.terrainData.GetHeight((int)rX, (int)rZ);

        return new Vector3(rX, rY + addedHeight, rZ);

    }

    public void CreateRandomObject(GameObject resource, int count, float addedHeight)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(resource, CreatePosition(resource, addedHeight), Quaternion.identity);
        }
    }

}
