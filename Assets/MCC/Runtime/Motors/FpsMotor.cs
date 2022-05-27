using MCC.Cameras;

using UnityEngine;
using UnityEngine.InputSystem;

namespace MCC.Motors
{
	/// <summary>
	/// This is an FPS Motor for FPS movement.
	/// This is derived from ModularBehaviour.
	/// </summary>
	public class FpsMotor : ModularBehaviour
	{
		/// <summary>
		/// Is the player touching a "ground" layer.
		/// </summary>
		public bool IsGrounded { get; private set; }
		
		/// <summary>
		/// The Rigidbody of the player.
		/// </summary>
		public Rigidbody Rigidbody { get; private set; }

		/// <summary>
		/// The scriptable object containing all movement variables.
		/// </summary>
		[SerializeField] private MovementSettings settings;
		
		/// <summary>
		/// This is a CapsuleCollider of the player. 
		/// </summary>
		private new CapsuleCollider collider;
		/// <summary>
		/// The player transform.
		/// </summary>
		private Transform player;
		/// <summary>
		/// Reference to the players Camera. 
		/// </summary>
		private new FpsCamera camera;

		/// <summary>
		/// This is constant for the speed mutiplier on the ground.
		/// </summary>
		private const float SPEED_ON_GROUND_MODIFIER = 1;
		/// <summary>
		/// This is constant for the speed mutiplier in the air.
		/// </summary>
		private const float SPEED_IN_AIR_MODIFIER = 1;
		/// <summary>
		/// How often the player checks they are on the ground.
		/// </summary>
		private const float IN_AIR_GROUND_CHECK_DELAY = 0.2f;
		/// <summary>
		/// This is used for Coyote jumps.
		/// </summary>
		private float lastTimeInAir;
		/// <summary>
		/// Is the jump button pressed.
		/// </summary>
		private bool isJumpPressed;
		
		/// <summary>
		/// What to do when this component is Initialised.
		/// </summary>
		/// <param name="_player"> Pass in the player this component is being initialised to.</param>
		public override void Init(MCCPlayer _player)
		{
			// Making sure this component has the correct reference to the player's Rigidbody.
			Rigidbody = _player.Rigidbody;
			// Making sure this component has a correct reference to the player's CapsualCollider.
			collider = (CapsuleCollider) _player.Collider;
			// Making sure this component has a correct reference to the player's Transform.
			player = _player.transform;
			// If the player has an FpsCamera component on it.
			if(_player.TryGetBehaviour(out FpsCamera cam))
				// Get an internal reference to it.
				camera = cam;
			// Enable the move action in the Movement settings as we are using it.
			settings.MoveAction.Enable();
			// Enable the jump action in the Movement settings as we are using it.
            settings.JumpAction.Enable();
            // Add the "OnJumpPerformed" to the JumpAction delegate (if the player pushes down on it).
			settings.JumpAction.performed += OnJumpPerformed;
			// Add the "OnJumpCanceled" to the JumpAction canceled delegate (if the player let goes).
			settings.JumpAction.canceled += OnJumpCanceled;
		}

		/// <summary>
		/// This is the Processes that need to be run.
		// TODO Make sure this component is in "Fixed Update"!!!
		/// </summary>
		protected override void OnProcess()
		{
			// Do a ground check to check if the player is grounded.
			CheckGrounded();
			// Read the current movement value of the player's Vector2 movement input.
			HandleMovement(settings.MoveAction.ReadValue<Vector2>());
			// Apply extra gravity if the player needs it.
			ApplyExtraGravity();
		}

		/// <summary>
		/// Detect if the playermotor is actually grounded; ie; not jumping or over nothing
		/// </summary>
		private void CheckGrounded()
		{
			// Use a smaller ground distance check if in air, to prevent suddenly snapping to ground
			float chosenGroundCheckDistance = settings.GetGroundDistanceCheck(IsGrounded);

			// Check if the current time is greater than the required jump check time.
			if(Time.time >= lastTimeInAir + IN_AIR_GROUND_CHECK_DELAY)
			{
				// Get all layers below us in the correct distance
				RaycastHit[] hits = CapsuleCastAllInDirection(-player.up, chosenGroundCheckDistance);

				// If there is actually anything in the array, we can loop through it, otherwise, we aren't grounded.
				if(hits.Length > 0)
				{
					foreach(RaycastHit hit in hits)
					{
						// Check if we are touching the ground, if so, set grounded to true and return.
						if(hit.transform.CompareTag(settings.GroundTag) || hit.transform.gameObject.layer == 0)
						{
							IsGrounded = true;
							return;
						}
					}
				}
				else
				{
					// We are standing above nothing so we aren't grounded
					IsGrounded = false;
				}
			}
		}

