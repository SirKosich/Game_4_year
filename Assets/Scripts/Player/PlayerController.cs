using SimpleMedievalPlatformer.Systems;
using UnityEngine;

namespace SimpleMedievalPlatformer.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private bool allowDoubleJump = true;
        [SerializeField] private float groundCheckRadius = 0.18f;
        [SerializeField] private LayerMask groundMask = ~0;

        private Rigidbody2D rb;
        private BoxCollider2D bodyCollider;
        private SpriteRenderer spriteRenderer;
        private Transform groundCheck;
        private PlayerCombat combat;
        private float touchMoveInput;
        private bool jumpQueued;
        private bool canDoubleJump;
        private bool controlsLocked;
        private bool facingRight = true;
        private bool wasGrounded;

        public bool FacingRight => facingRight;
        public Collider2D BodyCollider => bodyCollider;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 3f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            bodyCollider = GetComponent<BoxCollider2D>();
            if (bodyCollider == null)
            {
                bodyCollider = gameObject.AddComponent<BoxCollider2D>();
            }

            bodyCollider.size = new Vector2(0.8f, 1.2f);
            bodyCollider.offset = new Vector2(0f, 0f);

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Square);
            spriteRenderer.color = new Color(0.23f, 0.54f, 0.96f);

            transform.localScale = new Vector3(0.8f, 1.2f, 1f);

            combat = GetComponent<PlayerCombat>();
            if (combat == null)
            {
                combat = gameObject.AddComponent<PlayerCombat>();
            }

            if (groundCheck == null)
            {
                GameObject groundCheckRoot = new GameObject("GroundCheck");
                groundCheckRoot.transform.SetParent(transform, false);
                groundCheckRoot.transform.localPosition = new Vector3(0f, -0.65f, 0f);
                groundCheck = groundCheckRoot.transform;
            }
        }

        private void Update()
        {
            bool grounded = IsGrounded();

            if (grounded && !wasGrounded)
            {
                canDoubleJump = allowDoubleJump;
            }

            wasGrounded = grounded;

            if (!Application.isMobilePlatform || Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PressJump();
                }

                if (Input.GetKeyDown(KeyCode.J))
                {
                    PressAttack();
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    SwitchWeapon();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameManager.Instance.TogglePause();
                }
            }

            if (GameManager.Instance != null && GameManager.Instance.IsPaused)
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                return;
            }
        }

        private void FixedUpdate()
        {
            if (controlsLocked)
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                return;
            }

            float keyboardInput = 0f;
            if (!Application.isMobilePlatform || Application.isEditor)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    keyboardInput = -1f;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    keyboardInput = 1f;
                }
            }

            float moveInput = Mathf.Abs(touchMoveInput) > 0.01f ? touchMoveInput : keyboardInput;
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (moveInput > 0.01f && !facingRight)
            {
                SetFacing(true);
            }
            else if (moveInput < -0.01f && facingRight)
            {
                SetFacing(false);
            }

            if (jumpQueued)
            {
                bool grounded = IsGrounded();

                if (grounded)
                {
                    PerformJump();
                }
                else if (allowDoubleJump && canDoubleJump)
                {
                    PerformJump();
                    canDoubleJump = false;
                }

                jumpQueued = false;
            }
        }

        private void PerformJump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask) != null;
        }

        private void SetFacing(bool faceRight)
        {
            facingRight = faceRight;
            spriteRenderer.flipX = !faceRight;
        }

        public void SetTouchMove(float value)
        {
            touchMoveInput = Mathf.Clamp(value, -1f, 1f);
        }

        public void ClearTouchMove()
        {
            touchMoveInput = 0f;
        }

        public void PressJump()
        {
            jumpQueued = true;
        }

        public void PressAttack()
        {
            if (combat != null)
            {
                combat.TryAttack(facingRight ? Vector2.right : Vector2.left);
            }
        }

        public void SwitchWeapon()
        {
            if (combat != null)
            {
                combat.SwitchWeapon();
            }
        }

        public void SetControlsEnabled(bool enabled)
        {
            controlsLocked = !enabled;
            if (!enabled)
            {
                ClearTouchMove();
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
