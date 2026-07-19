

public class Upgrade
{
    int m_level;
    int m_cost;
    const float m_costScaling = 1.25f;

    internal int GetCost() { return m_cost; }
    internal int GetLevel() { return m_level; }

    internal enum eUpgradeType
    {
        HeadWidth, MovementForce, Stabiliser, WallHeight, Count
    }

    internal Upgrade()
    {
        m_level = 0;
        m_cost = 4;
    }

    internal void AttemptToBuy()
    {
        int cash = BattleHandler.s_instance.GetCash();
        if (cash >= m_cost)
        {
            BattleHandler.s_instance.ChangeCash(-m_cost);
            LevelUp();
        }
    }

    void LevelUp()
    {
        m_level++;
        m_cost = (int)(m_cost * m_costScaling);
    }
}