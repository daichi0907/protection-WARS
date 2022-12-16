using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemBehaviour : MonoBehaviour, IGrabableComponent
{
    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    /// <summary> ���g�ɂɃA�^�b�`���ꂽ�R���|�[�l���g���擾���邽�߂̕ϐ��Q </summary>
    private Transform _GrabedPoint;

    private int _HealPoint = 1;
    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -1) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // �P�łȂ���Έȉ��̏����͍s��Ȃ��B
        if (princess == null) return;

        // �񕜂�����
        princess.HealLifePoint(_HealPoint);

        // �G�t�F�N�g�𔭐�������
        EffectManager.Instance.Play(EffectManager.EffectID.Heal, transform.position);

        // �A�C�e������
        Destroy(this.gameObject);
    }
    #endregion

    #region public function
    /// <summary>
    /// �񕜃|�C���g��ύX����
    /// �h���b�v���̓G�N���X����A�񕜗ʂ𒲐߂ł���悤�ɂ���B
    /// </summary>
    /// <param name="point"></param>
    public void SetHealPoint(int point)
    {
        _HealPoint = point;
    }
    #endregion

    #region private function
    
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
}
