using LowEngine.Audio;
using LowEngine.Helpers;
using UnityEngine;

namespace LowEngine
{
    /// <summary>
    /// Allows for both arial and ground movement, including a ground pound, and can feed fall damage.
    /// </summary>
    public class Movement : MonoBehaviour, IMovement
    {
        #region Visable in editor

        [Space]
        [SerializeField] [Tooltip("The layers that the ground check will detect.")] private LayerMask Collisions = 0;

        [Header("Horizontal movement")]
        [SerializeField] [Tooltip("Walk speed.")] private float GroundedHorizontalSpeed = 5;

        [SerializeField] [Tooltip("Run speed.")] private float RunSpeed = 5;

        [SerializeField] [Tooltip("Ability to move while in the air.")] private float AirialHorizontalSpeed = 2;

        [SerializeField] private float DashSpeed = 8;

        [SerializeField] private float timeToDoubleTap = 0.2f;

        [Header("Vertical movement")]
        [SerializeField] [Tooltip("Pretty self explanatory.")] private float JumpHeight = 10;

        [SerializeField] [Tooltip("Distance this is able to fall before taking damage.")] private float MaxFallDistance = 10;

        [SerializeField] [Tooltip("The distance at which the ground will be found.")] private float groundCheckDist = 0.2f;

        [SerializeField] [Tooltip("Time the player can fall before the game will reset them.")] private float timeBeforeResetting = 3;

        [Header("Velicity Managment")]
        [SerializeField] [Range(0.05f, 1f)] private float accelerationSpeed = 0.6f;

        [SerializeField] [Range(0.05f, 1f)] private float decelerationSpeed = 0.2f;

        [SerializeField] [Tooltip("Maximum velocity the player can reach.")] private Vector2 maxSpeed = new Vector2(10, 25);

        [SerializeField] [Tooltip("Gravity multipler while falling.")] private float fallMultiplier = 3f;
        [SerializeField] [Tooltip("Gravity multipler after releasing jump button early.")] private float lowJumpMultiplier = 2.5f;

        #endregion Visable in editor

        #region Invisable in editor

        private float timeStuck;

        public TakeDamage fallDamageCallback { get; set; }

        private float fallDamage = 0;

        private float timeInAir = 0;

        private Vector3 LastSafeLocation;

        new private Rigidbody2D rigidbody;

        private bool double_jumped = false;

        private bool grounded = false;

        private bool upPressed = false;

        private bool Up => (input.y > 0 || input.z > 0);

        private bool Down => (input.z < 0);

        private int LastInput;

        private float firstTap;

        private CapsuleCollider2D capsuleCollider => GetComponent<CapsuleCollider2D>();

        public Vector4 input { get; set; }

        Vector2 IMovement.maxSpeed
        {
            get
            {
                return new Vector2(RunSpeed, maxSpeed.y);
            }
        }

        public Vector2 currentSpeed { get; set; }

        private bool dt_ref;

        private bool doubleTapped
        {
            get
            {
                int H = (int)input.x;

                if (Mathf.Abs(H) == 1 && LastInput == 0)
                {
                    firstTap = Time.time + timeToDoubleTap;

                    LastInput = H;
                }
                else
                if (Mathf.Abs(H) == 1 && LastInput != 0)
                {
                    if (H == LastInput)
                    {
                        if (timeSinceLastTap > Time.time && !dashed)
                        {
                            dt_ref = true;
                        }
                    }
                }

                if (LastInput != 0 && H == 0 && firstTap > Time.time)
                {
                    timeSinceLastTap = Time.time + timeToDoubleTap;
                }
                else
                if (firstTap < Time.time)
                {
                    timeSinceLastTap = 0;
                    firstTap = 0;
                }

                if ((LastInput != 0 && timeSinceLastTap < Time.deltaTime && firstTap < Time.time) || dashed == true)
                {
                    LastInput = 0;
                    timeSinceLastTap = 0;
                    firstTap = 0;
                    dt_ref = false;
                }

                return dt_ref;
            }
        }

        private float timeSinceLastTap = 0;

        [HideInInspector] public bool dashed = false;

        [HideInInspector] public bool running = false;

