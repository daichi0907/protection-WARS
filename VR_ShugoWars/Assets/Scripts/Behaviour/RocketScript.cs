using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S�� ���� �V��
public class RocketScript : EnemyWeapon
{
    public GameObject Front;
    private Vector3 dir;
    //public float rotio = 10f;
    public float speed = 0.1f;
    //public float homingTime = 3f;
    public float homingdistance = 2f;//�Z���̈�̋����܂Œǔ�
    private float homingrange;
    public float lifeTime = 5f;

    private bool homingf;
    private bool movef;
    //private bool rocketf;
    public GameObject Ship;
    //private bool canJudge = true;


    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        movef = true;
        //rocketf = true;
        homingf = false;
        Invoke("LifeTime", lifeTime);//�����b��ɏ���
        //Invoke("HomingTime", homingTime);//�����b��ɒǔ���~
        Invoke("RocketTime", 1.0f);

        homingrange = Vector3.Distance(Target.transform.position, this.gameObject.transform.position) / homingdistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (homingrange > Vector3.Distance(Target.transform.position, this.gameObject.transform.position))
        {
            Debug.Log("a");
            homingf = true;
        }

        if (movef == true)
        {
            if (Target == null) return;

            if (homingf == false)
            {
                dir = new Vector3(Target.transform.position.x, Target.transform.position.y + neck, Target.transform.position.z) - transform.position;
                dir.Normalize();
                transform.rotation = Quaternion.LookRotation(dir); //������ύX����

                transform.position = Vector3.MoveTowards(
                    transform.position, new Vector3(Target.transform.position.x, Target.transform.position.y + neck, Target.transform.position.z), speed * Time.deltaTime);//�ǔ�
            }
            else
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, Front.transform.position, speed * Time.deltaTime);//�~�T�C���̌����Ă�������ɐi�s
            }
        }
    }

    void LifeTime()
    {
        Destroy(gameObject);
    }

    //void HomingTime()
    //{
    //    homingf = true;
    //}

    //void RocketTime()
    //{
    //    rocketf = false;
    //}

    private void OnCollisionEnter(Collision hitcollision)
    {
        //if (Target.name == hitcollision.gameObject.name || this.gameObject.name == hitcollision.gameObject.name
        //    || hitcollision.gameObject.name == Ship.name + "(Clone)" && rocketf == false)
        //{
        //    Destroy(gameObject);//�^�[�Q�b�g�ɓ�����Ǝ���
        //}


        //// null�Q�Ƃ������悤��
        //if (hitcollision.transform.parent == null || hitcollision.transform.parent.parent == null)
        //    return;

        //if (RightHand == hitcollision.transform.parent.parent.gameObject || LeftHand == hitcollision.transform.parent.parent.gameObject)
        //{
        //    Destroy(gameObject);//�^�[�Q�b�g�ɓ�����Ǝ���
        //}

        DamageJudgment(hitcollision);
    }
}

