using System.Collections;
using UnityEngine;
public class Spawn : MonoBehaviour
{
    public MapGenerator map;
    public Enemy enemy;
    int spawnCount;//敌人当前波数，未生成敌人数量
    float nextSpawnTime;//当前波数内下一个敌人生成的时间间隔
    int aliveCount;//当前波数敌人剩余数量
   public  bool isDisabled;
    public static Spawn Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        spawnCount = 20;
        aliveCount = 20;
        MapGenerator.Instance.OnNewWave();
    }
    void Update()
    {
        if (!isDisabled)
        {
            if (spawnCount > 0  && Time.time > nextSpawnTime)
            {
                spawnCount--;
                nextSpawnTime = Time.time + 2;
                StartCoroutine(SpawnEnemy());
            }
        }
    }
    IEnumerator SpawnEnemy()
    {
        Transform tileTranform =map.GetReandomTransform();
        Material tileMat = tileTranform.GetComponent<Renderer>().material;
        float spawnTimer = 0;//用于记录时间
        while (spawnTimer< 1)
        {
            spawnTimer+=Time.deltaTime;
            tileMat.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(spawnTimer * 4, 1));
            yield return null;
        }
        Enemy spawnEnemy = Instantiate(enemy, tileTranform.position+Vector3.up, Quaternion.identity);
    }
   public  void OnEnemyDath()
    {
        aliveCount--;
        if(aliveCount <= 0)
        {
            Debug.LogError("quanbu jieshu");
            Game.Insatnce.End(true);
        }
    }
}
