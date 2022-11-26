using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 // WHY 
using Photon.Pun;
using Photon.Realtime;
using System;
using static CustomPropertiesConstant;
using TMPro;
public class PUNPlayerController : MonoBehaviourPunCallbacks
{
    // ------------------------------------ ANIMATION VARIABLES ------------------------------------ // 
    public UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary;
    [SerializeField] private Animator _animator;
    [SerializeField] AnimationState _animationState;
    // ------------------------------------ STATS VALUE ------------------------------------ // 
    public float speed = .2f;
    public float jumpSpeed = .2f;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _jumpTimeCounter;
    private bool _isJumping;
    private float _baseSpeed;
    public float baseSpeed { get { return _baseSpeed; } }
    private float _directionX;
    private bool _isShielded;
    private float _baseGravityScale;
    public float baseGravityScale { get { return _baseGravityScale; } }
    public bool forcedMovement = false;
    public bool lockMovement = false;
    [SerializeField] private bool _pushCD;
    [SerializeField] private bool _shieldCD;
    [SerializeField] public float pushCoolDownTime;
    [SerializeField] public float shieldCoolDownTime;
    [SerializeField] public float _shieldDuration;
    // ------------------------------------ PHYSIC VARIABLES ------------------------------------ // 
    private Rigidbody2D _rb;
    public Rigidbody2D rb { get { return _rb; } }
    private Collider2D collider2D;
    public LayerMask grounds;
    [SerializeField] Transform _originalParent;
    public Transform originalParent { get { return _originalParent; } }

    // ------------------------------------ NETWORK VARIABLES ------------------------------------ // 
    public PhotonView view;
    public int skinIndex; // obsolete
    public ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Player player;
    [SerializeField] TextMeshPro nameTagText;
    // ------------------------------------ SKILL AND POWERUP, Effects ------------------------------------ // 
    [SerializeField] PlayerPowerUpController playerPowerUpController;
    [SerializeField] GameObject shieldObject;
    [SerializeField] ParticleSystem _speedBoostEffect;
    [SerializeField] GhostEffect _ghostEffect;
    [SerializeField] ParticleSystem _jumpDust;
    public GhostEffect ghostEffect { get { return _ghostEffect; } }

    // ------------------------------------ EVENT ------------------------------------ // 
    Action skillAction;
    public EventHandler OnPushSkillUse;
    public EventHandler OnShieldSkillUse;

    Vector3 spawnPos;
    public AnimationState animationState
    {
        get { return _animationState; }
        set
        {
            _animationState = value;
            OnAnimationStateChange();
        }
    }

