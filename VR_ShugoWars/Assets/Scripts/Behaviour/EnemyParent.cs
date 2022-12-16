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
    /// ������΂���鏈���̃e���v���[�g
    /// �p����ł��̏���������
    /// </summary>
    #region Unity function
    //void Start()
    //{
    //    SetUp();
    //}

    //private void OnCollisionEnter(Collision hitcollision)
    //{
    //    // ��ɓ����������̏����i��ɓ���������������j
    //    HandHit(hitcollision.gameObject);

    //    // �G�ɂԂ���ꂽ���̏����i�G�ɓ���������������j
    //    EnemyHit(hitcollision.gameObject);
    //}
    #endregion


    #region�@Protected method
    // �Z�b�g�A�b�v����
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

    // ��ɓ����������𔻒肵�A�������s��
    protected void HandHit(GameObject other)
    {
        if (other.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            BlownAway(other.gameObject);
        }
    }

    // �G�ɐ�����΂��ꂽ�����肵�A�������s��
    protected void EnemyHit(GameObject other)
    {
        // �܂��A�ڐG�����̂��G�ł��邩�𔻒�
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

    // �v���C���[�ɔ�΂��ꂽ���̏���
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

        // ��̑��������ȏ�łȂ���ΐ�����΂Ȃ�
        if (handRC.GetSpeed() < awaySpeed)
            return;

        var vec = handRC.GetVelocity();
        vec.y = 1.0f;
        vector = vec;

        // �i�r���b�V���g�p�̏ꍇ�͂����̏���������
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        rb.useGravity = false;
        rb.AddForce(vec * blownPower);

        this.IsBlownAway = true;

        // ������ނ̓G�̃J�E���^�[�����炷
        EnemyAnalysis(this.gameObject);
        Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // �A�C�e���𗎂Ƃ�
        ItemDrop(true);

        Destroy(this.gameObject, 1f);
    }

    #endregion


    #region Method
    // �������ł����G�Ɋ������܂ꂽ���̏��� 
    void BlownAway_Enemy(GameObject other)
    {
        Vector3 Othervec;

        //�G�̎�ނɍ��킹�ăf�[�^(other.vector)�̎�荞�݁B
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

        //�Փˎ��̃x�N�g���v�Z
        var thisvec = new Vector3(Othervec.x, Othervec.y / 2, Othervec.z / 2);
        var othervec = new Vector3(Othervec.x / 2, Othervec.y / 2, Othervec.z);


        // �i�r���b�V���g�p�̏ꍇ�͂����̏���������
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        //�x�N�g���̔��f�A������сB(���g�Ɠ˂�����ł����Ώ�)
        rb.useGravity = false;
        rb.AddForce(thisvec * blownPower);

        var Otherrb = other.GetComponent<Rigidbody>();
        Otherrb.AddForce(othervec * blownPower);

        this.IsBlownAway = true;

        // ������ނ̓G�̃J�E���^�[�����炷
        EnemyAnalysis(this.gameObject);
        Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // �A�C�e���𗎂Ƃ�
        ItemDrop(false);

        Destroy(this.gameObject, 1f);
    }

    //�^�O����G�̎�ނ��@�m���A���̑Ώۂ̃J�E���g�����炷�B
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

    // ����E�A�C�e���h���b�v����
    void ItemDrop(bool isAttackPlayer)
    {
        var dropPos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        GameObject weapon = null;

        if (isAttackPlayer)
        {   // �v���C���[�Ƃ̐ڐG�œ|�ꂽ�ꍇ�̂ݕ���h���b�v���I���s��
            weapon = WeaponManager.Instance.GenerateWeapon(this.gameObject, dropPos);
        }

        if (weapon == null)
        {
            ItemManager.Instance.GenerateItem(dropPos);
        }
    }
    #endregion
}
