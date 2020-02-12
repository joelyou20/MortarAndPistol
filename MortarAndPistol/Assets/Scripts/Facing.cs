using UnityEngine;

public class Facing : MonoBehaviour
{
    private enum FacingType
    {
        FollowMouse,
        RbMove,
        TransMove
    }

    [SerializeField] private FacingType facingType = FacingType.FollowMouse;

    private int _facing;
    private Rigidbody2D _rb;
    private GameObject _player;
    private Vector3 _tempPos;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacing();
    }

    public int GetFacing() => _facing;

    public void SetFacing(int facing) => _facing = facing;

    private void UpdateFacing()
    {
        switch (facingType)
        {
            case FacingType.FollowMouse:
                _facing = FollowMouse_Facing();
                break;
            case FacingType.RbMove:
                _facing = Rb_Move_Facing();
                break;
            case FacingType.TransMove:
                _facing = Trans_Move_Facing();
                break;
            default:
                break;
        }

        transform.rotation = new Quaternion(0, _facing, 0, 0);
    }

    private int FollowMouse_Facing()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y + 1);
        Vector2 direction = target - myPos;
        direction.Normalize();
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        var value = (target.x - pos.x);
        return value >= 0 ? 0 : 1;
    }

    private int Rb_Move_Facing()
    {
        if (_rb.velocity.x >= 0.01f)
        {
            return 0;
        }
        else if (_rb.velocity.x <= -0.01f)
        {
            return 1;
        }

        return _facing;
    }

    private float CalculateDistance(Vector2 p1, Vector2 p2)
    {
        var sign = (p2.x - p1.x) >= 0 ? 1 : -1;
        return Mathf.Sqrt(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow((p2.y - p1.y), 2)) * sign;
    }

    private int Trans_Move_Facing()
    {
        // This is a problem. Don't want the enemies always looking at the player.
        var dist = CalculateDistance(transform.position, _player.transform.position);
        if(dist >= 0.01f)
        {
            return 0;
        }
        else if(dist <= -0.01f)
        {
            return 1;
        }

        return _facing;
    }
}
