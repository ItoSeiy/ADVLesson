using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Overdose.Novel
{
    public class GSSReader : MonoBehaviour
    {
        public string[][] Datas { get; private set; }
        public bool IsLoading { get; private set; }

        public event Action OnLoadEnd;

        [SerializeField]
        private string SheetID = "読み込むシートのID";

        [SerializeField]
        private string SheetName = "読み込むシート";

        void Start()
        {
            Load();
        }

        public void Load() => StartCoroutine(GetFromWeb());

        private IEnumerator GetFromWeb()
        {
            IsLoading = true;

            var tqx = "tqx=out:csv";
            var url = "https://docs.google.com/spreadsheets/d/" + SheetID + "/gviz/tq?" + tqx + "&sheet=" + SheetName;
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            IsLoading = false;

            var protocol_error = request.result == UnityWebRequest.Result.ProtocolError;
            var connection_error = request.result == UnityWebRequest.Result.ConnectionError;
            if (protocol_error || connection_error)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Datas = ConvertCSVtoJaggedArray(request.downloadHandler.text);
                OnLoadEnd.Invoke();
            }

        }


        private string[][] ConvertCSVtoJaggedArray(string t)
        {
            var reader = new StringReader(t);
            reader.ReadLine();  //ヘッダ読み飛ばし
            var rogws = new List<string[]>();
            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLine();
                var elements = line.Split(',');
                for (var i = 0; i < elements.Length; i++)
                {
                    elements[i] = elements[i].TrimStart('"').TrimEnd('"');
                }
                rows.Add(elements);
            }
            return rows.ToArray();
        }
    }
}
