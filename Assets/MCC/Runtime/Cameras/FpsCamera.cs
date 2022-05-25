using UnityEngine;
using UnityEngine.InputSystem;

namespace MCC.Cameras
{
	[RequireComponent(typeof(Camera))]
	public class FpsCamera : ModularBehaviour
	{
		private const string CONTROLLER_SCHEME_NAME = "Gamepad";
		
		[SerializeField] private CameraSettings settings;
		
		private new Camera camera;
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

		protected override void OnProcess()
		{
			Vector2 lookVector = settings.Look.ReadValue<Vector2>();

			rotation.x += lookVector.x * settings.GetSensitivity(input.currentControlScheme == CONTROLLER_SCHEME_NAME);
			rotation.y += lookVector.y * settings.GetSensitivity(input.currentControlScheme == CONTROLLER_SCHEME_NAME);
			rotation.y = Mathf.Clamp(rotation.y, -settings.VerticalLookBounds, settings.VerticalLookBounds);
			
			transform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
			player.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
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