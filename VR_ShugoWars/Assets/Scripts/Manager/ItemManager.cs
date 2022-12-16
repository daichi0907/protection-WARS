using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
	/// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

	#region define
	/// <summary>
	/// �A�C�e��ID ( �蓮�Œǉ�����)
	/// </summary>
	public enum ItemID : int
	{
		None = -1,
		Heal,
		Exp,
	}
	#endregion

	#region serialize field
	[SerializeField] private List<GameObject> _ItemList = new List<GameObject>();
	#endregion

	#region field
	//private int DefeatePoint;   // �G��ł��������������v��
	#endregion

	#region property

	#endregion

	#region Unity function
	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		// �L�[���͂ŃA�C�e������
		//DebugFunction();
	}
	#endregion

	#region public function
	/// <summary>
	/// �A�C�e���𐶐�
	/// </summary>
	/// <param name="id">�A�C�e��ID</param>
	/// <param name="position">�����|�W�V����</param>
	/// <returns></returns>
	public GameObject GenerateItem(Vector3 position)
	{
		ItemID id = JudgeGenerateItem();
		
		if (id == ItemID.None)
		{
			return null;
		}

		var index = (int)id;
		var prefab = _ItemList[index];
		if (index < 0 || _ItemList.Count <= index)
		{
			Debug.Log("index���s���Ȓl�ł�!!!!!!!!!!!!!!!!!!!!!!");
			return null;
		}
		if (prefab == null)
		{
			Debug.Log("prefab���ݒ肳��Ă��܂���!!!!!!!!!!!!!!!");
			return null;
		}
		var obj = Instantiate(prefab, position, Quaternion.identity);
		obj.transform.SetParent(transform);
		return obj;
	}
	#endregion

	#region private function
	private ItemID JudgeGenerateItem()
    {
		ItemID id = ItemID.Heal;

		if (GameModeController.Instance.Princess.Life >= 5) id = ItemID.Exp;

		return id;
	}

	private void DebugFunction()
    {
		// �A�C�e���𐶐�
		if(Input.GetKeyDown(KeyCode.G))
        {
			Vector3 vector3 = new Vector3(
				-0.25f,
				GameModeController.Instance.StartHight,
				-0.5f);
			GenerateItem(vector3);
		}
	}
	#endregion
}
