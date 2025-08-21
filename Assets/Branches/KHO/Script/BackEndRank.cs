using System.Linq;
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
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tablename);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInData = bro2.GetInDate();
        }

        Param param = new Param();
        param.Add("Money", money);

        var rankbro = Backend.URank.User.UpdateUserScore(RankUUID, tablename, rowInData, param);

        if (rankbro.IsSuccess())
        {
            Debug.Log("성공");
        }    
    }


    public void RankGet()
    {
        string rankUUID = "0198c64d-0138-78e2-876a-7b16b01d5ae1";
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.Log("랭킹 조회오류");
            return;
        }

        foreach (LitJson.JsonData json in bro.FlattenRows())
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine("순위 : " + json["rank"].ToString());
            info.AppendLine("닉네임" + json["nickname"].ToString());

            Debug.Log(info);
        }
    }
}
