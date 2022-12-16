using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = StateMachine<PrincessBehaviour>.State;

/// <summary> ソースを書くときのレンプレート </summary>
public partial class PrincessBehaviour
{
    #region define
    /// <summary> 姫の状態を知らせる列挙体 </summary>
    public enum StateEnum : int
    {
        Idle,
        Grabed,
        Fall,
        Ride,
        Dead,
    }

    /// <summary> ステートマシーンのトランジション用のイベントキー </summary>
    private enum Event : int
    {
        // 掴まれた
        ToGrabed,
        // アイドル状態へ
        ToIdle,
        // 降下状態へ
        ToFall,
        // 乗り状態へ
        ToRide,
        // 死亡状態へ
        ToDead,
        // 移動状態へ
        ToMove,
    }
    #endregion

    #region serialize field

    #endregion

    #region field
    /// <summary> プレイヤー挙動管理のためのステートマシーン </summary>
    private StateMachine<PrincessBehaviour> _StateMachine;

    private StateEnum _PrincessState;   // 姫の状態
    #endregion

    #region property
    public StateEnum PrincessState { get { return _PrincessState; } }   // 姫の状態
    #endregion

    #region public function
    /// <summary>
    /// 乗り状態に遷移。
    /// 呼貼れる場所：RideAreaBehaviour
    /// </summary>
    public void ToRideState()
    {
        // 降下状態以外なら実行しない
        if (_PrincessState != StateEnum.Fall) return;

        _StateMachine.Dispatch((int)Event.ToRide);
    }

    /// <summary>
    /// 降下状態に遷移。
    /// 呼貼れる場所：RideAreaBehaviour
    /// </summary>
    public void ToFallState()
    {
        // 乗り状態以外なら実行しない
        if (_PrincessState != StateEnum.Ride) return;

        _StateMachine.Dispatch((int)Event.ToFall);
    }
    #endregion

    #region private function
    /// <summary>
    /// ステートマシンの設定を行う（Startメソッドで呼び出すよう）
    /// </summary>
    private void SetUpStateMachine()
    {
        _StateMachine = new StateMachine<PrincessBehaviour>(this);

        // （Idel→Grabed）
        _StateMachine.AddTransition<StateIdle, StateGrabed>((int)Event.ToGrabed);
        // （Ride→Grabed）
        _StateMachine.AddTransition<StateRide, StateGrabed>((int)Event.ToGrabed);
        // （Fall→Grabed）
        _StateMachine.AddTransition<StateFall, StateGrabed>((int)Event.ToGrabed);
        // （Move→Grabed）
        _StateMachine.AddTransition<StateMove, StateGrabed>((int)Event.ToGrabed);

        // （Grabed→Fall）
        _StateMachine.AddTransition<StateGrabed, StateFall>((int)Event.ToFall);
        // （Ride→Fall）
        _StateMachine.AddTransition<StateRide, StateFall>((int)Event.ToFall);

        // （Fall→Ride）
        _StateMachine.AddTransition<StateFall, StateRide>((int)Event.ToRide);
        // （Fall→Idle）
        _StateMachine.AddTransition<StateFall, StateIdle>((int)Event.ToIdle);

        // （Idle→Move）
        _StateMachine.AddTransition<StateIdle, StateMove>((int)Event.ToMove);
        // （Move→Idle）
        _StateMachine.AddTransition<StateMove, StateIdle>((int)Event.ToIdle);

        // 死亡
        _StateMachine.AddAnyTransition<StateDead>((int)Event.ToDead);

        _StateMachine.Start<StateIdle>();
        _PrincessState = StateEnum.Idle;
    }
    #endregion

    #region StateIdle class
    /// <summary>
    /// 待機
    /// </summary>
    private class StateIdle : State
    {
        public StateIdle()
        {
            _name = "Idle";
        }

        protected override void OnEnter(State prevState)
        {
            Owner._PrincessState = StateEnum.Idle;
            Owner._Animator.SetTrigger("ToIdle");
            Owner._Animator.SetFloat("MoveBlend", 0.0f);
            // 角度を調整（地面に直立する）
            Vector3 tempRotate = Owner.transform.rotation.eulerAngles;
            Vector3 princessRotate = new Vector3(0.0f, tempRotate.y, 0.0f);
            Owner.transform.rotation = Quaternion.Euler(princessRotate);
        }

