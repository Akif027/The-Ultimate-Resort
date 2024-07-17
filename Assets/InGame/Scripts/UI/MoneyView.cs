using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoneyView : MonoBehaviour
{
    public TextMeshProUGUI value;

    private void Start()
    {
        EventManager.OnCoinChanged += OnMoneyChanged;
        EventManager.RaiseOnCoinChanged();
    }

    private void OnDestroy()
    {
        EventManager.OnCoinChanged -= OnMoneyChanged;
    }


    private void OnMoneyChanged()
    {
        value.text = Game.Instance.gameData.money.ToString();
    }
}