		/// <summary>
		/// Handles the movement of the player by the axis of the moveAction.
		/// </summary>
		/// <param name="_axis">The axis the controller or keyboard is requesting</param>
		private void HandleMovement(Vector2 _axis)
		{
			// If the camera motor isn't running right now we shouldn't be able to control the player
			if(!camera.Enabled)
				return;

			// Calculate the max speed and the speed modifier by the grounded state
			float maxSpeed = settings.GetMaxSpeed(IsGrounded);
			// If we are grounded use the groundMaxSpeed, otherwise use the airMaxSpeed.
			float modifier = IsGrounded ? SPEED_ON_GROUND_MODIFIER : SPEED_IN_AIR_MODIFIER;

			// Calculate the correct velocity by the axis of the input (these are all in Vector3 so we can calculate the final velocity).
			Vector3 forward = player.forward * _axis.y; // Foward/back Input.
			Vector3 right = player.right * _axis.x; // Since the player is on an the gound we need to change the input directions.
			Vector3 desiredVelocity = (forward + right) * (maxSpeed * modifier) - Rigidbody.velocity; // This is the velocity we want the player to be at

			Debug.Log($"desiredVelocity = {desiredVelocity}");
			// Check we can move this way, if we can apply the velocity
			if(CanMoveInDirection(desiredVelocity))
				// Addes force in the horizontal plane to 
				Rigidbody.AddForce(new Vector3(desiredVelocity.x, 0, desiredVelocity.z), ForceMode.Impulse);
		}

		/// <summary>
		/// Attempt to apply extra gravity depending on the state of the rigidbody and jump button
		/// </summary>
		private void ApplyExtraGravity()
		{
			// Check if we are falling, if we are apply normal snappy fall
			if(Rigidbody.velocity.y < 0)
			{
				Rigidbody.velocity += Vector3.up * (Physics.gravity.y * settings.FallMultiplier * Time.deltaTime);
			}
			// We are rising, but we aren't pressing the jump button, so fall faster
			else if(Rigidbody.velocity.y > 0 && !isJumpPressed)
			{
				Rigidbody.velocity += Vector3.up * (Physics.gravity.y * settings.LowJumpMultiplier * Time.deltaTime);
			}
		}

		/// <summary>
		/// To ensure the player is able to actually move in this direction and not going into an object or wall.
		/// </summary>
		/// <param name="_targetDir"> The direction the player is attempting to move in. </param>
		/// <returns> Is the player able to move in this direction. </returns>
		private bool CanMoveInDirection(Vector3 _targetDir)
		{
			// Find everything in the direction we are attempting to move at least half a meter away
			RaycastHit[] hits = CapsuleCastAllInDirection(_targetDir, 0.5f);

			// Cycle through all the objects hit in this dirction.
			foreach(RaycastHit hit in hits)
			{
				// Compares all the collider of the collider hit is a wall then 
				if(hit.collider.CompareTag(settings.WallTag) || hit.transform.gameObject.layer == 0)
				{
					// We will walk into a wall so don't move that way
					return false;
				}
			}
			// We can move this way since we won't walk into a wall
			return true;
		}

		/// <summary>
		/// Cast a Capsule in the passed direction and get all hit objects in that direction.
		/// </summary>
		/// <param name="_direction">The direction to cast the capsule</param>
		/// <param name="_distance">How far the capsule will travel</param>
		private RaycastHit[] CapsuleCastAllInDirection(Vector3 _direction, float _distance)
		{
			Vector3 top = transform.position + collider.center + Vector3.up * ((collider.height * 0.5f) - collider.radius);
			Vector3 bot = transform.position + collider.center - Vector3.up * ((collider.height * 0.5f) - collider.radius);

			// Cast the capsule in the passed direction and distance
			return Physics.CapsuleCastAll(top, bot, collider.radius * 0.95f, _direction, _distance, settings.LayerChecks);
		}

		/// <summary>
		/// Fired when the jump action is pressed.
		/// </summary>
		private void OnJumpPerformed(InputAction.CallbackContext _context)
		{
			// If the camera is not running right now, we won't jump
			if(!camera.Enabled)
				return;

			isJumpPressed = true;

			// If we are grounded jump and store the jump time
			if(IsGrounded)
			{
				Rigidbody.AddForce(Vector3.up * settings.JumpForce, ForceMode.Impulse);
				IsGrounded = false;

				lastTimeInAir = Time.time;
			}
		}
		
		/// <summary>
		/// Fired when the jump action either a) fails or b) is released
		/// </summary>
		private void OnJumpCanceled(InputAction.CallbackContext _context) => isJumpPressed = false;
	}
}