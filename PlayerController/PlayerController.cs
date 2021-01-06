using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField]
    private float
        _speed,
        _jumpForce;

    [SerializeField]
    private LayerMask _whatIsGround;

    private readonly PlayerInput _playerInput = new PlayerInput();

    [SerializeField]
    private Transform
        _groundCheck;

    [SerializeField]
    private Collider2D _collider2D;

    public new Rigidbody2D rigidbody2D;

    private Animator _animator;

    public float
        groundCheckDist;

    private float
        _aux,
        _horizontal;

    private bool
        _hasBounced,
        _hasMarioJumped,
        _facingRight = true;

    private bool
        canMove;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Initialize();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _playerInput.HandleInputKeyBoard();
    }

    private void FixedUpdate()
    {
        HandleMovment();
        FacingRight();
        ControlLanding();
        ControlLayers();
        //CheckLanding();
        LandingOrFalling();
        CheckingActions();
    }

    private void HandleMovment()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _animator.SetFloat("speed", Mathf.Abs(_horizontal));

        if (canMove)
        {
            rigidbody2D.velocity = new Vector2(_horizontal * _speed * Time.deltaTime, rigidbody2D.velocity.y);
        }

        else if (!PlayerCombat.Instance.MarioAttack && _hasMarioJumped && !PlayerCombat.Instance.IsDashing)
        {
            SemiMovmentControl(PlayerCombat.FacingRightBeforAction, new Vector2(-6, 8));
        }

        else if (!PlayerCombat.Instance.IsBouncing && _hasBounced && !PlayerCombat.Instance.IsDashing)
        {
            SemiMovmentControl(PlayerCombat.FacingRightBeforAction, new Vector2(-8, 2));
        }


    }

    private void CheckingActions()
    {
        if (PlayerCombat.Instance.MarioAction)
        {
            _hasMarioJumped = true;
            _hasBounced = false;
        }

        if (PlayerCombat.Instance.IsBouncing)
        {
            _hasBounced = true;
            _hasMarioJumped = false;
        }

        if (PlayerCombat.Instance.MarioAction)
        {
            _hasMarioJumped = true;
        }

        if (Grounded())
        {
            PlayerCombat.Instance.HasJumped = false;

            _hasMarioJumped = false;
            _hasBounced = false;

            if (!PlayerCombat.Instance.IsDashing) canMove = true;
        }

    }
    
    public bool Grounded()
    {
        Vector3 aux = new Vector3(0.1f, 0f);

        RaycastHit2D ray = Physics2D.BoxCast(_collider2D.bounds.center,
                         _collider2D.bounds.size - aux, 0f, Vector2.down, 0.1f, _whatIsGround);

        return ray.collider != null;
    } 

    private void Initialize()
    {
        canMove = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void LandingOrFalling()
    {
        if(!PlayerCombat.Instance.HasJumped && _animator.GetBool("landing") && 
            !_animator.GetCurrentAnimatorStateInfo(1).IsName("Falling") && !_hasBounced)
        {
            _animator.Play("Falling");
        }
    }

    private void FacingRight()
    {
        if((_horizontal < 0 && _facingRight) || (_horizontal > 0 && !_facingRight))
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (!PlayerCombat.Instance.MarioAction && !PlayerCombat.Instance.IsBouncing)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            _facingRight = !_facingRight;
        }
    }

    private void ControlLanding()
    {
        if (!Grounded() && rigidbody2D.velocity.y < -0.01f &&
            !PlayerCombat.Instance.IsDashing && !PlayerCombat.Instance.IsBouncing)
        {
            _animator.SetBool("landing", true);
        }
        else
        {
            _animator.SetBool("landing", false);
        }
    }

    private void SemiMovmentControl(bool facingRight, Vector2 bounds)
    {
        _aux = _horizontal * _speed * Time.deltaTime * 0.1f + rigidbody2D.velocity.x;

        if (facingRight) _aux = Mathf.Clamp(_aux, bounds.x, bounds.y);
        else _aux = Mathf.Clamp(_aux, -bounds.y, -bounds.x);

        rigidbody2D.velocity = new Vector2(_aux, rigidbody2D.velocity.y);
    }

    private void ControlLayers()
    {
        if (!Grounded() && !PlayerCombat.Instance.IsDashing)
        {
            _animator.SetLayerWeight(1, 1);
            _animator.SetBool("grounded", false);
        }
        else if(Grounded())
        {   
            _animator.SetLayerWeight(1, 0);
            _animator.SetBool("grounded", true);
        }
    }
    
    //private void CheckLanding()
    //{        
    //    if(Grounded() && _inAirTime > 0.1f)
    //    {
    //        _animator.SetBool("grounded", true);
    //    }

    //    InAirCounter();
    //}

    //private void InAirCounter()
    //{
    //    if (!Grounded())
    //    {
    //        _inAirTime += Time.deltaTime;
    //    }
    //    else
    //    {
    //        _inAirTime = 0f;
    //    }
    //}

    public void AddBounceMA()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(new Vector2(_jumpForce * FacingDirection().x * 0.5f, _jumpForce));
    }

    public void StopMovment()
    {
        canMove = false;
        rigidbody2D.velocity = Vector2.zero;
    }
    
    public void CanMove()
    {
        canMove = true;
    }

    public void SetLayerWeight(int layerIndex, float layerWeight)
    {
        _animator.SetLayerWeight(layerIndex, layerWeight);
    }

    public Vector2 FacingDirection()

    {
        if (_facingRight)
        {
            return Vector2.right;
        }
        return Vector2.left;
    }

    public void SetAnimation(string name)
    {
        _animator.SetTrigger(name);
    }
    
    public void SetAnimation(string name, bool val)
    {
        _animator.SetBool(name, val);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - groundCheckDist));
    //}
}
