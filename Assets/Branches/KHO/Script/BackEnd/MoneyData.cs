using UnityEngine;
using BackEnd;
using System.Text;

public class MoneyData
{
    public long money => MoneyMangaer.Instance.Money;

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"Money {money}");

        return result.ToString();
    }

}

public class MoneyGameData
{
    private static MoneyGameData _instance = null;

    public static MoneyGameData Intance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MoneyGameData();
            }

            return _instance;
        }
    }

    public static MoneyData moneyData;
    private string gameDataRowInDate = string.Empty;

    public void GameDateInsert()
    {
        if (moneyData == null)
        {
            moneyData = new MoneyData();
        }

        Param param = new Param();
        param.Add("Money", moneyData.money);

        var bro = Backend.GameData.Insert("Money", param);

        if (bro.IsSuccess())
        {
            gameDataRowInDate = bro.GetInDate();
        }

    }

    public void GetData()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");

        var bro = Backend.GameData.GetMyData("Money", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
                
                Param param = new Param();
                param.Add("Money",0);
                var broInIt = Backend.GameData.Insert("Money", param);
                if (bro.IsSuccess())
                {
                    gameDataRowInDate = bro.GetInDate();
                }
            }
            else
            {
                
                //gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.  

                //moneyData = new MoneyData();

                //moneyData.money = long.Parse(gameDataJson[0]["Money"].ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);

        }
    }

    public void UpdateDate()
    {
        if (moneyData == null)
        {
            moneyData = new MoneyData();
            // return;
        }

        Param param = new Param();

        BackendReturnObject bro = null;

        param.Add("Money", moneyData.money);

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            bro = Backend.GameData.Update("Money", new Where(), param);
        }
        else
        {
            bro = Backend.GameData.UpdateV2("Money", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("수정 성공");
            BackEndRank.Instance.RankInsert(moneyData.money);
        }
        else
        {
            Debug.Log("수정 실패 ");
        }
    }
}


