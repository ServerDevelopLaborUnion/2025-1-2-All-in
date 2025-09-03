using TMPro;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private MoneyManager _moneyManager;
    private long _bankBook;
    private long _interest;
    private long _condition;
    private int _rounds;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _bankBookText;
    public void InMoney()//��ư�� �̺�Ʈ
    {
        //���� �����ݿ� x%��ŭ ����
        _interest = _moneyManager.Money / 10;
        //������ �ݾ׸�ŭ ����
        _bankBook += _interest;
        //Ui���� 
        _creditsText.text = "Credits :" + _moneyManager.Money;//���� ������ �ݾ� ����
        _bankBookText.text = "DeadLine :" + _bankBook;//���� ������ �ݾ� ����
        CheckMoney();
    }

    private void CheckMoney()
    {
        //������ �ݾ��� ���ǿ� �´��� Ȯ��
        if (_bankBook >= _condition)
        {
            //�´ٸ� �������� ����
            _condition = _condition * 2;
        }
        else if (_rounds > 0)//�ƴϸ� ���� ���� ����x�� x > 0���� Ȯ��
        {
            //�´ٸ� �ٿ� ���� ����
            MoneyP();
        }
        else
        {
            //�ƴϸ� ���� ����
            //���� �˾�â ���°� ���� ��
        }
    }

    private void MoneyP()
    {
        bool async = false;
        if (!async)
        {
            //���� ���� �� ����
            _rounds--;
            //���� �Աݵ� �ݾ��� x%��ŭ �� ����
            _moneyManager.Money += _bankBook / 10;
            _creditsText.text = "Credits :" + _moneyManager.Money;//���� ������ �ݾ� ����

            //���� ���� ���� => ��.. ��� ������?
        }
        async = true;

    }
}
