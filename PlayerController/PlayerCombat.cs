using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    private Rigidbody2D _rigidbody2D;

    private Animator  _animator ;

    [SerializeField]
    private Vector2
        knockForce;

    [SerializeField]
    private float
        _dashSpeed,
        _dashCooldown,
        _dashTime,
        _jumpForce;

    private float
        _bounceDuration = 0.6f,
        _bounceTimer,
        _dashResetDelay = 0.08f,
        _dashResetDelayTimer = 0.08f,
        _marioAttackTimer = 0f,
        _adForceTimer = -1f,
        _hasJumpedTimer = 0f,
        _dashTimeLeft,
        _fallTimer = 0,
        _fallCooldown = .3f,
        _delayTimer;

    [HideInInspector]
    public float
        lastDash;

    private bool
        _lockDoubleJump = false,
        _shouldAccelerateMA,
        _canJump,
        _doubleJump;

    private int
        _attackState = 1;

    private Vector2
        _marioAttackInitialVelocity,
        workSpace;

    public bool MarioAttack { get; set; }

    public bool IsDashing { get; set; }

    public bool IsLanding { get; set; }

    public bool IsFalling { get; set; }

    public bool AttackChain { get; set; }

    public bool Attacking { get; set; }

    public bool MarioAction { get; set; }

    public bool HasJumped { get; set; }

    public bool IsBouncing { get; set; }

    public bool IsJumping { get; set; }

    public static bool FacingRightBeforAction { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator  = GetComponent<Animator >();
        _marioAttackInitialVelocity = new Vector3(0f, -10f);
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        CanJump();
        StartCoroutine(Timer());
        //Landed();
        CheckDash();
        MarioAttackFnc();
        //CountBounceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
;       if (other.CompareTag("Enemy") && IsDashing)
        {
            BounceBack(EnemyToTheLeft(other.transform));
        }
    }

   
    private void CheckDash()
    {
        if (IsDashing)
        {
            if (_dashTimeLeft >= 0)
            {
                _rigidbody2D.velocity = new Vector2(PlayerController.Instance.FacingDirection().x * _dashSpeed, 0);
                _dashTimeLeft -= Time.deltaTime;
            }
            else
            {
                PlayerController.Instance.CanMove();

                if (_dashResetDelayTimer >= 0)
                {
                    _dashResetDelayTimer -= Time.deltaTime;
                }
                else
                {
                    _dashResetDelayTimer = _dashResetDelay;
                    IsDashing = false;
                }
            }
        }
    }

    private void BounceBack(bool enemyToTheLeft)
    {
        _rigidbody2D.velocity = Vector2.zero;

        KnockDelegate.Knocking(enemyToTheLeft, knockForce);
        IsDashing = false;
        _dashTimeLeft = -0.01f;
        _dashResetDelayTimer = -0.01f;
        _animator.SetTrigger("bounce");
        _bounceTimer = _bounceDuration;

        FacingRightBeforAction = PlayerController.Instance.FacingDirection().x == 1;
        _rigidbody2D.AddForce(new Vector2(-300f * PlayerController.Instance.FacingDirection().x, 550f));
    }

    //private void CountBounceTimer()
    //{
    //    if(_bounceTimer > 0)
    //    {
    //        _bounceTimer -= Time.deltaTime;
    //    }

    //    else IsBouncing = false;
    //}

    public void Jump()
    {
        if (_canJump && !_doubleJump)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _rigidbody2D.AddForce(new Vector2(0, _jumpForce));
            _animator.SetTrigger("jump");
            _doubleJump = true;
            _lockDoubleJump = true;
        }
        else if (_doubleJump)
        {
            _animator.ResetTrigger("jump");
            _animator.SetTrigger("double_jump");
            _animator.SetBool("landing", false);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _rigidbody2D.AddForce(new Vector2(0, _jumpForce));
            _doubleJump = false;
        }
    }
   
    private void CanJump()
    {
        if (PlayerController.Instance.Grounded() && !_lockDoubleJump)
        {
            _doubleJump = false;
            _fallTimer = _fallCooldown;
            _animator.ResetTrigger("double_jump");
        }

        else if (!PlayerController.Instance.Grounded())
        {
            if (_fallTimer > 0)
            {
                _fallTimer -= Time.deltaTime;
                return;
            }
            else
            {
                _canJump = false;
                return;
            }
        }

        _canJump = true;
    }

    private void MarioAttackFnc()
    {
        if (MarioAttack)
        {
            _marioAttackTimer += Time.deltaTime;

            if (_marioAttackTimer > 0.1)
            {
                if (!_shouldAccelerateMA)
                {
                    _rigidbody2D.velocity = new Vector2(0f, -10f);
                    _shouldAccelerateMA = true;
                }
                else
                {
                    _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y * 1.08f);
                }
            }
            
        }
        else
        {
            _marioAttackTimer = 0f;
        }
    }

    private IEnumerator Timer()
    {
        if (HasJumped)
        {
            _hasJumpedTimer += Time.deltaTime;
        }
        else
        {
            _hasJumpedTimer = 0f;
        }

        if (_lockDoubleJump)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            _lockDoubleJump = false;
        }
    }

    public void StopJump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.7f);
    }

    //private void Landed()
    //{
    //    if (HasJumpedTimer > 0.2 && PlayerController.Instance.Grounded())
    //    {
    //        _animator .SetTrigger("land");
    //        HasJumped = false;
    //    }
    //}

    public void StartMarioAttack()
    {
        if (!PlayerController.Instance.Grounded())
        {
            MarioAction = true;
            _animator.Play("HighUpAttack");
            MarioAttack = true;
            PlayerController.Instance.StopMovment();
            _animator.ResetTrigger("jump");
            _shouldAccelerateMA = false;
            FacingRightBeforAction = PlayerController.Instance.FacingDirection().x == 1;
        }
    }

    public void IsBouncingFalse()
    {
        IsBouncing = false;
    }

    public bool EnemyToTheLeft(Transform enemyPos)
    {

        return transform.position.x < enemyPos.transform.position.x;
    }

    public void Attack()
    {
        if (PlayerController.Instance.Grounded())
        {
            switch (_attackState)
            {
                case 1:
                    _animator.Play("Attack");
                    _attackState = 2;
                    break;

                case 2:
                    AttackChain = true;
                    break;
            }
        }
    }

    public int GetAttackState()
    {
        return _attackState;
    }

    public void SetAttackState(int val)
    {
        _attackState = val;
    }

    public void AttemptToDash()
    {
        if (Time.time > lastDash + _dashCooldown)
        {
            PlayerController.Instance.StopMovment();
            _dashTimeLeft = _dashTime;
            lastDash = Time.time;
            IsDashing = true;

            if (PlayerController.Instance.Grounded()) _animator.Play("Dash_v2", 0);
            else _animator.Play("Dash_v2", 1);
        }
    }
}
