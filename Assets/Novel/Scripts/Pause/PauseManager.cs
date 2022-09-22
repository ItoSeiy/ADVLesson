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
    /// 一時停止・再開を切り替える
    /// </summary>
    public　void PauseResume()
    {
        _pauseFlag = !_pauseFlag;
        _onPauseResume(_pauseFlag);  // これで変数に代入した関数を全て呼び出せる
        _pauseUi?.gameObject.SetActive(_pauseFlag);
    }

    /// <summary>
    /// 一時停止、再開時の関数をデリゲートに登録する関数
    /// 
    /// 一時停止を実装したいスクリプトから呼び出す
    /// 
    /// OnEnable関数で PauseManager.Instance.SetEvent(this); と記述される
    /// </summary>
    /// <param name="pauseable"></param>
    public void SetEvent(IPauseable pauseable)
    {
        _onPauseResume += pauseable.PauseResume;
    }

    /// <summary>
    /// 一時停止、再開時の関数をデリゲートから登録を解除する関数
    /// 
    /// 一時停止を実装したいスクリプトから呼び出す
    /// 
    /// OnDisable関数でPauseManager.Instance.RemoveEvent(this); と記述される
    /// </summary>
    /// <param name="pauseable"></param>
    public void RemoveEvent(IPauseable pauseable)
    {
        _onPauseResume -= pauseable.PauseResume;
    }
}
