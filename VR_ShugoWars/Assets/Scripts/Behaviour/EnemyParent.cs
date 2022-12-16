using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyParent : MonoBehaviour
{
    #region Protected field
    protected float blownPower = 100f;
    protected float awaySpeed = 1.0f;
    protected GameObject Target;
    protected GameObject SpawnArea;
    protected SpawnArea_Air spawnArea_AirScript;
    protected GameObject SpawnGround;
    protected SpawnArea_Ground spawnArea_GroundScript;
    #endregion


    #region Field
    public bool IsBlownAway = false;
    public Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    public Vector3 vector;
    #endregion


    /// <summary>
    /// 吹っ飛ばされる処理のテンプレート
    /// 継承先でこの処理を書く
    /// </summary>
    #region Unity function
    //void Start()
    //{
    //    SetUp();
    //}

    //private void OnCollisionEnter(Collision hitcollision)
    //{
    //    // 手に当たった時の処理（手に当たったかも判定）
    //    HandHit(hitcollision.gameObject);

    //    // 敵にぶつかられた時の処理（敵に当たったかも判定）
    //    EnemyHit(hitcollision.gameObject);
    //}
    #endregion


    #region　Protected method
    // セットアップ処理
    protected void SetUp()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Target = GameObject.Find("Princess");

        SpawnArea = GameObject.Find("Spawn_Air");
        spawnArea_AirScript = SpawnArea.GetComponent<SpawnArea_Air>();

        SpawnGround = GameObject.Find("Spawn_Ground");
        spawnArea_GroundScript = SpawnGround.GetComponent<SpawnArea_Ground>();

    }

    // 手に当たったかを判定し、処理を行う
    protected void HandHit(GameObject other)
    {
        if (other.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            BlownAway(other.gameObject);
        }
    }

    // 敵に吹っ飛ばされたか判定し、処理を行う
    protected void EnemyHit(GameObject other)
    {
        // まず、接触したのが敵であるかを判定
        EnemyParent OtherData;
        bool Otherf;
        OtherData = other.gameObject.GetComponent<EnemyParent>();
        if (OtherData != null)
        {
            Otherf = OtherData.IsBlownAway;
        }
        else
        {
            return;
        }

        Debug.Log(Otherf);

        if ((other.gameObject.tag == "enemy_ship_r" && !IsBlownAway && Otherf)
            || (other.gameObject.tag == "enemy_tank" && !IsBlownAway && Otherf)
            || (other.gameObject.tag == "enemy_soldier" && !IsBlownAway && Otherf))
        {

            BlownAway_Enemy(other.gameObject);
        }
    }

    // プレイヤーに飛ばされた時の処理
    protected void BlownAway(GameObject other)
    {
        var handObj = other.transform.parent.parent.gameObject;
        GameObject handRigitObj;
        if (handObj.name == "LeftOVRHandPrefab")
        {
            handRigitObj = GameObject.Find("LeftRigitCollider");
        }
        else if (handObj.name == "RightOVRHandPrefab")
        {
            handRigitObj = GameObject.Find("RightRigitCollider");
        }
        else
        {
            return;
        }
        var handRC = handRigitObj.gameObject.GetComponent<HandRightCollider>();

        // 手の速さが一定以上でなければ吹っ飛ばない
        if (handRC.GetSpeed() < awaySpeed)
            return;

        var vec = handRC.GetVelocity();
        vec.y = 1.0f;
        vector = vec;

        // ナビメッシュ使用の場合はここの処理をする
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        rb.useGravity = false;
        rb.AddForce(vec * blownPower);

        this.IsBlownAway = true;

        // 同じ種類の敵のカウンターを減らす
        EnemyAnalysis(this.gameObject);
        Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // アイテムを落とす
        ItemDrop(true);

        Destroy(this.gameObject, 1f);
    }

    #endregion


    #region Method
    // 吹っ飛んできた敵に巻き込まれた時の処理 
    void BlownAway_Enemy(GameObject other)
    {
        Vector3 Othervec;

        //敵の種類に合わせてデータ(other.vector)の取り込み。
        if (other.tag == "enemy_ship_r")
        {
            var OtherData = other.gameObject.GetComponent<Ship_RScript>();
            Othervec = OtherData.vector;
        }
        else if (other.tag == "enemy_soldier")
        {
            var OtherData = other.gameObject.GetComponent<SoldierMove>();
            Othervec = OtherData.vector;
        }
        else if (other.tag == "enemy_tank")
        {
            var OtherData = other.gameObject.GetComponent<TankMove>();
            Othervec = OtherData.vector;
        }
        else
        {
            return;
        }

        //衝突時のベクトル計算
        var thisvec = new Vector3(Othervec.x, Othervec.y / 2, Othervec.z / 2);
        var othervec = new Vector3(Othervec.x / 2, Othervec.y / 2, Othervec.z);


        // ナビメッシュ使用の場合はここの処理をする
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        //ベクトルの反映、吹っ飛び。(自身と突っ込んできた対象)
        rb.useGravity = false;
        rb.AddForce(thisvec * blownPower);

        var Otherrb = other.GetComponent<Rigidbody>();
        Otherrb.AddForce(othervec * blownPower);

        this.IsBlownAway = true;

        // 同じ種類の敵のカウンターを減らす
        EnemyAnalysis(this.gameObject);
        Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // アイテムを落とす
        ItemDrop(false);

        Destroy(this.gameObject, 1f);
    }

    //タグから敵の種類を察知し、その対象のカウントを減らす。
    void EnemyAnalysis(GameObject other)
    {
        if (other.tag == "enemy_ship_r")
        {
            spawnArea_AirScript.enemy_count[0]--;
        }
        else if (other.tag == "enemy_soldier")
        {
            spawnArea_GroundScript.enemy_count[0]--;
        }
        else if (other.tag == "enemy_tank")
        {
            spawnArea_GroundScript.enemy_count[1]--;
        }
        else
        {
            return;
        }
    }

    // 武器・アイテムドロップ処理
    void ItemDrop(bool isAttackPlayer)
    {
        var dropPos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        GameObject weapon = null;

        if (isAttackPlayer)
        {   // プレイヤーとの接触で倒れた場合のみ武器ドロップ抽選を行う
            weapon = WeaponManager.Instance.GenerateWeapon(this.gameObject, dropPos);
        }

        if (weapon == null)
        {
            ItemManager.Instance.GenerateItem(dropPos);
        }
    }
    #endregion
}
