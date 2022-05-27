using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace MCC.Motors
{
	/// <summary>
	/// This is the basic settings for the the movement.
	/// </summary>
	#pragma warning disable CS0649
	[Serializable]
	[CreateAssetMenu(fileName = "Movement Settings", menuName = "MCC/Movement Settings")]
	public class MovementSettings : ScriptableObject
	{
		/// <summary>
		/// The amount of force used to make the player jump
		/// </summary>
		public float JumpForce => jumpForce;
		
		/// <summary>
		/// The amount of gravity to apply when the player is falling normally
		/// </summary>
		public float FallMultiplier => fallMultiplier - 1;
		
		/// <summary>
		/// The amount of gravity to apply when the player is rising and not pressing the jump button
		/// </summary>
		public float LowJumpMultiplier => lowJumpMultiplier - 1;

		/// <summary>
		/// The layers to check against when raycasting.
		/// </summary>
		public LayerMask LayerChecks => layerChecks;
		/// <summary>
		/// Virtual function for the groundTag.
		/// </summary>
		public string GroundTag => groundTag;
		/// <summary>
		/// Virtual function for the wallTag.
		/// </summary>
		public string WallTag => wallTag;

		/// <summary>
		/// Gets the maximum speed of the player depending if they are in the air or not.
		/// </summary>
		/// <param name="_isGrounded">Whether or not the player is in the air.</param>
		public float GetMaxSpeed(bool _isGrounded = false) => individualMaxSpeeds ? _isGrounded ? maxSpeedOnGround : maxSpeedInAir : maxSpeed;

		/// <summary>
		/// Gets the distance of the ray depending if they are in the air or not.
		/// </summary>
		/// <param name="_isGrounded">Whether or not the player is in the air.</param>
		public float GetGroundDistanceCheck(bool _isGrounded = false) => _isGrounded ? groundDistanceCheck : groundDistanceInAirCheck;
		
		/// <summary>
		/// The movement action Vector2.
		/// </summary>
		public InputAction MoveAction => moveAction;
		
		/// <summary>
		/// The jump action Button.
		/// </summary>
		public InputAction JumpAction => jumpAction;
		
		/// <summary>
		/// The force the player is to jump at.
		/// </summary>
		[SerializeField, Range(1, 20)] private float jumpForce;

		/// <summary>
		/// Is there different max speeds on different situations.
		/// </summary>
		[SerializeField] private bool individualMaxSpeeds = true;
		/// <summary>
		/// The overall max speed.
		/// </summary>
		[SerializeField, Range(1, 20)] private float maxSpeed;
		/// <summary>
		/// Max speed on the ground *(if different from maxSpeed).
		/// </summary>
		[SerializeField, Range(1, 20)] private float maxSpeedOnGround;
		/// <summary>
		/// The max speed in the air if different to the normal max speed. 
		/// </summary>
		[SerializeField, Range(1, 20)] private float maxSpeedInAir;

		/// <summary>
		/// How far away the player checks it is next to the ground for ground checks (if on the ground).
		/// </summary>
		[SerializeField, Min(0.1f)] private float groundDistanceCheck = 1;
		/// <summary>
		/// How far away the player checks it is next to the ground for ground checks (if in the air).
		/// </summary>
		[SerializeField, Min(0.2f)] private float groundDistanceInAirCheck = 0.2f;

		/// <summary>
		/// The mutiplier the player will fall at when they reach the top of the jump.
		/// </summary>
		[SerializeField, Min(0.1f)] private float fallMultiplier = 2.5f;
		/// <summary>
		/// Mutiplier the player falls at when they don't hold down the jump for the full time.
		/// </summary>
		[SerializeField, Min(0.2f)] private float lowJumpMultiplier = 2f;

		/// <summary>
		/// The layers to check against.
		/// </summary>
		[SerializeField] private LayerMask layerChecks;
		/// <summary>
		/// The serialized ground tag.
		/// </summary>
		[SerializeField] [Tooltip("Make sure to add a Tag and Layer to the project")] private string groundTag;
		/// <summary>
		/// The serialized wall tag.
		/// </summary>
		[SerializeField] [Tooltip("Make sure to add a Tag and Layer to the project")]private string wallTag;

		/// <summary>
		/// The reference in the Input to the move Vector 2 action.
		/// </summary>
		[SerializeField] private InputActionReference moveAction;
		/// <summary>
		/// The reference in the Input to the jump Button action.
		/// </summary>
		[SerializeField] private InputActionReference jumpAction;
	}
}