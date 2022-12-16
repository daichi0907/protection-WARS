/// <summary> 松島 </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PrincessBehaviour
    : MonoBehaviour, IGrabableComponent, IBattleComponent, IPlayerGetItemComponent
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define
    #endregion

    #region serialize field

    #endregion

    #region field
    /// <summary> プレイヤーにアタッチされたコンポーネントを取得するための変数群 </summary>
    private Animator _Animator;
    private Transform _GrabedPoint;
    private Rigidbody _Rigidbody;

    /// <summary> プレイヤーのステータス </summary>
    private int _Life = 5;
    private int _Exp = 0;
    private int _Level = 1;
    private int _MaxLevel = 5;
    private WeaponType _HaveWeapon = WeaponType.None;

    private bool _IsGrounded;   // 机に接地しているかどうか
    private bool _IsDead = false;   // 姫が死んだかどうか

    private RideAreaBehaviour _RideArea = null;
    private RideAreaBehaviour.HandTypeEnum _RideSide = RideAreaBehaviour.HandTypeEnum.None;

    private StateEnum _CurrentState;   //デバッグ用。姫の現在のステート。
    private StateEnum _PrevState;   //デバッグ用。姫の一つ前のステート。

    private PlayerData[] _PlayerDatas = new PlayerData[2];

    // レベルアップまでに必要なオーブの数
    int[] _LevelUpLimits = new int[] { 10, 20, 30, 40 };
    #endregion

    #region property
    public bool IsDead { get { return _IsDead; } }   // 姫が死んだかどうか
    public bool IsGrounded
    {
        get { return _IsGrounded; }
        set { _IsGrounded = value; }
    }
    public RideAreaBehaviour.HandTypeEnum RideSide { get { return _RideSide; } }

    public int Level { get { return _Level; } }   // 他クラスから取得できるようにする。

    public int Exp { get { return _Exp; } }   // 他クラスから取得できるようにする。
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
        // デバッグ用関数
        DebugFunction();

        // ステートマシンの更新
        _StateMachine.Update();

        // ひとつ前のステートを保存
        _PrevState = _CurrentState;
    }
    #endregion

    #region public function
    /// <summary>
    /// 姫の位置をリセットする
    /// 呼び出し先：DeskBehaviour.cs
    /// </summary>
    /// <param name="respawnPoint">リスポーン位置</param>
    public void Respawn(Vector3 respawnPoint)
    {
        respawnPoint += new Vector3(0.0f, 0.05f, 0.0f);
        transform.position = respawnPoint;
    }

    /// <summary>
    /// 現在乗っているライドエリアのビヘイビアクラスをセットする
    /// </summary>
    public void SetRideArea(RideAreaBehaviour rideArea)
    {
        _RideArea = rideArea;
    }

    /// <summary>
    /// 何処にも載っていなければ、nullをセット
    /// </summary>
    public void ResetRideArea()
    {
        _RideArea = null;
    }
    #endregion

    #region private function
    /// <summary> デバッグ用関数 </summary>
    private void DebugFunction()
    {
        // 実験用：Backspaceを5回で死亡状態に強制的に遷移
        if (Input.GetKeyDown(KeyCode.Backspace)) ReceiveDamage(1);
        if (_Life <= 0 && _PrincessState != StateEnum.Dead) _StateMachine.Dispatch((int)Event.ToDead);

        // アイテムを生成
        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 vector3 = new Vector3(
                -0.25f,
                GameModeController.Instance.StartHight,
                -0.5f);
            ItemManager.Instance.GenerateItem(vector3);
        }

        // 姫のステートの変化をデバッグログに表示
        if (_PrevState == _CurrentState) return;
        Debug.Log("ChangeState " + _PrevState + " -> " + _CurrentState);
    }

    /// <summary>
    /// 外敵からのダメージを受ける
    /// 呼び出し先：ApplyDamage関数（インターフェイスから継承）
    /// </summary>
    /// <param name="damage"></param>
    private void ReceiveDamage(int damage)
    {
        if (_Life <= 0) return;

        // デバッグモード：姫が無敵　ならすぐリターン
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Invincible)
            return;

        //  デバッグモード：Safety　なら、Life=1ならリターン
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Safety
            && _Life == 1)
            return;

        _Life -= damage;

        if (_Life < 0) _Life = 0;
    }

    /// <summary>
    /// 溜まった経験値からレベルを決定する
    /// 経験値を得るたびに呼ばれる
    /// </summary>
    private void UpdateLevel()
    {
        int index = _Level - 1;

        // レベルアップに必要な経験値が溜まっていたら
        if(_Exp >= _LevelUpLimits[index])
        {
            _Level++;   // レベルアップ
            _Exp -= _LevelUpLimits[index];   // 使った経験値は消滅
        }
    }
    #endregion


    /// <summary> 摘まむ指先から、摘ままれたオブジェクトにアクセスするためのインターフェース </summary>
    #region IGrabableObject
    /// <summary> 摘ままれているかどうか </summary>
    public bool IsGrabed { get; set; }

    /// <summary> 掴む座標を渡す </summary>
    public Transform Get_GrabedPoint()
    {
        return _GrabedPoint;
    }
    #endregion


    /// <summary> ダメージを与える等の、バトル系の機能を集めたインターフェース。 </summary>
    #region IBattleObject

    /// <summary>
    /// 残りライフ
    /// </summary>
    public int Life
    {
        get { return _Life; }
    }

    /// <summary>
    /// ダメージを与える関数。
    /// ダメージを与える側のオブジェクトが、この関数を呼び出す。
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(int damage)
    {
        ReceiveDamage(damage);
    }
    #endregion


    /// <summary> アイテムをゲットした際に呼ぶ関数群 </summary>
    #region IPlayerGetItem

    /// <summary>
    /// 回復アイテムを取得した場合
    /// </summary>
    /// <param name="healPoint"></param>
    public void HealLifePoint(int healPoint)
    {
        // ライフがMaxなら以下の処理は行わない。
        if (_Life >= 5) return;

        _Life += healPoint;
        if (_Life > 5) _Life = 5;   // 5より上にはしない
    }

    /// <summary>
    /// 経験値アイテムを取得した場合
    /// </summary>
    /// <param name="addExpPoint"></param>
    public void AddExp(int addExpPoint)
    {
        // 上限レベルなら以下の処理は行わない。
        if (_Level >= _MaxLevel) return;

        _Exp += addExpPoint;
        UpdateLevel();
    }

    /// <summary>
    /// 武器アイテムを取得した場合
    /// 取得武器を装備
    /// </summary>
    /// <param name="weaponType">取得した武器の種類</param>
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
