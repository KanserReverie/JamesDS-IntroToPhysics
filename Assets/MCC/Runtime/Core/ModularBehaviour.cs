using UnityEngine;

namespace MCC
{
	/// <summary>
	/// These are an abstract class that is used as a base of all all components.
	/// E.g. Camera and Motion.
	/// </summary>
	public abstract class ModularBehaviour : MonoBehaviour
	{
		// Creating a public Property "Enabled"
		// This will enable or disable the state of this component.
		// When set it forces the class to run OnEnabledStateChanged();
		public bool Enabled
		{
			get => intEnabled;
			set
			{
				intEnabled = value;
				OnEnabledStateChanged(intEnabled);
			}
		}

		// This is for if the variable is "localOnly" (for multiplayer)
		public bool LocalOnly => localOnly;
		[SerializeField] private bool localOnly = true;
		
		// This will be how often Process() will be called, defaulting to "Update()"
		[SerializeField] private UpdatePhase updatePhase = UpdatePhase.Update;
		
		// If this component was enabled.
		private bool intEnabled = true;
		
		/// <summary>
		/// Initialisation of this component, passing in the player.
		/// This is called in the MCCPlayer Start().
		/// </summary>
		/// <param name="_player">The Start() of the MCC player this commponent is attached to.</param>
		public virtual void Init(MCCPlayer _player) { }

		/// <summary>
		/// Called in all update functions on the MCCPlayer, with a reference to the phase being called from,
		/// so that the ModularBehaviour can be updated in specific loops.
		/// </summary>
		/// <param name="_phase"> The update loop that this process function is being called from. </param>
		public void Process(UpdatePhase _phase)
		{
			if(_phase != updatePhase || !Enabled)
				return;
			
			OnProcess();
		}

		/// <summary>
		/// "Abstract" forces inherited classes to overwrite OnProcess().
		/// </summary>
		protected abstract void OnProcess();

		/// <summary>
		/// This is called when this component is Enabled.
		/// This allows the derived class to overwrite what happens when this component is enabled.
		/// </summary>
		/// <param name="_newState"> If the component is enabled or disabled. </param>
		protected virtual void OnEnabledStateChanged(bool _newState) { }
	}
}