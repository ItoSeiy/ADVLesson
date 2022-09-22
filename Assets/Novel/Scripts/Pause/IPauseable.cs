using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ꎞ��~�@�\���\�ɂ���C���^�[�t�F�[�X
/// 
/// �ꎞ��~���\�ɂ���菇
/// 
/// 1,���̃C���^�t�F�C�X���p������
/// 2,PauseResume�֐��Ɉꎞ��~�A�ĊJ��̓������L�q����
/// ������ bool isPause ��p����if���ŕ��򂳂��ď������s��
/// 
/// 3,OnEnable�֐��� PauseManager.Instance.SetEvent(this); �ƋL�q
/// 4,OnDisable�֐���PauseManager.Instance.RemoveEvent(this); �ƋL�q
/// </summary>
public interface IPauseable
{
    void PauseResume(bool isPause);
}
