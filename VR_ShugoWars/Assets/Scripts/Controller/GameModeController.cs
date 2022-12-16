/// <summary> 松島 </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region define
/// <summary>
/// GameModeの状態
/// </summary>
public enum GameModeStateEnum : int
{
	/// <summary> 何もない状態 </summary>
	None,
	/// <summary> ゲーム開始時にカウントダウン </summary>
	CountDown,
	/// <summary> ハンドのセットアップ </summary>
	HandsSetUp,
	/// <summary> ゲーム中 </summary>
	Play,
	/// <summary> クリア </summary>
	Clear,
	/// <summary> ゲームオーバー </summary>
	GameOver,
}
#endregion

public class GameModeController : SingletonMonoBehaviour<GameModeController>
{
	#region define
	/// <summary>
	/// GameModeの状態
	/// </summary>
	public enum DebugModeEnum : int
	{
		/// <summary> 通常プレイ </summary>
		None,
		/// <summary> 姫無敵 </summary>
		Invincible,
		/// <summary> 姫は1ライフは残る </summary>
		Safety,
	}
	#endregion

	#region serialize field
	/// <summary> 姫 </summary>
	[SerializeField] private PrincessBehaviour _Princess = null;

	/// <summary> ハンドセットアップステートを管理するクラス </summary>
	[SerializeField] private HandsSetUpController _HandsSetUp = null;
	#endregion

	#region field
	/// <summary> GameModeの現在の状態 </summary>
	private GameModeStateEnum _State;

	private float _Time = 5.0f;

	/// <summary> デバッグモード </summary>
	private DebugModeEnum _DebugMode = DebugModeEnum.None;
	#endregion

	#region property
	/// <summary> Playerを他のプログラムから使うときにここからアクセス </summary>
	public PrincessBehaviour Princess { get { return _Princess; } }

	/// <summary> 現在のゲームモードを取得 </summary>
	public GameModeStateEnum State { get { return _State; } }

    /// <summary> タイムを取得 </summary>
    public float GameTime { get { return _Time; } }

    /// <summary> デバッグモード </summary>
    public DebugModeEnum DebugMode { get { return _DebugMode; } }

    /// <summary> オブジェクトが机の上に着地しているときの高さ </summary>
    public float StartHight { get; set; }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    private void Start()
	{
		ChangeState(GameModeStateEnum.CountDown);
	}

	// Update is called once per frame
	private void Update()
	{
		UpdateState();
		UpdateDebugMode();
	}
	#endregion

	#region public function
	public void Debug_ChangeDebugMode()
    {
		var n = ((int)_DebugMode) + 1;
		if (n > (int)DebugModeEnum.Safety) n = 0;
		_DebugMode = ((DebugModeEnum)n);
	}
	#endregion

	#region private function
	private void UpdateDebugMode()
    {
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
        {
			var n = ((int)_DebugMode) + 1;
			if (n > (int)DebugModeEnum.Safety) n = 0;
			_DebugMode = ((DebugModeEnum)n);
        }
    }

	private void Restart()
	{
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
		{
			// 現在のSceneを取得
			Scene loadScene = SceneManager.GetActiveScene();
			// 現在のシーンを再読み込みする
			SceneManager.LoadScene(loadScene.name);
		}
	}

	/// <summary>
	/// 状態の変更
	/// </summary>
	/// <param name="next">次の状態</param>
	private void ChangeState(GameModeStateEnum next)
	{
		// 以前の状態を保持
		var prev = _State;
		// 次の状態に変更する
		_State = next;

		// ログを出す
		Debug.Log("ChangeState " + prev + "-> " + next); 

		switch (_State)
		{
			case GameModeStateEnum.None:
				{
				}
				break;
			case GameModeStateEnum.CountDown:
				{
					_DebugMode = DebugModeEnum.Invincible;
					_HandsSetUp.gameObject.SetActive(false);
				}
				break;
			case GameModeStateEnum.HandsSetUp:
                {
					_HandsSetUp.gameObject.SetActive(true);
				}
				break;
			case GameModeStateEnum.Play:
				{
					_HandsSetUp.gameObject.SetActive(false);
					_DebugMode = DebugModeEnum.None;
				}
				break;
			case GameModeStateEnum.Clear:
				{
					// クリアシーンをロードする?
					// SceneManager.LoadScene("ClearScene");
					//Debug.Log("Go to ClearScene!");
				}
				break;
			case GameModeStateEnum.GameOver:
				{
					// ゲームオーバーシーンをロードする?
					// SceneManager.LoadScene("GameoverScene");
					//Debug.Log("Go to GameoverScene!");
					//_Canvas.UI_ToDead();
				}
				break;
		}

	}

	/// <summary>
	/// 状態毎の毎フレーム呼ばれる処理
	/// </summary>
	private void UpdateState()
	{
		switch (_State)
		{
			case GameModeStateEnum.None:
				{
				}
				break;
			case GameModeStateEnum.CountDown:
				{
					_Time -= Time.deltaTime;

					if (_Time > 0.0001f) return;

					// ここでカウントダウンの処理を書く。演出を入れるのもアリ。
					_Time = 0.0f;
					ChangeState(GameModeStateEnum.HandsSetUp);
				}
				break;
			case GameModeStateEnum.HandsSetUp:
				{
					if (_HandsSetUp.IsFnish) ChangeState(GameModeStateEnum.Play);
				}
				break;
			case GameModeStateEnum.Play:
				{
					// プレイヤーが死んだら GameOver 状態へ (関数 ChangeState を使う)
					// (ヒント プレイヤーへは _Player で アクセス可能)
					if (_Princess.IsDead) ChangeState(GameModeStateEnum.GameOver);

					// 経過時間表示を更新
					_Time += Time.deltaTime;
				}
				break;
			case GameModeStateEnum.Clear:
				{
				}
				break;
			case GameModeStateEnum.GameOver:
				{
					Restart();
				}
				break;
		}
	}
	#endregion
}
