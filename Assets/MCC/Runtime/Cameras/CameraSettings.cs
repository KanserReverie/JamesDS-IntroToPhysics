using UnityEngine;
using UnityEngine.InputSystem;

namespace MCC.Cameras
{
	[CreateAssetMenu(fileName = "New Camera Settings", menuName = "MCC/Camera Settings", order = 0)]
	public class CameraSettings : ScriptableObject
	{
		/// <summary>
		/// This is the "Look" action input.
		/// </summary>
		public InputAction Look => lookAction.action;
		/// <summary>
		/// The sensitivity of the input.
		/// </summary>
		/// <param name="_controller"> If we are using a controller, use the controller sensitivity.
		/// If we are using a mouse/keyboard use the mouse/keyboard sensitivity.</param>
		/// <returns> returns the correct sensitivity.</returns>
		public float GetSensitivity(bool _controller) => _controller ? controllerSensitivity : mouseSensitivity;
		/// <summary>
		/// This is the vertical look bounds of the player.
		/// </summary>
		public float VerticalLookBounds => verticalLookBounds;
		
		/// <summary>
		/// The the mouse sensitivity for the look direction.
		/// </summary>
		[SerializeField, Range(0, 3)] private float mouseSensitivity = .5f;
		/// <summary>
		/// The controller sensitivity, if you are using the controller.
		/// </summary>
		[SerializeField, Range(0, 3)] private float controllerSensitivity = .5f;
		/// <summary>
		/// The vertical look bounds up and down.
		/// </summary>
		[SerializeField, Range(0, 90)] private float verticalLookBounds = 90;

		/// <summary>
		/// The look action from the Input system.
		/// </summary>
		[SerializeField] private InputActionReference lookAction;
	}
}