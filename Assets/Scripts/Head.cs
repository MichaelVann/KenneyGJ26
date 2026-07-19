using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] BoxCollider2D _boxCollider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _massPerVolume, _defaultHeadSize, _headSizeIncreaseInterval, _proportionalIncrement, _originalHeadWallHeight, _headWallHeightIncrement;
    [SerializeField] Transform[] _headWalls;
    [SerializeField] SpriteRenderer[] _eyes;
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] float _balancePidProportional, _balancePidIntegral, _balancePidDifferential;
    vPID m_balancePID;
    Player m_player;

    Rigidbody2D m_rigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_balancePID = new vPID(1f, 0f, 0f);
        m_player = GetComponentInParent<Player>();
        RefreshSize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float proportional = _balancePidProportional + _proportionalIncrement * BattleHandler.s_instance.GetUpgrade(Upgrade.eUpgradeType.Stabiliser).GetLevel();

        m_balancePID.SetVariables(proportional, _balancePidIntegral, proportional / 4f);
        float currentAngle = VLib.Vector2ToEulerAngle(transform.up);
        float torque = m_balancePID.Update(currentAngle, 0f);
        m_rigidBody.AddTorque(torque * Time.fixedDeltaTime);

        for (int i = 0; i < _headWalls.Length; i++)
        {
            SpriteRenderer spriteRenderer = _headWalls[i].GetComponent<SpriteRenderer>();
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, _originalHeadWallHeight + _headWallHeightIncrement * BattleHandler.s_instance.GetUpgrade(Upgrade.eUpgradeType.WallHeight).GetLevel());
        }

    }

    internal void SetSizeLevel(int a_level)
    {
        _spriteRenderer.size = new Vector2(_defaultHeadSize + _headSizeIncreaseInterval * a_level, _spriteRenderer.size.y);
        RefreshSize();
    }

    void RefreshSize()
    {
        _boxCollider.size = _spriteRenderer.size;
        m_rigidBody.mass = _boxCollider.size.x * _boxCollider.size.y * _massPerVolume;

        float wallY = 0.8f + _headWalls[0].gameObject.GetComponent<SpriteRenderer>().size.y / 2f;
        _headWalls[0].transform.localPosition = new Vector3(-_boxCollider.size.x/2f, wallY);
        _headWalls[1].transform.localPosition = new Vector3(_boxCollider.size.x/2f,  wallY);

        _eyes[0].transform.localPosition = new Vector2(-_boxCollider.size.x / 4f, _eyes[0].transform.localPosition.y);
        _eyes[1].transform.localPosition = new Vector2(_boxCollider.size.x / 4f, _eyes[1].transform.localPosition.y);
    }

    private void OnCollisionEnter2D(Collision2D a_collision)
    {
        if (a_collision.gameObject.CompareTag("Floor"))
        {
            m_player.Explode();
            Instantiate(_explosionPrefab, a_collision.contacts[0].point, Quaternion.identity);
        }
    }
}
