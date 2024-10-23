using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimationController : MonoBehaviour
{
    #region Animation Setup
    [Header("Animation Setup")]
    [SerializeField] private Animator animatorController;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    #endregion

    #region animation IDs
    public readonly int _animIDSpeed = Animator.StringToHash("Speed");
    public readonly int _animIDGrounded = Animator.StringToHash("Grounded");
    public readonly int _animIDJump = Animator.StringToHash("Jump");
    public readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
    public readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    public readonly int _animIDMotionShoot = Animator.StringToHash("IsShooting");
    public readonly int _animIDMotionReload = Animator.StringToHash("IsReloading");
    public readonly int _animIDMotionWeapon = Animator.StringToHash("WeaponStyle");
    public readonly int _animIDMotionDeadth = Animator.StringToHash("IsDead");
    public readonly int _animIDMotionDeadthStyle = Animator.StringToHash("DeadthStyle");
    public readonly int _animIDMotionMoveX = Animator.StringToHash("DirectionX");
    public readonly int _animIDMotionMoveY = Animator.StringToHash("DirectionY");
    #endregion

    #region private variable
    private ThirdPersonController playerController;
    private bool hasAnimator;
    #endregion

    private void Start()
    {
        hasAnimator = animatorController != null;
    }

    public void Move(Vector2 playerInput, float blend, float magnitude)
    {
        if (!hasAnimator) return;
        animatorController.SetFloat(_animIDSpeed, blend);
        animatorController.SetFloat(_animIDMotionSpeed, magnitude);
        animatorController.SetFloat(_animIDMotionMoveX, playerInput.x);
        animatorController.SetFloat(_animIDMotionMoveY, playerInput.y);
    }

    public void Jump(bool isJump = false, bool isFreefall = false)
    {
        if (!hasAnimator) return;

        animatorController.SetBool(_animIDJump, isJump);
        animatorController.SetBool(_animIDFreeFall, isFreefall);
    }

    public void Grounded(bool isGrounded = false)
    {
        if (!hasAnimator) return;

        animatorController.SetBool(_animIDGrounded, isGrounded);
    }


    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                if (playerController != null)
                {
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(playerController.Center), FootstepAudioVolume);
                }
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (playerController != null)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(playerController.Center), FootstepAudioVolume);
            }
        }
    }

}