    public enum AnimationState
    {
        Idling,
        Running,
        Kicking,
        Hurting
    }
    private void Awake()
    {
        // local variable
        spawnPos = transform.position;
        collider2D = GetComponent<Collider2D>();
        spriteLibrary = GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>();
        if (_animator == null)
            _animator = GetComponent<Animator>();
        animationState = AnimationState.Idling;
        _rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        playerPowerUpController = GetComponent<PlayerPowerUpController>();
        _ghostEffect = GetComponent<GhostEffect>();
        player = view.Owner;
        _baseSpeed = speed;
        _baseGravityScale = rb.gravityScale;
        _originalParent = transform.parent;
    }
    private void Start()
    {
        if (!view.IsMine)
        {
            view.RPC("SyncData", RpcTarget.All);
            GameMananger.instance.otherClientPlayerController = this;
        }
        else
        {
            CameraController.instance.target = transform;
            GameMananger.instance.clientPlayerController = this;
            GameMananger.instance.SetUpSkillUICooldown();
        }
        GameMananger.instance.ShowPlayerEnterNotification(view.Owner.NickName);

    }
    private void Update()
    {
        if (view.IsMine && !_animator.GetBool("IsHurting"))
            ProcessInput();
    }
    private void FixedUpdate()
    {
        if (!_animator.GetBool("IsHurting") && animationState != AnimationState.Kicking)
            MoveHorizontal();
    }
    public void ProcessInput()
    {
        // Debug.Log(PhotonNetwork.SendRate);
        // Debug.Log(PhotonNetwork.SerializationRate);
        // get horizontal direction
        _directionX = Input.GetAxisRaw("Horizontal");
        if (IsGrounded() && Input.GetButtonDown("Jump") && !lockMovement)
        {
            _isJumping = true;
            // _rb.velocity = Vector2.up * jumpSpeed * 3f;
            _jumpTimeCounter = _jumpTime;
            _rb.velocity = Vector2.up * jumpSpeed;
            JumpDustEffect();
            view.RPC(nameof(JumpDustEffect), RpcTarget.Others);
        }
        if (Input.GetButton("Jump") && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _rb.velocity = Vector2.up * jumpSpeed;
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump") || HeadHitGround())
        {
            _isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.Q) && !_pushCD)
        {
            // if (IsGrounded())
            //     rb.velocity = Vector2.zero; // do this for better ground kick
            // animationState = AnimationState.Kicking;
            // skillAction?.Invoke();
            Push();

        }
        if (Input.GetKeyDown(KeyCode.E) && !_shieldCD && !_isShielded)
        {
            StartCoroutine(ShieldSkill(_shieldDuration));
        }

    }
    public void MoveHorizontal()
    {
        if (forcedMovement || lockMovement)
            return;
        // flip character sprite
        if (animationState != AnimationState.Kicking)
        {
            if (_directionX != 0)
            {
                // transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
                transform.rotation = Quaternion.Euler(0f, _directionX > 0 ? 0 : 180f, 0f);
                animationState = AnimationState.Running;
            }
            else if (animationState != AnimationState.Idling)
            {
                animationState = AnimationState.Idling;
            }
        }
        // transform.position += (Vector3)new Vector2(directionX * speed * Time.fixedDeltaTime, 0);
        rb.velocity = new Vector2(_directionX * speed, rb.velocity.y);
    }
    public virtual void OnAnimationStateChange()
    {
        // Debug.Log("animation state changed!");
        // state.AnimationState = (int)animationState;
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, false);
        }

        switch (_animationState)
        {
            case AnimationState.Idling:
                // local
                // _animator.SetBool("IsRunning", false);
                _animator.SetBool("IsIdling", true);
                break;
            case AnimationState.Running:
                // local
                _animator.SetBool("IsRunning", true);
                break;
            case AnimationState.Kicking:
                _animator.SetBool("IsKicking", true);
                break;
            case AnimationState.Hurting:
                _animator.SetBool("IsHurting", true);
                break;
            default:
                throw new System.Exception("Null animation state exception");
        }
    }
    public bool HeadHitGround()
    {
        float extendRayCastDistance = 0.15f;
        RaycastHit2D raycastHit = Physics2D.Raycast(
            collider2D.bounds.center,
            Vector2.up,
            extendRayCastDistance + collider2D.bounds.size.y / 2,
            grounds
        );
        // debug
        Color rayColor = Color.red;
        if (raycastHit.collider == null)
            rayColor = Color.green;
        Debug.DrawRay(
            collider2D.bounds.center,
            Vector2.up * (extendRayCastDistance + collider2D.bounds.size.y / 2),
            rayColor
        );
        return raycastHit.collider != null;
    }
    public bool IsGrounded() // BAD
    {
        float extendRayCastDistance = 0.15f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            collider2D.bounds.center,
            collider2D.bounds.size,
            0f,
            Vector2.down,
            extendRayCastDistance,
            grounds
        );

        // debug
        Color rayColor = Color.green;
        if (raycastHit.collider == null)
            rayColor = Color.red;
        Debug.DrawRay(
            collider2D.bounds.center + new Vector3(collider2D.bounds.extents.x, 0),
            Vector2.down * (collider2D.bounds.extents.y + extendRayCastDistance),
            rayColor
        );
        Debug.DrawRay(
            collider2D.bounds.center - new Vector3(collider2D.bounds.extents.x, 0),
            Vector2.down * (collider2D.bounds.extents.y + extendRayCastDistance),
            rayColor
        );
        Debug.DrawRay(
          collider2D.bounds.center - new Vector3(collider2D.bounds.extents.x, collider2D.bounds.extents.y + extendRayCastDistance),
          Vector2.right * collider2D.bounds.size.x,
          rayColor
        );
        return raycastHit.collider != null;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Kill Zone":
                rb.velocity = Vector2.zero;
                transform.position = spawnPos;
                break;
            case "Win Zone":
                view.RPC("WinGame", RpcTarget.All, view.Owner.NickName);
                // view.
                break;
        }
    }
    //TODO
    public void StopInvulnerable()
    {
        // Debug.Log("stop vulnerable func call");
        ResetAnimationStateToIdle();
        rb.velocity = Vector2.zero;
    }
    public void ResetAnimationStateToIdle()
    {
        animationState = AnimationState.Idling;
    }
    [PunRPC]
    public void PushBack(Vector3 knockBackDirection)
    {
        if (!_isShielded)
        {
            animationState = AnimationState.Hurting;
            rb.velocity = knockBackDirection;
        }
        else Debug.Log("is shielded can't be attacked");
    }
    public void SetSkillAction(Action skill)
    {
        skillAction = skill;
    }
    [PunRPC]
    public void SyncSkin()
    {
        //GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>().spriteLibraryAsset = GameMananger.instance.CharacterSpriteLibraryAssets[skinIndex];
    }
    public void SetData(string name)//, int skinIndex)
    {
        // sync change maybe ...
        //playerProperties[SKIN_INDEX] = skinIndex;
        playerProperties[PLAYER_NAME] = name;
        // local change
        nameTagText.text = name;
        //GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>().spriteLibraryAsset = GameMananger.instance.CharacterSpriteLibraryAssets[skinIndex];
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    [PunRPC]
    public void SyncData()
    {
        //GetComponent<SpriteLibrary>().spriteLibraryAsset =
        //GameMananger.instance.CharacterSpriteLibraryAssets[(int)view.Owner.CustomProperties[SKIN_INDEX]];
        nameTagText.text = view.Owner.NickName;
    }
    [PunRPC]
    public void ToggleShield(bool value)
    {
        shieldObject?.SetActive(value);
        _isShielded = value;
    }
    public IEnumerator ShieldSkill(float duration)
    {
        OnShieldSkillUse?.Invoke(this, null);
        view.RPC("ToggleShield", RpcTarget.All, true);
        yield return new WaitForSeconds(duration);
        view.RPC("ToggleShield", RpcTarget.All, false);
        yield return SkillCoolDown(
            shieldCoolDownTime,
            () => { _shieldCD = true; },
            () => { _shieldCD = false; }
        );


    }
    public IEnumerator SkillCoolDown(float cooldownDuration, Action preCoolDownAction = null, Action postCooldownAction = null)
    {
        preCoolDownAction?.Invoke();
        yield return new WaitForSeconds(cooldownDuration);
        postCooldownAction?.Invoke();
    }
    public void Push()
    {
        OnPushSkillUse?.Invoke(this, null);
        if (IsGrounded())
            rb.velocity = Vector2.zero; // do this for better ground kick
        animationState = AnimationState.Kicking;
        StartCoroutine(SkillCoolDown(
              pushCoolDownTime,
              () => { _pushCD = true; },
              () => { _pushCD = false; }
          ));
    }
    [PunRPC]
    public void SlowDown(float duration, float slowPercent)
    {
        StartCoroutine(GetSlow(duration, slowPercent));
    }
    IEnumerator GetSlow(float duration, float slowPercent)
    {
        speed -= _baseSpeed * Mathf.Clamp(slowPercent, 0f, 1f);
        speed = Mathf.Clamp(speed, 0.1f, speed); //0.1f min speed
        yield return new WaitForSeconds(duration);
        speed = _baseSpeed;
    }
    [PunRPC]
    public void SpeedBoost(float duration, float speedBoostPercent)
    {
        StartCoroutine(IncreaseSpeed(duration, speedBoostPercent));
    }
    IEnumerator IncreaseSpeed(float duration, float speedBoostPercent)
    {
        _speedBoostEffect.gameObject.SetActive(true);
        speed += _baseSpeed * speedBoostPercent;
        yield return new WaitForSeconds(duration);
        speed = _baseSpeed;
        _speedBoostEffect.gameObject.SetActive(false);

    }
    [PunRPC]
    public void ToggleDashGhostEffect(bool value)
    {
        _ghostEffect.enabled = value;
    }
    [PunRPC]
    public void WinGame(string playerName)
    {
        GameMananger.instance.EndGame(playerName);
    }
    [PunRPC]
    public void LockMovement(bool value, Vector3 position)
    {
        lockMovement = value;
        rb.velocity = Vector2.zero;
        if (position != null)
        {
            transform.position = position;
        }
    }
    [PunRPC]
    public void JumpDustEffect()
    {
        AudioManager.instance.PlaySound(AudioManager.Sound.JumpSFX, transform.position);
        _jumpDust.Play();
    }
}