        protected override void OnUpdate()
        {
            // 掴まれた場合
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
            }

            // デバッグ用
            if(Input.GetKeyDown(KeyCode.M))
            {
                Owner._StateMachine.Dispatch((int)Event.ToMove);
            }
        }

        protected override void OnExit(State nextState)
        {
            Owner._Animator.ResetTrigger("ToIdle");
        }
    }
    #endregion

    #region StateGrabed class
    /// <summary>
    /// 掴まれた
    /// </summary>
    private class StateGrabed : State
    {
        public StateGrabed()
        {
            _name = "Grabed";
        }

        protected override void OnEnter(State prevState)
        {
            Owner._PrincessState = StateEnum.Grabed;
            Owner._Animator.SetTrigger("ToGrabed");
        }

        protected override void OnUpdate()
        {
            // 離された場合
            if (!Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToFall);
            }
        }

        protected override void OnExit(State nextState)
        {
            Owner._Animator.ResetTrigger("ToGrabed");
        }
    }
    #endregion

    #region StateFall class
    /// <summary>
    /// 降下状態
    /// </summary>
    private class StateFall : State
    {
        public StateFall()
        {
            _name = "Fall";
        }

        protected override void OnEnter(State prevState)
        {
            Owner._PrincessState = StateEnum.Fall;
            Owner._Animator.SetTrigger("ToFall");
        }

        protected override void OnUpdate()
        {
            // 着地
            if(Owner._IsGrounded)
            {
                Owner._StateMachine.Dispatch((int)Event.ToIdle);
            }

            // 掴まれた場合
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
            }
        }

        protected override void OnExit(State nextState)
        {
            Owner._Animator.ResetTrigger("ToFall");
        }
    }
    #endregion

    #region StateRide class
    /// <summary>
    /// 乗り状態
    /// </summary>
    private class StateRide : State
    {
        private Vector3 princessRotate;
        private bool canRide;
        private float waitTimeFall = 0.0f;

        public StateRide()
        {
            _name = "Ride";
        }

        protected override void OnEnter(State prevState)
        {
            Owner._PrincessState = StateEnum.Ride;
            Owner._Animator.SetTrigger("ToRide");
            princessRotate = Vector3.zero;
            canRide = true;
            Owner._RideSide = Owner._RideArea._HandType;
            waitTimeFall = 0.0f;
        }

        protected override void OnUpdate()
        {
            // 掴まれた場合
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
                return;
            }

            //--- 手の角度が急になっていれば、姫を手から落下させる ---//
            if (!CanRide())
            {
                // 設定値より急なので2秒経過したら落とす
                waitTimeFall += Time.deltaTime;
                if (waitTimeFall > 2.0f) FallFromHands();

                //Debug.Log("落ちるー－－!!!");
                //Owner._Rigidbody.isKinematic = false;
                //Owner.transform.parent = null;
                //canRide = false;
                //return;
            }
            // リカバリーが出来た場合
            else if (!canRide)
            {
                canRide = true;

                waitTimeFall = 0.0f;

                // ライドエリアを再設定
                //ResetRideArea();
            }

            // 不正な値が入っていないかチェック
            if (Owner._RideArea == null || Owner._RideSide == RideAreaBehaviour.HandTypeEnum.None)
            {
                Debug.Log("ライドエリアがセットされていません！");
                return;
            }

            // 手が使用不能な場合は Fallステートに遷移
            if(Owner._PlayerDatas[(int)Owner._RideSide].Life < 1)
            {
                Debug.Log("落ちるー－－!!!");
                Owner._Rigidbody.isKinematic = false;
                Owner.transform.parent = null;
                canRide = false;
                Owner._RideArea = null;

                Owner.ToFallState();

                return;
            }

            //--- 手の中心にアンカーさせる ---//
            Owner.transform.parent = Owner._RideArea.gameObject.transform;
            Owner._Rigidbody.isKinematic = true;
            Owner.gameObject.transform.localPosition = Vector3.zero;
            // 角度を調整（地面に直立する）
            Vector3 tempRotate = Owner.gameObject.transform.localRotation.eulerAngles;
            Vector3 princessRotate = new Vector3(0.0f, tempRotate.y, 0.0f);
            Owner.gameObject.transform.localRotation = Quaternion.Euler(princessRotate);
        }

        protected override void OnExit(State nextState)
        {
            Owner._Animator.ResetTrigger("ToRide");

            // 姫の、手の中心へのアンカーを解除
            Owner._Rigidbody.isKinematic = false;
            Owner.transform.parent = null;

            // 姫のライドエリア設定を解除
            Owner._RideArea = null;

            Owner._RideSide = RideAreaBehaviour.HandTypeEnum.None;
        }

        /// <summary>
        /// 姫の角度から、まだ手に乗っていられるかどうか判定する。
        /// </summary>
        /// <returns></returns>
        private bool CanRide()
        {
            float limitRotate = 45.0f;

            princessRotate = Owner.transform.eulerAngles;
            if (princessRotate.x > 180.0f) princessRotate += new Vector3(-360.0f, 0.0f, 0.0f);
            if (princessRotate.z > 180.0f) princessRotate += new Vector3(0.0f, 0.0f, -360.0f);
            //Debug.Log(princessRotate);

            float princessRX = Mathf.Abs(princessRotate.x);
            float princessRZ = Mathf.Abs(princessRotate.z);

            if (princessRX > limitRotate || princessRZ > limitRotate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 手から落ちる処理
        /// </summary>
        private void FallFromHands()
        {
            Debug.Log("落ちるー－－!!!");
            Owner._Rigidbody.isKinematic = false;
            Owner.transform.parent = null;
            canRide = false;
            // 姫のライドエリア設定を解除
            Owner._RideArea = null;
            Owner._RideSide = RideAreaBehaviour.HandTypeEnum.None;

            // 強制的に落とす
            Owner._Rigidbody.AddForce(Owner.transform.up * 10.0f);
        }

        private void ResetRideArea()
        {
            if (Owner._RideSide == RideAreaBehaviour.HandTypeEnum.Left)
            {
                var leftHandArea = GameObject.Find("LeftRideArea").GetComponent<RideAreaBehaviour>();
                Owner.SetRideArea(leftHandArea);
            }
            else if (Owner._RideSide == RideAreaBehaviour.HandTypeEnum.Right)
            {
                var rightHandArea = GameObject.Find("RightRideArea").GetComponent<RideAreaBehaviour>();
                Owner.SetRideArea(rightHandArea);
            }
        }
    }
    #endregion

    #region StateMove class
    /// <summary>
    /// 移動
    /// </summary>
    private class StateMove : State
    {
        public StateMove()
        {
            _name = "Move";
        }

        protected override void OnEnter(State prevState)
        {
            Owner._Animator.SetFloat("MoveBlend", 1.0f);
        }

        protected override void OnUpdate()
        {
            // 掴まれた場合
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
                return;
            }

            // デバッグ用
            // 停止（Idle）用
            if (Input.GetKeyDown(KeyCode.M))
            {
                Owner._StateMachine.Dispatch((int)Event.ToIdle);
                return;
            }

            //Owner._Rigidbody.AddForce(Owner.transform.forward * 5.0f, ForceMode.Force);
            Owner._Rigidbody.velocity = Owner.transform.forward * 0.2f;
        }

        protected override void OnExit(State nextState)
        {
            Owner._Rigidbody.velocity = Vector3.zero;
        }
    }
    #endregion

    #region StateDead class
    /// <summary>
    /// 死亡
    /// </summary>
    private class StateDead : State
    {
        public StateDead()
        {
            _name = "Dead";
        }

        protected override void OnEnter(State prevState)
        {
            Owner._PrincessState = StateEnum.Dead;
            Owner._Animator.SetTrigger("ToDead");
            Owner._IsDead = true;

            Owner._Rigidbody.isKinematic = false;
            Owner.transform.parent = null;
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnExit(State nextState)
        {
            
        }
    }
    #endregion
}
