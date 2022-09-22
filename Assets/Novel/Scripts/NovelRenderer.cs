using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NovelRenderer : MonoBehaviour, IPauseable
{
    public bool IsNovelFinish { get; private set; }

    [SerializeField]
    private Text _mainText = null;

    [SerializeField]
    private Text _nameText = null;

    [SerializeField]
    [Range(0f, 0.5f)]
    private float _textInterval = 0.1f;

    private float _oldTextInterval;

    [SerializeField]
    private Animator _characterAnimator;

    [SerializeField]
    private Animator _bossAnimator;

    [SerializeField]
    private Animator _nextIconAnimator;

    [SerializeField]
    private AudioSource _textAudioSource;

    [SerializeField]
    private GSSReader _gssReader;

    [SerializeField]
    private KeyCode _novelInputKeys = KeyCode.Space;

    private string[][] _textDataJaggedArray = null;

    private int _ggsRow = 0;
    private int _currentCharNum = 0;

    private bool _isCommandFirstTime = true;
    private bool _isDisplaying = false;
    private bool _isClick = false;
    private bool _isPause = false;

    private const int NAME_TEXT_COLUMN = 0;
    private const int MAIN_TEXT_COLUMN = 1;
    private const int ACTION_TEXT_COLUMN = 2;

    private const string ANIMATION_COMMAND_IDENTIFIER = "&CharacterAnim";
    private const string SFX_COMMAND_INDENTIFIER = "&Sound";

    private void Update()
    {
        if (_gssReader.IsLoading || CheckNovelFinish() || _isPause) return;


        ControllText();
    }
    public void OnGSSLoadEnd()
    {
        _textDataJaggedArray = _gssReader.DataJaggedArray;
        _oldTextInterval = _textInterval;
    }

    public void ControllText()
    {
        //�e�L�X�g���Ō�܂œǂݍ��܂�Ă��Ȃ�������
        if (_currentCharNum < _textDataJaggedArray[_ggsRow][MAIN_TEXT_COLUMN].Length)
        {

            _nextIconAnimator.gameObject.SetActive(false);

            if (_isClick)//�N���b�N���ꂽ��e�L�X�g���΂�
            {
                _textInterval = 0;
                _isClick = false;
            }

            if (_textDataJaggedArray[_ggsRow][MAIN_TEXT_COLUMN][_currentCharNum] == '&' && _isCommandFirstTime)
            {
                //�R�}���h���͂����o����
                Command();
                _isCommandFirstTime = false;
            }
            else
            {
                DisplayText();
            }
        }
        else//�e�L�X�g���Ō�܂œǂݍ��܂ꂽ��
        {
            _nextIconAnimator.gameObject.SetActive(true);
            if (_isClick)
            {
                NextRow();//�s�̓Y�������J�E���g�A�b�v
                _isClick = false;
            }
        }
    }

    void NextRow()
    {
        _ggsRow++;
        _textInterval = _oldTextInterval;
        _currentCharNum = 0;
        _mainText.text = string.Empty;
        _nameText.text = string.Empty;

        _isCommandFirstTime = true;
        _isDisplaying = false;
    }

    void Command()
    {
        string command = _textDataJaggedArray[_ggsRow][MAIN_TEXT_COLUMN];
        string action = _textDataJaggedArray[_ggsRow][ACTION_TEXT_COLUMN];
        switch (command)
        {
            case ANIMATION_COMMAND_IDENTIFIER:
                Debug.Log("���C���L�����A�j���[�V�����A�j���[�V����" + action);
                _characterAnimator.Play(action);
                break;
            case SFX_COMMAND_INDENTIFIER:
                SoundManager.Instance.UseSFX(action);
                break;
            default:
                Debug.LogError(command + action + "�Ƃ����R�}���h�͖����ł�");
                break;
        }
        NextRow();
    }

    void DisplayText()
    {
        //�o�͈͂�s�ɂ���x�̂ݎ��s����
        if (_isDisplaying) return;
        StartCoroutine(MoveText());
    }

    IEnumerator MoveText()
    {
        while (_isPause)
        {
            yield return null;
        }

        _isDisplaying = true;

        switch (_textDataJaggedArray[_ggsRow][NAME_TEXT_COLUMN])
        {
            case "���ʉ�":
                _textAudioSource.mute = true;
                _nameText.text = string.Empty;
                break;
            default:
                _textAudioSource.mute = false;
                _nameText.text = _textDataJaggedArray[_ggsRow][NAME_TEXT_COLUMN];
                break;
        }

        while (_isDisplaying)
        {
            _textAudioSource.Play();

            if (_currentCharNum == _textDataJaggedArray[_ggsRow][MAIN_TEXT_COLUMN].Length) yield break;

            _mainText.text += _textDataJaggedArray[_ggsRow][MAIN_TEXT_COLUMN][_currentCharNum];
            _currentCharNum++;
            yield return new WaitForSeconds(_textInterval);
        }
    }


    public void InputClick()
    {
        if (Input.GetKeyDown(_novelInputKeys))
        {
            _isClick = true;
        }

        if (Input.GetKeyUp(_novelInputKeys))
        {
            _isClick = false;
        }
    }

    bool CheckNovelFinish()
    {
        if (_ggsRow >= _textDataJaggedArray.Length)
        {
            Debug.Log("���ׂẴm�x����ǂݍ���");
            IsNovelFinish = true;
            _mainText.text = string.Empty;
            _nameText.text = string.Empty;
            return true;
        }
        else
        {
            IsNovelFinish = false;
            return false;
        }
    }

    void IPauseable.PauseResume(bool isPause)
    {
        _isPause = isPause;
    }

    private void OnEnable()
    {
        PauseManager.Instance.SetEvent(this);   
    }

    private void OnDisable()
    {
        PauseManager.Instance.RemoveEvent(this);
    }
}