        private float timeSinceDash;

        #endregion Invisable in editor

        private void OnEnable()
        {
            #region Rigidbody check.

            LastSafeLocation = transform.position;

            rigidbody = GetComponent<Rigidbody2D>();

            //Error check.
            if (rigidbody == null)
            {
                Debug.LogError("Warning no Rigidbody connected to player object... Attempting to add one now.\nPlease inform your developer of this error");

                rigidbody = gameObject.AddComponent<Rigidbody2D>();
            }

            #endregion Rigidbody check.
        }

        private Vector2 currentVelocity;

        private void FixedUpdate()
        {
            //Get the current velocity
            currentVelocity = rigidbody.velocity;

            currentSpeed = currentVelocity;

            GroundMovement();

            if (currentVelocity == Vector2.zero && input != Vector4.zero)
            {
                timeStuck += Time.deltaTime;
            }
            else
            {
                timeStuck = 0;
            }

            if (timeStuck > 0.5f)
            {
                Debug.Log("Moving player to last known safe location.");

                transform.position = LastSafeLocation;
            }
        }

        private void GroundMovement()
        {
            #region Grounded detection

            //Draw a cone down
            var skinWidth = 0.1f;
            var distDown = MaxFallDistance * 2f + Mathf.Abs(currentVelocity.y);

            var dist = groundCheckDist + (capsuleCollider.size.y / 2 + (capsuleCollider.offset.y + skinWidth));

            var right = transform.position + (Vector3.down * (capsuleCollider.size.y / 2 + (capsuleCollider.offset.y - skinWidth))) + (Vector3.right * capsuleCollider.size.x);
            var left = transform.position + (Vector3.down * (capsuleCollider.size.y / 2 + (capsuleCollider.offset.y - skinWidth))) + (Vector3.left * capsuleCollider.size.x);
            var drawToPos = transform.position + (Vector3.down * distDown);
            RaycastHit2D hitGround_Right = Physics2D.Linecast(right, drawToPos, Collisions);
            RaycastHit2D hitGround_Left = Physics2D.Linecast(left, drawToPos, Collisions);

            Debug.DrawLine(left, drawToPos, hitGround_Left.distance < MaxFallDistance ? Color.blue : Color.red);
            Debug.DrawLine(right, drawToPos, hitGround_Right.distance < MaxFallDistance ? Color.blue : Color.red);

            float distanceRight = hitGround_Right.transform != null ? hitGround_Right.distance : MaxFallDistance;

            float distanceLeft = hitGround_Left.transform != null ? hitGround_Left.distance : MaxFallDistance;

            float distanceToGround = distanceLeft <= dist ? distanceLeft : distanceRight;

            if (distanceToGround < MaxFallDistance)
            {
                grounded = distanceToGround <= dist;
            }
            else
            {
                //Just encase the player jumps off a ledge be sure that grounded == false.
                grounded = false;

                float dam = (distanceToGround - MaxFallDistance);

                //Set how much fall damage to take and make sure that fall damage has not already been set.
                //Also check velocity and ensure that matches the fall distance.
                if (fallDamage < dam && Mathf.Abs(rigidbody.velocity.y) >= Mathf.Abs(dam))
                {
                    if (timeInAir > 0.5f)
                        fallDamage = dam;
                }
            }

            #endregion Grounded detection

            //Hold the players desired velocity, plus the current velocity.
            var dVX = (input.x != 0) ? (currentVelocity.x + (input.x * (grounded ? (running ? RunSpeed : GroundedHorizontalSpeed) : AirialHorizontalSpeed))) : (grounded ? 0 : currentVelocity.x);

            //Move the current velocity towards the desired one.
            currentVelocity.x = (input.x != 0) ? Mathf.Lerp(currentVelocity.x, dVX, accelerationSpeed) : Mathf.Lerp(currentVelocity.x, dVX, decelerationSpeed);

            #region Jump / Double Jump / Ground Pound

            //Detect if the player has hit jump/up.

            if (input.y == 0 && input.z == 0) upPressed = false;

            if (Up && !upPressed)
            {
                upPressed = true;

                //Jump
                if (grounded)
                {
                    currentVelocity.y = JumpHeight;

                    AudioManager.instance.PlayJumpSound(transform.position);
                }
                else
                if (double_jumped == false)
                {
                    double_jumped = true;

                    currentVelocity.y = JumpHeight;
                }
            }

            if (currentVelocity.y < 0) // Falling
            {
                currentVelocity.y += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }

            if (currentVelocity.y > 0 && !upPressed) // Released jump early
            {
                currentVelocity.y += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //Detect if the player has hit down.
            if (Down && !grounded)
            {
                //Launch the player towards the ground.
                currentVelocity.y += -JumpHeight * (distanceToGround / 2);
            }

            if (grounded)
            {
                running = input.w != 0;

                double_jumped = false;

                if (hitGround_Right.transform != null && hitGround_Left.transform != null)
                {
                    // The distance to the ground is equal on both edges.
                    if (Mathf.Round(distanceRight) == Mathf.Round(distanceLeft) && currentSpeed == Vector2.zero)
                    {
                        if (Vector2.Distance(LastSafeLocation, transform.position) > 2)
                            LastSafeLocation = transform.position;
                    }
                }

                timeInAir = 0;

                if (fallDamage > 0)
                {
                    //Damage the player.

                    if (fallDamageCallback != null) // Check if any one has subscribed to the falldamage callback.
                    {
                        Debug.Log("Dealing fall damage... " + fallDamage);

                        fallDamageCallback.Invoke(fallDamage);
                    }
                    else
                    {
                        Debug.LogError($"Unable to deal fall damage of: {fallDamage}. Reason: fallDamageCallback subscribers empty.");
                    }

                    //Set fall damage to 0 so we don't try to hurt the player again.
                    fallDamage = 0;
                }
            }
            else
            {
                if (hitGround_Right.transform == null && hitGround_Left.transform == null) timeInAir += Time.deltaTime;

                if (timeInAir >= timeBeforeResetting)
                {
                    //Reset the character to the last safe place they were standing.
                    transform.position = LastSafeLocation;

                    currentVelocity = Vector2.zero;

                    Debug.Log("Moving player to last known safe location.");
                }
            }

            #endregion Jump / Double Jump / Ground Pound

            //Clamp the velocity so that the player doesnt get stuck in walls.

            maxSpeed.x = running ? RunSpeed : GroundedHorizontalSpeed;

            if (dashed == false)
            {
                currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed.x, maxSpeed.x);
                currentVelocity.y = Mathf.Clamp(currentVelocity.y, -maxSpeed.y, maxSpeed.y);
            }

            if (doubleTapped) // Check for double tap
            {
                float distanceToWall()
                {
                    Vector3 faceDir = new Vector3(input.x, 0); // Generate a cast direction for the ray
                    float rayDistance = DashSpeed; // How far to send the ray out.

                    Vector3 start = transform.position + faceDir;
                    Vector3 direction = faceDir;

                    RaycastHit2D hitInfo = Physics2D.Raycast(start, direction, rayDistance, LayerMask.GetMask("Default")); // Store some ray info.

                    Debug.DrawRay(start, direction * (hitInfo.transform != null ? hitInfo.distance : rayDistance), hitInfo.transform != null ? Color.red : Color.white);

                    return (hitInfo.transform != null ? hitInfo.distance : 0);
                }

                if (!dashed && (distanceToWall() > 2 || distanceToWall() == 0))
                {
                    dashed = true;

                    float spd = (DashSpeed * ((DashSpeed - distanceToWall()) / DashSpeed));

                    currentVelocity.x = (Mathf.Abs(currentVelocity.x) * input.x) * spd;

                    timeSinceDash = Time.time + 0.2f;

                    //AudioManager.instance.PlayDashSound(transform.position);
                }
            }

            //Wait for the player to slow down before allowing another dash.
            if (dashed && Mathf.Abs(currentVelocity.x) <= maxSpeed.x || timeSinceDash < Time.time)
            {
                dashed = false;
            }

            //Apply the velocity.
            rigidbody.velocity = currentVelocity;
        }

        #region Collision Detection

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (dashed) dashed = false;
        }

        #endregion Collision Detection
    }
}