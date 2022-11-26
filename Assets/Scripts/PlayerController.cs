using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Photon.Bolt;
//using UnityEngine.Experimental.U2D.Animation; // WHY 

// public class PlayerController :  EntityEventListener<ICustomSquareState>
public class PlayerControlle : MonoBehaviour
{
    //     [SerializeField] private string playerID;
    //     public float speed = .2f;
    //     private float directionX;
    //     public SpriteLibrary spriteLibrary;
    //     public LayerMask grounds;
    //     [SerializeField] private Animator _animator;
    //     private Rigidbody2D rb;
    //     private Collider2D collider2D;
    //     [SerializeField] AnimationState _animationState;

    //     public AnimationState animationState
    //     {
    //         get { return _animationState; }
    //         set
    //         {
    //             _animationState = value;
    //             OnAnimationStateChange();
    //         }
    //     }

    //     public Rigidbody2D Rb { get => rb; set => rb = value; }

    //     public enum AnimationState
    //     {
    //         Idling,
    //         Running,
    //         Kicking,
    //         Hurting
    //     }
    // public override void Attached()
    // {
    //     // local variable
    //     collider2D = GetComponent<Collider2D>();
    //     spriteLibrary = GetComponent<SpriteLibrary>();
    //     if (_animator == null)
    //         _animator = GetComponent<Animator>();
    //     animationState = AnimationState.Idling;
    //     Rb = GetComponent<Rigidbody2D>();

    //     // network variable
    //     state.SetTransforms(state.SquareTransform, transform);
    //     state.SetAnimator(_animator);
    //     state.AnimationState = (int)animationState;
    //     state.AddCallback("AnimationState", AnimationStateCallBack);

    //     playerID = entity.GetInstanceID().ToString();
    //     base.Attached();
    // }
    // public override void SimulateOwner()
    // {
    //     ProcessInput();
    //     base.SimulateOwner();
    // }
    // private void Update()
    // {
    //     // Debug.Log(entity.IsOwner);
    //     if (entity.IsOwner && !_animator.GetBool("IsHurting"))
    //         ProcessInput();
    // }
    // private void FixedUpdate()
    // {
    //     if (!_animator.GetBool("IsHurting"))
    //         Move();
    // }
    // public void ProcessInput()
    // {
    //     // get horizontal direction
    //     directionX = Input.GetAxisRaw("Horizontal");
    //     if (IsGrounded() && Input.GetButtonDown("Jump"))
    //     {
    //         Rb.velocity = Vector2.up * speed * 3;
    //     }

    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         var evnt = PlayerTakeDamage.Create(entity);
    //         evnt.Message = "id : " + entity.GetInstanceID() + " take damage";
    //         evnt.Send();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         animationState = AnimationState.Kicking;
    //         // _animator.SetTrigger("Kick");
    //         // state.Animator.SetTrigger("Kick");
    //     }
    // }
    // public void Move()
    // {
    //     // flip character sprite
    //     if (animationState != AnimationState.Kicking)
    //     {
    //         if (directionX != 0)
    //         {
    //             // transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
    //             transform.rotation = Quaternion.Euler(0f, directionX > 0 ? 0 : 180f, 0f);
    //             animationState = AnimationState.Running;
    //         }
    //         else if (animationState != AnimationState.Idling)
    //         {
    //             animationState = AnimationState.Idling;
    //         }
    //     }
    //     transform.position += (Vector3)new Vector2(directionX * speed * Time.fixedDeltaTime, 0);
    // }
    // public void AnimationStateCallBack()
    // {
    //     // Debug.Log("state animation state changed!");
    //     foreach (AnimatorControllerParameter parameter in state.Animator.parameters)
    //     {
    //         if (parameter.type == AnimatorControllerParameterType.Bool)
    //             state.Animator.SetBool(parameter.name, false);
    //     }

