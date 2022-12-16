/// <summary> ���� </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PrincessBehaviour
    : MonoBehaviour, IGrabableComponent, IBattleComponent, IPlayerGetItemComponent
{
    /// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

    #region define
    #endregion

    #region serialize field

    #endregion

    #region field
    /// <summary> �v���C���[�ɃA�^�b�`���ꂽ�R���|�[�l���g���擾���邽�߂̕ϐ��Q </summary>
    private Animator _Animator;
    private Transform _GrabedPoint;
    private Rigidbody _Rigidbody;

    /// <summary> �v���C���[�̃X�e�[�^�X </summary>
    private int _Life = 5;
    private int _Exp = 0;
    private int _Level = 1;
    private int _MaxLevel = 5;
    private WeaponType _HaveWeapon = WeaponType.None;

    private bool _IsGrounded;   // ���ɐڒn���Ă��邩�ǂ���
    private bool _IsDead = false;   // �P�����񂾂��ǂ���

    private RideAreaBehaviour _RideArea = null;
    private RideAreaBehaviour.HandTypeEnum _RideSide = RideAreaBehaviour.HandTypeEnum.None;

    private StateEnum _CurrentState;   //�f�o�b�O�p�B�P�̌��݂̃X�e�[�g�B
    private StateEnum _PrevState;   //�f�o�b�O�p�B�P�̈�O�̃X�e�[�g�B

    private PlayerData[] _PlayerDatas = new PlayerData[2];

    // ���x���A�b�v�܂łɕK�v�ȃI�[�u�̐�
    int[] _LevelUpLimits = new int[] { 10, 20, 30, 40 };
    #endregion

    #region property
    public bool IsDead { get { return _IsDead; } }   // �P�����񂾂��ǂ���
    public bool IsGrounded
    {
        get { return _IsGrounded; }
        set { _IsGrounded = value; }
    }
    public RideAreaBehaviour.HandTypeEnum RideSide { get { return _RideSide; } }

    public int Level { get { return _Level; } }   // ���N���X����擾�ł���悤�ɂ���B

    public int Exp { get { return _Exp; } }   // ���N���X����擾�ł���悤�ɂ���B
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
        _Rigidbody = GetComponent<Rigidbody>();

        _PlayerDatas[0]  = GameObject.Find("LeftOVRHandPrefab").GetComponent<PlayerData>();
        _PlayerDatas[1] = GameObject.Find("RightOVRHandPrefab").GetComponent<PlayerData>();

        SetUpStateMachine();
    }

    void FixedUpdate()
    {
        //_Rigidbody.AddForce(transform.forward * 5.0f, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        _CurrentState = _PrincessState;
        // �f�o�b�O�p�֐�
        DebugFunction();

        // �X�e�[�g�}�V���̍X�V
        _StateMachine.Update();

        // �ЂƂO�̃X�e�[�g��ۑ�
        _PrevState = _CurrentState;
    }
    #endregion

    #region public function
    /// <summary>
    /// �P�̈ʒu�����Z�b�g����
    /// �Ăяo����FDeskBehaviour.cs
    /// </summary>
    /// <param name="respawnPoint">���X�|�[���ʒu</param>
    public void Respawn(Vector3 respawnPoint)
    {
        respawnPoint += new Vector3(0.0f, 0.05f, 0.0f);
        transform.position = respawnPoint;
    }

    /// <summary>
    /// ���ݏ���Ă��郉�C�h�G���A�̃r�w�C�r�A�N���X���Z�b�g����
    /// </summary>
    public void SetRideArea(RideAreaBehaviour rideArea)
    {
        _RideArea = rideArea;
    }

    /// <summary>
    /// �����ɂ��ڂ��Ă��Ȃ���΁Anull���Z�b�g
    /// </summary>
    public void ResetRideArea()
    {
        _RideArea = null;
    }
    #endregion

    #region private function
    /// <summary> �f�o�b�O�p�֐� </summary>
    private void DebugFunction()
    {
        // �����p�FBackspace��5��Ŏ��S��Ԃɋ����I�ɑJ��
        if (Input.GetKeyDown(KeyCode.Backspace)) ReceiveDamage(1);
        if (_Life <= 0 && _PrincessState != StateEnum.Dead) _StateMachine.Dispatch((int)Event.ToDead);

        // �A�C�e���𐶐�
        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 vector3 = new Vector3(
                -0.25f,
                GameModeController.Instance.StartHight,
                -0.5f);
            ItemManager.Instance.GenerateItem(vector3);
        }

        // �P�̃X�e�[�g�̕ω����f�o�b�O���O�ɕ\��
        if (_PrevState == _CurrentState) return;
        Debug.Log("ChangeState " + _PrevState + " -> " + _CurrentState);
    }

    /// <summary>
    /// �O�G����̃_���[�W���󂯂�
    /// �Ăяo����FApplyDamage�֐��i�C���^�[�t�F�C�X����p���j
    /// </summary>
    /// <param name="damage"></param>
    private void ReceiveDamage(int damage)
    {
        if (_Life <= 0) return;

        // �f�o�b�O���[�h�F�P�����G�@�Ȃ炷�����^�[��
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Invincible)
            return;

        //  �f�o�b�O���[�h�FSafety�@�Ȃ�ALife=1�Ȃ烊�^�[��
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Safety
            && _Life == 1)
            return;

        _Life -= damage;

        if (_Life < 0) _Life = 0;
    }

    /// <summary>
    /// ���܂����o���l���烌�x�������肷��
    /// �o���l�𓾂邽�тɌĂ΂��
    /// </summary>
    private void UpdateLevel()
    {
        int index = _Level - 1;

        // ���x���A�b�v�ɕK�v�Ȍo���l�����܂��Ă�����
        if(_Exp >= _LevelUpLimits[index])
        {
            _Level++;   // ���x���A�b�v
            _Exp -= _LevelUpLimits[index];   // �g�����o���l�͏���
        }
    }
    #endregion


    /// <summary> �E�܂ގw�悩��A�E�܂܂ꂽ�I�u�W�F�N�g�ɃA�N�Z�X���邽�߂̃C���^�[�t�F�[�X </summary>
    #region IGrabableObject
    /// <summary> �E�܂܂�Ă��邩�ǂ��� </summary>
    public bool IsGrabed { get; set; }

    /// <summary> �͂ލ��W��n�� </summary>
    public Transform Get_GrabedPoint()
    {
        return _GrabedPoint;
    }
    #endregion


    /// <summary> �_���[�W��^���铙�́A�o�g���n�̋@�\���W�߂��C���^�[�t�F�[�X�B </summary>
    #region IBattleObject

    /// <summary>
    /// �c�胉�C�t
    /// </summary>
    public int Life
    {
        get { return _Life; }
    }

    /// <summary>
    /// �_���[�W��^����֐��B
    /// �_���[�W��^���鑤�̃I�u�W�F�N�g���A���̊֐����Ăяo���B
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(int damage)
    {
        ReceiveDamage(damage);
    }
    #endregion


    /// <summary> �A�C�e�����Q�b�g�����ۂɌĂԊ֐��Q </summary>
    #region IPlayerGetItem

    /// <summary>
    /// �񕜃A�C�e�����擾�����ꍇ
    /// </summary>
    /// <param name="healPoint"></param>
    public void HealLifePoint(int healPoint)
    {
        // ���C�t��Max�Ȃ�ȉ��̏����͍s��Ȃ��B
        if (_Life >= 5) return;

        _Life += healPoint;
        if (_Life > 5) _Life = 5;   // 5����ɂ͂��Ȃ�
    }

    /// <summary>
    /// �o���l�A�C�e�����擾�����ꍇ
    /// </summary>
    /// <param name="addExpPoint"></param>
    public void AddExp(int addExpPoint)
    {
        // ������x���Ȃ�ȉ��̏����͍s��Ȃ��B
        if (_Level >= _MaxLevel) return;

        _Exp += addExpPoint;
        UpdateLevel();
    }

    /// <summary>
    /// ����A�C�e�����擾�����ꍇ
    /// �擾����𑕔�
    /// </summary>
    /// <param name="weaponType">�擾��������̎��</param>
    public void EquipWeapon(WeaponType weaponType)
    {
        //switch(weaponType)
        //{
        //    case WeaponType.Gun:
        //        {
        //            _HaveWeapon = WeaponType.Gun;
        //        }
        //        break;
        //    case WeaponType.Launcher:
        //        {
        //            _HaveWeapon = WeaponType.Launcher;
        //        }
        //        break;
        //}

        _HaveWeapon = weaponType;
        Debug.Log(_HaveWeapon);
    }
    #endregion
}
