using System.Collections.Generic;
using System.Text;
using BackEnd;
using UnityEngine;

public class BackEndRank
{
    private static BackEndRank _instance;

    public static BackEndRank Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackEndRank();
            }
            return _instance;
        }
    }

    public void RankInsert(long money)
    {
        string RankUUID = "0198c64d-0138-78e2-876a-7b16b01d5ae1";
        string tablename = "Money";
        string rowInData = string.Empty;

        var bro = Backend.GameData.GetMyData(tablename, new Where());

        if (bro.IsSuccess())
        {

        }

        if (bro.FlattenRows().Count > 0)
        {
            rowInData = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            var bro2 = Backend.GameData.Insert(tablename);

            if (bro2.IsSuccess() == false)
            {
                return;
            }


            rowInData = bro2.GetInDate();
        }

        Param param = new Param();
        param.Add("Money", money);

        var rankbro = Backend.URank.User.UpdateUserScore(RankUUID, tablename, rowInData, param);
    }


    public List<string> RankGet()
    {
        string rankUUID = "0198c64d-0138-78e2-876a-7b16b01d5ae1";
        var bro = Backend.URank.User.GetRankList(rankUUID);
        List<string> rank = new List<string>();
 
        if (bro.IsSuccess() == false)
        {
            return null;
        }

        foreach (LitJson.JsonData json in bro.FlattenRows())
        {
            StringBuilder info = new StringBuilder();
            long money = long.Parse(json["score"].ToString());
            info.AppendLine($"{json["rank"].ToString()}|위 {json["nickname"].ToString()}|점수: {money:N0}");

            rank.Add(info.ToString());
            Debug.Log(info);
        }

        return rank;
    }
}
