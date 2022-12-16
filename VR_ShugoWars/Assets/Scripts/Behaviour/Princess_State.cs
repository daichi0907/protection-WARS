using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = StateMachine<PrincessBehaviour>.State;

/// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>
public partial class PrincessBehaviour
{
    #region define
    /// <summary> �P�̏�Ԃ�m�点��񋓑� </summary>
    public enum StateEnum : int
    {
        Idle,
        Grabed,
        Fall,
        Ride,
        Dead,
    }

    /// <summary> �X�e�[�g�}�V�[���̃g�����W�V�����p�̃C�x���g�L�[ </summary>
    private enum Event : int
    {
        // �͂܂ꂽ
        ToGrabed,
        // �A�C�h����Ԃ�
        ToIdle,
        // �~����Ԃ�
        ToFall,
        // ����Ԃ�
        ToRide,
        // ���S��Ԃ�
        ToDead,
        // �ړ���Ԃ�
        ToMove,
    }
    #endregion

    #region serialize field

    #endregion

    #region field
    /// <summary> �v���C���[�����Ǘ��̂��߂̃X�e�[�g�}�V�[�� </summary>
    private StateMachine<PrincessBehaviour> _StateMachine;

    private StateEnum _PrincessState;   // �P�̏��
    #endregion

    #region property
    public StateEnum PrincessState { get { return _PrincessState; } }   // �P�̏��
    #endregion

    #region public function
    /// <summary>
    /// ����ԂɑJ�ځB
    /// �ē\���ꏊ�FRideAreaBehaviour
    /// </summary>
    public void ToRideState()
    {
        // �~����ԈȊO�Ȃ���s���Ȃ�
        if (_PrincessState != StateEnum.Fall) return;

        _StateMachine.Dispatch((int)Event.ToRide);
    }

    /// <summary>
    /// �~����ԂɑJ�ځB
    /// �ē\���ꏊ�FRideAreaBehaviour
    /// </summary>
    public void ToFallState()
    {
        // ����ԈȊO�Ȃ���s���Ȃ�
        if (_PrincessState != StateEnum.Ride) return;

        _StateMachine.Dispatch((int)Event.ToFall);
    }
    #endregion

    #region private function
    /// <summary>
    /// �X�e�[�g�}�V���̐ݒ���s���iStart���\�b�h�ŌĂяo���悤�j
    /// </summary>
    private void SetUpStateMachine()
    {
        _StateMachine = new StateMachine<PrincessBehaviour>(this);

        // �iIdel��Grabed�j
        _StateMachine.AddTransition<StateIdle, StateGrabed>((int)Event.ToGrabed);
        // �iRide��Grabed�j
        _StateMachine.AddTransition<StateRide, StateGrabed>((int)Event.ToGrabed);
        // �iFall��Grabed�j
        _StateMachine.AddTransition<StateFall, StateGrabed>((int)Event.ToGrabed);
        // �iMove��Grabed�j
        _StateMachine.AddTransition<StateMove, StateGrabed>((int)Event.ToGrabed);

        // �iGrabed��Fall�j
        _StateMachine.AddTransition<StateGrabed, StateFall>((int)Event.ToFall);
        // �iRide��Fall�j
        _StateMachine.AddTransition<StateRide, StateFall>((int)Event.ToFall);

        // �iFall��Ride�j
        _StateMachine.AddTransition<StateFall, StateRide>((int)Event.ToRide);
        // �iFall��Idle�j
        _StateMachine.AddTransition<StateFall, StateIdle>((int)Event.ToIdle);

        // �iIdle��Move�j
        _StateMachine.AddTransition<StateIdle, StateMove>((int)Event.ToMove);
        // �iMove��Idle�j
        _StateMachine.AddTransition<StateMove, StateIdle>((int)Event.ToIdle);

        // ���S
        _StateMachine.AddAnyTransition<StateDead>((int)Event.ToDead);

        _StateMachine.Start<StateIdle>();
        _PrincessState = StateEnum.Idle;
    }
    #endregion

