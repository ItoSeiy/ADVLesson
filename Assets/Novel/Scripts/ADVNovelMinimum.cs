using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ADVNovelMinimum : MonoBehaviour
{
    [SerializeField]
    Text _dialogText;

    [SerializeField] 
    TextAsset _scriptTextFile;

    void Start()
    {
        var splitted = _scriptTextFile.text.Replace("\r", "").Split('\n');
        StartCoroutine(AdvPlay(splitted));
    }

    IEnumerator AdvPlay(string[] texts)
    {
        foreach (var text in texts)
        {
            _dialogText.text = text;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            yield return new WaitForSeconds(0.1f);
        }
    }
}