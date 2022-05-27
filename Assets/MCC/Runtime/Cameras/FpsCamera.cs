using UnityEngine;
using UnityEngine.InputSystem;

namespace MCC.Cameras
{
	/// <summary>
	/// Is is the Camera movement for the player.
	/// A camera is needed for this Component.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class FpsCamera : ModularBehaviour
	{
		/// <summary>
		/// This is the contoller scheme name.
		/// Make sure there is a "Gamepad" Input Scheme "Gamepad";
		/// </summary>
		private const string CONTROLLER_SCHEME_NAME = "Gamepad";
		
		/// <summary>
		/// These are the CameraSettings for the player.
		/// </summary>
		[SerializeField] private CameraSettings settings;
		
		/// <summary>
		/// This is refernce to the camera.
		/// </summary>
		private new Camera camera;
		/// <summary>
		/// 
		/// </summary>
		private PlayerInput input;
		private Transform player;

		private Vector2 rotation = Vector2.zero;
		
		public override void Init(MCCPlayer _player)
		{
			input = _player.Input;
			player = _player.transform;
			
			camera = gameObject.GetComponent<Camera>();
			camera.enabled = true;
			input.camera = camera;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		/// <summary>
		/// This is run in player Update method.
		/// </summary>
		protected override void OnProcess()
		{
			// We read the input systems lookVector.
			Vector2 lookVector = settings.Look.ReadValue<Vector2>();

			// This is the look rotation of the player side to side.
			rotation.x += lookVector.x * settings.GetSensitivity(input.currentControlScheme == CONTROLLER_SCHEME_NAME);
			// This is the look rotation of the player up and down.
			rotation.y += lookVector.y * settings.GetSensitivity(input.currentControlScheme == CONTROLLER_SCHEME_NAME);
			// Clamp the up and down to the look bounds.
			rotation.y = Mathf.Clamp(rotation.y, -settings.VerticalLookBounds, settings.VerticalLookBounds);
			// This changes the Camera Up and down transform.
			transform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
			//transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
			player.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up); // TODO Kieran fix this
		}

		protected override void OnEnabledStateChanged(bool _newState)
		{
			if(camera == null)
				camera = gameObject.GetComponent<Camera>();

			gameObject.GetComponent<AudioListener>().enabled = _newState;
			camera.enabled = _newState;
		}
	}
}