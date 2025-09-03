using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private SloltMachine _sloltMahcin;
    public MoneyLogUI logUI;


    [Header("������� �Աݵ� �ݾ�")]
    [SerializeField] private long _bankBook;
    private long aaa;
    [Header("������ ex)10 ->10%")]
    [SerializeField] private long aa;
    [Header("������� ����")]
    [SerializeField] private long _condition;

    public int _rounds = 3;
    private bool a;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _conditionText;
    [SerializeField] private TextMeshProUGUI _currentBankText;
    private void Start()
    {
        _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";//������� ���� ǥ��
        _currentBankText.text = $"Current deposit amount : {_bankBook.ToString("N0")}";//������� �Աݵ� �ݾ� ǥ��

    }

    private void Update()
    {
        if (!a)
            CheckMoney();

        MoneyP();
    }
    public void InMoney()//��ư�� �̺�Ʈ
    {
        //���� �����ݿ� x%��ŭ ����
        aaa = _condition / 10;
        _moneyManager.Money -= aaa;
        //������ �ݾ׸�ŭ ����
        _bankBook += aaa;
        //Ui���� 
        _creditsText.text = $"Credits : {_moneyManager.Money.ToString("N0")}";//���� ������ �ݾ� ����\
        logUI.AddLog($"-{aaa.ToString("N0")} balance : {_moneyManager.Money.ToString("N0")}", Color.red);
        _currentBankText.text = $"Current deposit amount : {_bankBook.ToString("N0")}";//������� �Աݵ� �ݾ� ǥ��
    }

    public void CheckMoney()
    {
        //������ �ݾ��� ���ǿ� �´��� Ȯ��

        if (_bankBook >= _condition)
        {
            //�´ٸ� �������� ����
            _condition = _condition * 2;
            _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";//������� ���� ǥ��
            _sloltMahcin.HaveSpin += 10;
            _sloltMahcin.UpdateMagnificationUI();
        }
        else if (_bankBook < _condition && _rounds == 0)
        {
            Debug.Log("���ӿ���");
            a = true;
        }
    }

    private void MoneyP()
    {
        long abc =_bankBook * aa / 100;
        bool async = false;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!async)
            {
                //���� ���� �� ����
                _rounds--;
                //���� �Աݵ� �ݾ��� x%��ŭ �� ����
                _moneyManager.Money +=abc;
                _creditsText.text = $"Credits : {_moneyManager.Money.ToString("N0")}";//���� ������ �ݾ� ����
                logUI.AddLog($"+{abc.ToString("N0")} balance : {_moneyManager.Money.ToString("N0")}",Color.green);


                //���� ���� ���� => ��.. ��� ������?
            }
        }
        async = true;

    }
}
