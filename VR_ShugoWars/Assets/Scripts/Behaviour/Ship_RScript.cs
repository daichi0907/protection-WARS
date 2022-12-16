using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S�� ���� �V��

public class Ship_RScript : EnemyParent
{

    public Vector3 course; //�j�H
    private int coursep;//�j�H�|�C���g
    private float course_near = 0; //�߂��̐j�H
    private GameObject[] Point;//0�`5 1�`6
    public string[] pointname;//0�`5 1�`6
    public float ship_speed = 10;
    private Vector3 dir;

    public GameObject Rocket;//�~�T�C��
    public GameObject Fire_P;//���ˈʒu
    public float fire_StartTime; //�Z�b��ɍU���J�n
    public float reloadTime;//���b����ɔ���

    public bool movef = true;
    //private bool canJudge = true; // �t���O

    void Start()
    {
        SetUp();

        Point = new GameObject[pointname.Length];//pointname�̏������񂾔z�񐔂�GameObject�̔z�񐔂������ɂȂ�B
        for (int i = 0; i < pointname.Length; i++)
        {
            Point[i] = GameObject.Find(pointname[i]);//pointname�ɏ������񂾖��O�Ɠ���GameObject�������A�L�^����B
        }

        //�ŏ���(�X�|�[������)��ԋ߂�����|�C���g���߂�B
        for (int i = 0; i < Point.Length; i++)
        {
            if (course_near == 0 || course_near > Vector3.Distance(Point[i].transform.position, this.transform.position))
            {
                course_near = Vector3.Distance(Point[i].transform.position, this.transform.position);
                course = Point[i].transform.position;
                coursep = i;
            }
        }

        //�X�|�[�����Ă���A�Z�b��ɍU���J�n�A���b����ɔ���
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
            var look = Quaternion.LookRotation(dir); //������ύX����
            look.x = 0;
            look.z = 0;
            transform.rotation = look;

            transform.position = Vector3.MoveTowards(
                    transform.position, course, ship_speed * Time.deltaTime);//�j�H�̕����Ɉړ�
        }
    }

    void Fire_Rocket()
    {
        Instantiate(Rocket, Fire_P.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // ��ɓ����������̏����i��ɓ���������������j
        HandHit(hitcollision.gameObject);

        // �G�ɂԂ���ꂽ���̏����i�G�ɓ���������������j
        EnemyHit(hitcollision.gameObject);
    }
}
