using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation; // WHY 
using Photon.Pun;
using Photon.Realtime;
using System;
using static CustomPropertiesConstant;
using TMPro;
public class PUNPlayerController : MonoBehaviourPunCallbacks
{
    // ------------------------------------ ANIMATION VARIABLES ------------------------------------ // 
    public SpriteLibrary spriteLibrary;
    [SerializeField] private Animator _animator;
    [SerializeField] AnimationState _animationState;
    // ------------------------------------ STATS VALUE ------------------------------------ // 
    public float speed = .2f;
    public float jumpSpeed = .2f;
    private float baseSpeed;
    private float _directionX;
    private bool _isShielded;
    [SerializeField] private bool _pushCD;
    [SerializeField] private bool _shieldCD;
    [SerializeField] public float pushCoolDownTime;
    [SerializeField] public float shieldCoolDownTime;

    [SerializeField] public float _shieldDuration;
    // ------------------------------------ PHYSIC VARIABLES ------------------------------------ // 
    private Rigidbody2D rb;
    private Collider2D collider2D;
    public LayerMask grounds;

    // ------------------------------------ NETWORK VARIABLES ------------------------------------ // 
    public PhotonView view;
    public int skinIndex; // obsolete
    public ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Player player;
    [SerializeField] TextMeshPro nameTagText;
    // ------------------------------------ SKILL AND POWERUP ------------------------------------ // 
    [SerializeField] PlayerPowerUpController playerPowerUpController;
    [SerializeField] GameObject shieldObject;

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

    public Rigidbody2D Rb { get => rb; set => rb = value; }

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
        spriteLibrary = GetComponent<SpriteLibrary>();
        if (_animator == null)
            _animator = GetComponent<Animator>();
        animationState = AnimationState.Idling;
        Rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        playerPowerUpController = GetComponent<PlayerPowerUpController>();
        player = view.Owner;
        baseSpeed = speed;
    }
    private void Start()
    {
        if (!view.IsMine)
        {
            view.RPC("SyncData", RpcTarget.All);
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
            Move();
    }
    public void ProcessInput()
    {
        // get horizontal direction
        _directionX = Input.GetAxisRaw("Horizontal");
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            Rb.velocity = Vector2.up * jumpSpeed * 3;
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
    public void Move()
    {
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
        if (other.gameObject.tag == "Kill Zone")
        {
            rb.velocity = Vector2.zero;
            transform.position = spawnPos;
        }
    }
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
        GetComponent<SpriteLibrary>().spriteLibraryAsset = GameMananger.instance.CharacterSpriteLibraryAssets[skinIndex];
    }
    public void SetData(string name, int skinIndex)
    {
        // sync change maybe ...
        playerProperties[SKIN_INDEX] = skinIndex;
        playerProperties[PLAYER_NAME] = name;
        // local change
        nameTagText.text = name;
        GetComponent<SpriteLibrary>().spriteLibraryAsset = GameMananger.instance.CharacterSpriteLibraryAssets[skinIndex];
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    [PunRPC]
    public void SyncData()
    {
        GetComponent<SpriteLibrary>().spriteLibraryAsset =
            GameMananger.instance.CharacterSpriteLibraryAssets[(int)view.Owner.CustomProperties[SKIN_INDEX]];
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
        speed -= baseSpeed * Mathf.Clamp(slowPercent, 0f, 1f);
        speed = Mathf.Clamp(speed, 0.1f, speed); //0.1f min speed
        yield return new WaitForSeconds(duration);
        speed = baseSpeed;
    }
    [PunRPC]
    public void SpeedBoost(float duration, float speedBoostPercent)
    {
        StartCoroutine(IncreaseSpeed(duration, speedBoostPercent));
    }
    IEnumerator IncreaseSpeed(float duration, float speedBoostPercent)
    {
        speed += baseSpeed * speedBoostPercent;
        yield return new WaitForSeconds(duration);
        speed = baseSpeed;

    }
}
