using System;
using UnityEngine;

public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
    public event Action<bool> _onPauseResume = default;

    [SerializeField]
    RectTransform _pauseUi;

    [SerializeField]
    private KeyCode _pauseKey = KeyCode.Escape;

    private bool _pauseFlag = false;

    private void Update()
    {
        InputPauseResume();
    }

    public void InputPauseResume()
    {
        if(Input.GetKeyDown(_pauseKey))
        {
            PauseResume();
        }
    }

    /// <summary>
    /// �ꎞ��~�E�ĊJ��؂�ւ���
    /// </summary>
    public�@void PauseResume()
    {
        _pauseFlag = !_pauseFlag;
        _onPauseResume(_pauseFlag);  // ����ŕϐ��ɑ�������֐���S�ČĂяo����
        _pauseUi?.gameObject.SetActive(_pauseFlag);
    }

    /// <summary>
    /// �ꎞ��~�A�ĊJ���̊֐����f���Q�[�g�ɓo�^����֐�
    /// 
    /// �ꎞ��~�������������X�N���v�g����Ăяo��
    /// 
    /// OnEnable�֐��� PauseManager.Instance.SetEvent(this); �ƋL�q�����
    /// </summary>
    /// <param name="pauseable"></param>
    public void SetEvent(IPauseable pauseable)
    {
        _onPauseResume += pauseable.PauseResume;
    }

    /// <summary>
    /// �ꎞ��~�A�ĊJ���̊֐����f���Q�[�g����o�^����������֐�
    /// 
    /// �ꎞ��~�������������X�N���v�g����Ăяo��
    /// 
    /// OnDisable�֐���PauseManager.Instance.RemoveEvent(this); �ƋL�q�����
    /// </summary>
    /// <param name="pauseable"></param>
    public void RemoveEvent(IPauseable pauseable)
    {
        _onPauseResume -= pauseable.PauseResume;
    }
}
