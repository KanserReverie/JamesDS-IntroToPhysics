// Creator: Kieran
// Creation Time: 2022/06/06 16:41
using MCC;

using UnityEngine;
using UnityEngine.InputSystem;

namespace MCC.Combat
{
    public class CombatBehaviour : ModularBehaviour
    {
        [SerializeField] private InputActionReference shoot;

        public override void Init(IMCCPlayer _playerInterface)
        {
            shoot.action.Enable();
        }

        protected override void OnProcess(UpdatePhase _phase)
        {
            if(Mathf.Approximately(shoot.action.ReadValue<float>(), 1))
            {
                Debug.LogWarning("Bang");
            }
        }
    }
}