    //     switch (state.AnimationState)
    //     {
    //         case (int)AnimationState.Idling:
    //             state.Animator.SetBool("IsIdling", true);
    //             break;
    //         case (int)AnimationState.Running:
    //             state.Animator.SetBool("IsRunning", true);
    //             break;
    //         case (int)AnimationState.Kicking:
    //             _animator.SetBool("IsKicking",true);
    //             break;
    //         case (int)AnimationState.Hurting:
    //             state.Animator.SetBool("IsHurting", true);
    //             break;
    //         default:
    //             break;
    //     }
    // }
    // public virtual void OnAnimationStateChange()
    // {
    //     // Debug.Log("animation state changed!");
    //     state.AnimationState = (int)animationState;
    //     foreach (AnimatorControllerParameter parameter in _animator.parameters)
    //     {
    //         if (parameter.type == AnimatorControllerParameterType.Bool)
    //             _animator.SetBool(parameter.name, false);
    //     }

    //     switch (_animationState)
    //     {
    //         case AnimationState.Idling:
    //             // local
    //             // _animator.SetBool("IsRunning", false);
    //             _animator.SetBool("IsIdling", true);
    //             break;
    //         case AnimationState.Running:
    //             // local
    //             _animator.SetBool("IsRunning", true);
    //             break;
    //         case AnimationState.Kicking:
    //             break;
    //         case AnimationState.Hurting:
    //             _animator.SetBool("IsHurting", true);
    //             break;
    //         default:
    //             throw new System.Exception("Null animation state exception");
    //     }
    // }
    // public bool IsGrounded() // BAD
    // {
    //     float extendRayCastDistance = 0.15f;
    //     RaycastHit2D raycastHit = Physics2D.BoxCast(
    //         collider2D.bounds.center,
    //         collider2D.bounds.size,
    //         0f,
    //         Vector2.down,
    //         extendRayCastDistance,
    //         grounds
    //     );

    //     // debug
    //     Color rayColor = Color.green;
    //     if (raycastHit.collider == null)
    //         rayColor = Color.red;
    //     Debug.DrawRay(
    //         collider2D.bounds.center + new Vector3(collider2D.bounds.extents.x, 0),
    //         Vector2.down * (collider2D.bounds.extents.y + extendRayCastDistance),
    //         rayColor
    //     );
    //     Debug.DrawRay(
    //         collider2D.bounds.center - new Vector3(collider2D.bounds.extents.x, 0),
    //         Vector2.down * (collider2D.bounds.extents.y + extendRayCastDistance),
    //         rayColor
    //     );
    //     Debug.DrawRay(
    //       collider2D.bounds.center - new Vector3(collider2D.bounds.extents.x, collider2D.bounds.extents.y + extendRayCastDistance),
    //       Vector2.right * collider2D.bounds.size.x,
    //       rayColor
    //     );
    //     return raycastHit.collider != null;
    // }
    // public override void OnEvent(PlayerTakeDamage evnt) // doesn't work as intended
    // {

    //     base.OnEvent(evnt);
    //     animationState = AnimationState.Hurting;
    //     state.AnimationState = (int)AnimationState.Hurting;
    //     rb.velocity = evnt.KnockBackDirection;
    //     // state.Animator.SetBool("IsHurting", true);
    //     // state.Animator.SetBool("IsIdling", false);

    //     // _animator.SetBool("IsHurting", true);
    //     // _animator.SetBool("IsIdling", false);

    //     // Debug.Log(_animator.GetBool("IsHurting"));
    //     // Debug.Log(state.Animator.GetBool("IsHurting"));
    //     // Debug.Log(evnt.Message);

    // }
    // public void StopInvulnerable()
    // {
    //     // Debug.Log("stop vulnerable func call");
    //     ResetAnimationStateToIdle();
    //     rb.velocity = Vector2.zero;
    // }
    // public void ResetAnimationStateToIdle()
    // {
    //     animationState = AnimationState.Idling;
    //     state.AnimationState = (int)animationState;
    // }
    // public override void OnEvent(HitEvent evnt)
    // {

    // }
}
