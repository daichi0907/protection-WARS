using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMove : EnemyParent
{
 
 public float chaseSpeed = 0.1f;//追いかけるスピード
    public float MaxRange = 0.8f;//手が届くであろう距離
    public float CoolTime = 3f;//攻撃のクールタイム
    public float ExitDistance = 0.3f;//手が近くに来たら逃げる距離

    //Transform Target;
    //GameObject Hime;
    Transform RHandTarget;
    GameObject RHand;
    Transform LHandTarget;
    GameObject LHand;

    private int state = 0;
    private float time;
    float speed;
    float StopDistance = 0;
    public static bool gun = false;


    float removespeed;
    int turn = 0;
    float moveRange = 0f;

    float repostime;

    public static bool remove = false;

    public float Desk_height = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        //Hime = GameObject.FindWithTag("Princess");
        //Target = Hime.transform;
        speed = chaseSpeed;

        //右手のオブジェクト（名前）
        RHand = GameObject.Find("RightOVRHandPrefab");
        RHandTarget = RHand.transform;

        //左手のオブジェクト（名前）
        LHand = GameObject.Find("LeftOVRHandPrefab");
        LHandTarget = LHand.transform;
    }

    // Update is called once per frame
    void Update()
    {


        float distance = Vector3.Distance(transform.position, Target.transform.position);

        //手と銃歩兵との距離(Rが右手、Lが左手)
        float Rhanddistance = Vector3.Distance(transform.position, RHandTarget.position);
        float Lhanddistance = Vector3.Distance(transform.position, LHandTarget.position);

        //姫と手の距離がExitDistanceより小さい時に返す
        if (Rhanddistance < ExitDistance || Lhanddistance < ExitDistance)
        {
            if (IsBlownAway)
            {
                return;
            }

        }
        Vector3 TarPos = Target.transform.position;
        TarPos.y = transform.position.y;

        transform.LookAt(TarPos);

        float StopRange = Random.Range(0.3f, 0.5f);//姫から0.3～0.5のランダム距離

        if (StopDistance == 0)
        {
            StopDistance = StopRange;
        }
        if (gun == true)
        {
            gun = false;
        }



        //敵と姫の距離が0.3～0.5の距離になったら停止、それ以上の時は追いかける
        if (distance > StopDistance)
        {

            Chase();

        }
        else
        {
            StopDistance = MaxRange;
            //手の届かない距離になったら追いかける
            if (distance > StopDistance)
            {
                StopDistance = 0;
            }

            Debug.Log("停止");
            chaseSpeed = 0;

            switch (state)
            {
                case 0:
                    chaseSpeed = 0;
                    stop();
                    break;
                case 1:
                    chaseSpeed = 0;
                    shot();
                    break;
                default:
                    reposition();
                    break;

            }


        }

        void Chase()
        {
            if (distance > MaxRange)
            {
                chaseSpeed = speed;
            }
            Debug.Log("追いかける");
            transform.position += transform.forward * chaseSpeed * Time.deltaTime;

        }

        void stop()
        {

            time = time + Time.deltaTime;

            if (time > CoolTime)
            {
                time = 0;
                state = 1;
            }
        }

        void shot()
        {
            Debug.Log("うった");
            gun = true;
            state = 2;
            remove = true;
        }
    }
    void reposition()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        repostime += Time.deltaTime;
        if (remove == true)
        {
            turn = Random.Range(0, 2);
            moveRange = Random.Range(-0.3f, 0.3f);
            remove = false;
        }
        chaseSpeed = speed * 0.05f;

        if (distance < 0.4f)
        {
            turn = 0;
        }

        switch (turn)
        {
            case 0://左右にずれる
                Vector3 repos_x = new Vector3(transform.position.x + moveRange, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, repos_x, chaseSpeed);
                break;
            default://前後にずれる
                Vector3 repos_z = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveRange);
                transform.position = Vector3.MoveTowards(transform.position, repos_z, chaseSpeed);
                break;
        }

        Debug.Log("うったあとにうごいてる");
        if (repostime > 0.5f)
        {
            state = 0;
            repostime = 0f;
        }


    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // hit がいらなくなり次第if 文なくす
        HandHit(hitcollision.gameObject);

        // 敵にぶつかられた時の処理（敵に当たったかも判定）
        EnemyHit(hitcollision.gameObject);
    }
}
