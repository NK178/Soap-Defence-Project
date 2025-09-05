using UnityEngine;

public class MoneyUI : GeneralTextUI
{

    [SerializeField] private FloatSO playerMoney;

    // Update is called once per frame
    void Update()
    {
        string newString = playerMoney.value.ToString();
        display.text = newString;
    }
}
