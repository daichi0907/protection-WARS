using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当 中薗 昂太

public class Ship_RScript : EnemyParent
{

    public Vector3 course; //針路
    private int coursep;//針路ポイント
    private float course_near = 0; //近くの針路
    private GameObject[] Point;//0～5 1～6
    public string[] pointname;//0～5 1～6
    public float ship_speed = 10;
    private Vector3 dir;

    public GameObject Rocket;//ミサイル
    public GameObject Fire_P;//発射位置
    public float fire_StartTime; //〇秒後に攻撃開始
    public float reloadTime;//□秒定期に発射

    public bool movef = true;
    //private bool canJudge = true; // フラグ

    void Start()
    {
        SetUp();

        Point = new GameObject[pointname.Length];//pointnameの書き込んだ配列数とGameObjectの配列数が同じになる。
        for (int i = 0; i < pointname.Length; i++)
        {
            Point[i] = GameObject.Find(pointname[i]);//pointnameに書き込んだ名前と同じGameObjectを見つけ、記録する。
        }

        //最初に(スポーン時に)一番近い周回ポイントを定める。
        for (int i = 0; i < Point.Length; i++)
        {
            if (course_near == 0 || course_near > Vector3.Distance(Point[i].transform.position, this.transform.position))
            {
                course_near = Vector3.Distance(Point[i].transform.position, this.transform.position);
                course = Point[i].transform.position;
                coursep = i;
            }
        }

        //スポーンしてから、〇秒後に攻撃開始、□秒定期に発射
        InvokeRepeating(nameof(Fire_Rocket), fire_StartTime, reloadTime);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pointname.Length; i++)
        {
            if (this.gameObject.transform.position == Point[i].transform.position && this.gameObject.transform.position == course)
            {
                coursep = i + 1;
                if (coursep == 6)
                {
                    coursep = 0;
                }
            }
        }

        if (movef == true)
        {
            course = Point[coursep].transform.position;

            dir = Target.transform.position - transform.position;
            dir.Normalize();
            var look = Quaternion.LookRotation(dir); //向きを変更する
            look.x = 0;
            look.z = 0;
            transform.rotation = look;

            transform.position = Vector3.MoveTowards(
                    transform.position, course, ship_speed * Time.deltaTime);//針路の方向に移動
        }
    }

    void Fire_Rocket()
    {
        Instantiate(Rocket, Fire_P.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // 手に当たった時の処理（手に当たったかも判定）
        HandHit(hitcollision.gameObject);

        // 敵にぶつかられた時の処理（敵に当たったかも判定）
        EnemyHit(hitcollision.gameObject);
    }
}
