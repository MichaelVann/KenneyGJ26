using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] int _upgradeID;
    [SerializeField] Button _buyButton;
    [SerializeField] TextMeshProUGUI _titleText, _costText, _levelText;
    Upgrade m_upgrade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_upgrade = BattleHandler.s_instance.GetUpgrade(_upgradeID);

        switch ((Upgrade.eUpgradeType)_upgradeID)
        {
            case Upgrade.eUpgradeType.HeadWidth:
            _titleText.text = "Head Scale";
                break;
            case Upgrade.eUpgradeType.MovementForce:
            _titleText.text = "Move Speed";
                break;
            case Upgrade.eUpgradeType.Stabiliser:
            _titleText.text = "Stability";
                break;
            case Upgrade.eUpgradeType.WallHeight:
            _titleText.text = "Wall Height";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int cash = GameHandler.s_instance.GetCash();
        _buyButton.interactable = m_upgrade.GetCost() <= cash;
        _costText.text = "$" + m_upgrade.GetCost().ToString();
        _levelText.text = m_upgrade.GetLevel().ToString();
    }

    public void BuyPressed()
    {
        m_upgrade.AttemptToBuy();
    }
}
