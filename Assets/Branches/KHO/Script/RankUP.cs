using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RankUP : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private bool _active = true;
    private float _currentTime = 0;
    private float _rankUpdateTime = 30f;


    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(TypeRankText(_text));
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _rankUpdateTime)
        {
            StartCoroutine(TypeRankText(_text));
        }
    }



    //좋은데 느낌이 안살아남
    public void RankText(TextMeshProUGUI ranktext)
    {
        int rankMark = 10;
        List<string> ranks = BackEndRank.Instance.RankGet();
        if (ranks == null)
        {
             ranktext.text = string.Empty;
            return;
        }
        ranktext.text = string.Empty;
        for (int i = 0; i < ranks.Count;i++)
        {
            ranktext.text += ranks[i];

            if (i + 1 == rankMark)
            {
                break;
            }
        }
    }

    public IEnumerator TypeRankText(TextMeshProUGUI ranktext)
    {
        _active = false;
        ranktext.text = string.Empty;
        int rankMark = 10;
        List<string> ranks = BackEndRank.Instance.RankGet();

        for (int i = 0; i < ranks.Count && i < rankMark; i++)
        {
            string rankLine = ranks[i];
            foreach (char c in rankLine)
            {
                ranktext.text += c;
                yield return new WaitForSeconds(0.01f);
            }
        }

        _active = true;
    }
}
