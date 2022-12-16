using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaScript : MonoBehaviour
{
    #region Protected field
    protected GameObject gamemodeobj;
    protected GameModeController gamemode;
    #endregion


    /// <summary>
    /// スポーン処理のテンプレート
    /// 継承先でこの処理を書く
    /// </summary>
    #region Unity function
    //void Start()
    //{
    //    Setup()
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (gamemode.State == GameModeStateEnum.Play)
    //    {
    //        Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
    //    }
    //}
    #endregion


    #region Protected method
    protected void Setup()
    {
        gamemodeobj = GameObject.Find("GameModeController");
        gamemode = gamemodeobj.GetComponent<GameModeController>();
    }

    //敵の種類リストから、□秒間置きに、最大数〇体までスポーンさせる。
    protected void Spawncount(GameObject[] Enemy_List, float[] enemy_spawn_time, float[] enemy_count, float[] enemy_time, float[] max_enemy, GameObject range_A, GameObject range_B)
    {
        for (int i = 0; i < Enemy_List.Length; i++)
        {
            enemy_time[i] = enemy_time[i] + Time.deltaTime;

            if (enemy_time[i] > enemy_spawn_time[i])
            {
                enemy_time[i] = 0f;

                if (enemy_count[i] < max_enemy[i])
                {
                    enemy_count[i]++;
                    Spawn_Enemy(Enemy_List, i, range_A, range_B);
                }
            }
        }
    }

    protected void Spawn_Enemy(GameObject[] Enemy_List, int enemy, GameObject range_A, GameObject range_B)
    {
        float x = Random.Range(range_A.transform.position.x, range_B.transform.position.x);
        float y = Random.Range(range_A.transform.position.y, range_B.transform.position.y);
        float z = Random.Range(range_A.transform.position.z, range_B.transform.position.z);

        Instantiate(Enemy_List[enemy], new Vector3(x, y, z), Quaternion.identity);
    }

    #endregion

}