    #region StateIdle class
    /// <summary>
    /// �ҋ@
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
            // �p�x�𒲐��i�n�ʂɒ�������j
            Vector3 tempRotate = Owner.transform.rotation.eulerAngles;
            Vector3 princessRotate = new Vector3(0.0f, tempRotate.y, 0.0f);
            Owner.transform.rotation = Quaternion.Euler(princessRotate);
        }

        protected override void OnUpdate()
        {
            // �͂܂ꂽ�ꍇ
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
            }

            // �f�o�b�O�p
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
    /// �͂܂ꂽ
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
            // �����ꂽ�ꍇ
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
    /// �~�����
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
            // ���n
            if(Owner._IsGrounded)
            {
                Owner._StateMachine.Dispatch((int)Event.ToIdle);
            }

            // �͂܂ꂽ�ꍇ
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
    /// �����
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
            // �͂܂ꂽ�ꍇ
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
                return;
            }

            //--- ��̊p�x���}�ɂȂ��Ă���΁A�P���肩�痎�������� ---//
            if (!CanRide())
            {
                // �ݒ�l���}�Ȃ̂�2�b�o�߂����痎�Ƃ�
                waitTimeFall += Time.deltaTime;
                if (waitTimeFall > 2.0f) FallFromHands();

                //Debug.Log("������[�|�|!!!");
                //Owner._Rigidbody.isKinematic = false;
                //Owner.transform.parent = null;
                //canRide = false;
                //return;
            }
            // ���J�o���[���o�����ꍇ
            else if (!canRide)
            {
                canRide = true;

                waitTimeFall = 0.0f;

                // ���C�h�G���A���Đݒ�
                //ResetRideArea();
            }

            // �s���Ȓl�������Ă��Ȃ����`�F�b�N
            if (Owner._RideArea == null || Owner._RideSide == RideAreaBehaviour.HandTypeEnum.None)
            {
                Debug.Log("���C�h�G���A���Z�b�g����Ă��܂���I");
                return;
            }

            // �肪�g�p�s�\�ȏꍇ�� Fall�X�e�[�g�ɑJ��
            if(Owner._PlayerDatas[(int)Owner._RideSide].Life < 1)
            {
                Debug.Log("������[�|�|!!!");
                Owner._Rigidbody.isKinematic = false;
                Owner.transform.parent = null;
                canRide = false;
                Owner._RideArea = null;

                Owner.ToFallState();

                return;
            }

            //--- ��̒��S�ɃA���J�[������ ---//
            Owner.transform.parent = Owner._RideArea.gameObject.transform;
            Owner._Rigidbody.isKinematic = true;
            Owner.gameObject.transform.localPosition = Vector3.zero;
            // �p�x�𒲐��i�n�ʂɒ�������j
            Vector3 tempRotate = Owner.gameObject.transform.localRotation.eulerAngles;
            Vector3 princessRotate = new Vector3(0.0f, tempRotate.y, 0.0f);
            Owner.gameObject.transform.localRotation = Quaternion.Euler(princessRotate);
        }

        protected override void OnExit(State nextState)
        {
            Owner._Animator.ResetTrigger("ToRide");

            // �P�́A��̒��S�ւ̃A���J�[������
            Owner._Rigidbody.isKinematic = false;
            Owner.transform.parent = null;

            // �P�̃��C�h�G���A�ݒ������
            Owner._RideArea = null;

            Owner._RideSide = RideAreaBehaviour.HandTypeEnum.None;
        }

        /// <summary>
        /// �P�̊p�x����A�܂���ɏ���Ă����邩�ǂ������肷��B
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
        /// �肩�痎���鏈��
        /// </summary>
        private void FallFromHands()
        {
            Debug.Log("������[�|�|!!!");
            Owner._Rigidbody.isKinematic = false;
            Owner.transform.parent = null;
            canRide = false;
            // �P�̃��C�h�G���A�ݒ������
            Owner._RideArea = null;
            Owner._RideSide = RideAreaBehaviour.HandTypeEnum.None;

            // �����I�ɗ��Ƃ�
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
    /// �ړ�
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
            // �͂܂ꂽ�ꍇ
            if (Owner.IsGrabed)
            {
                Owner._StateMachine.Dispatch((int)Event.ToGrabed);
                return;
            }

            // �f�o�b�O�p
            // ��~�iIdle�j�p
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
    /// ���S
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
