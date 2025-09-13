using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;

public class RankUP : MonoBehaviour
{
    private TextMeshProUGUI _txte;
    private bool _active = true;


    private void Awake()
    {
        _txte = GetComponent<TextMeshProUGUI>();
    }

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
