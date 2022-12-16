using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : EnemyParent
{
    GameObject pos0;
    GameObject pos1;
    GameObject pos2;
    GameObject pos3;
    GameObject pos4;
    GameObject pos5;


    //private GameObject Target;
    //public string himename;

    public float movespeed = 0.01f;//�s����܂œ����X�s�[�h
    public float rotationSpeed = 0.01f;//�P�̕�����������]����X�s�[�h

    GameObject course;

    int posmove;
    float shottime = 0f;
    bool move = true;//true�̂Ƃ��s��܂œ���
    bool rol = false;//false�̂Ƃ��s�����������
    public static bool cannon = false;//true�ő�C������

    void Start()
    {
        SetUp();

        pos0 = GameObject.Find("pos0");
        pos1 = GameObject.Find("pos1");
        pos2 = GameObject.Find("pos2");
        pos3 = GameObject.Find("pos3");
        pos4 = GameObject.Find("pos4");
        pos5 = GameObject.Find("pos5");

        //Target = GameObject.Find(himename);
        Randomrol();
    }

    // Update is called once per frame
    void Update()
    {
        //int posmove = Random.Range(0, 6);

        if (rol == false)
        {
            //�s���悪ON�̏ꍇ�Č���
            if (course.gameObject.tag == "ON")
            {
                Randomrol();
                return;
            }
            rol = true;
            transform.LookAt(course.transform.position);
            Debug.Log("�s����" + course);
        }

        if (move == true)
        {
            Debug.Log("�^���N����");
            float distance = Vector3.Distance(transform.position, course.transform.position);

            run();
            if (distance <= 0)
            {
                move = false;
            }
        }
        else
        {
            Debug.Log("�^���N��]");
            course.tag = "ON";

            Vector3 targetPosition = Target.transform.position;

            if (transform.position.y != Target.transform.position.y)
            {
                targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            }

            Quaternion TargetRot = Quaternion.LookRotation(targetPosition - transform.position);

            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot, step * 0.1f);//�Ђ߂Ɍ������ĉ�]

            shot();
        }
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // ��ɓ����������̏����i��ɓ���������������j
        HandHit(hitcollision.gameObject);

        // �G�ɂԂ���ꂽ���̏����i�G�ɓ���������������j
        EnemyHit(hitcollision.gameObject);
    }

    void Randomrol()
    {
        posmove = Random.Range(0, 6);

        switch (posmove)
        {
            case 0:
                course = pos0;
                break;
            case 1:
                course = pos1;
                break;
            case 2:
                course = pos2;
                break;
            case 3:
                course = pos3;
                break;
            case 4:
                course = pos4;
                break;
            default:
                course = pos5;
                break;

        }
        Debug.Log("�s����T����");
    }

    void run()
    {
        //�s����Ɍ������đ���
        transform.position = Vector3.MoveTowards(transform.position, course.transform.position, movespeed * 0.01f);
    }

    void shot()
    {
        //10�b���Ƃɒe�𐶐�
        shottime = shottime + Time.deltaTime;
        if (shottime > 1f)
        {
            shottime = 0f;

            Debug.Log("����");

            cannon = true;

        }
    }
    void OnDestroyed()
    {
        Debug.Log("�j�󂳂ꂽ");

        course.tag = "OFF";
    }

}
