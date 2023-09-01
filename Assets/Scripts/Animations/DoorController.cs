using UnityEngine;

namespace Animations
{
    public class DoorController : MonoBehaviour
    {
        private static readonly int _open = Animator.StringToHash("Open");
        private static readonly int _close = Animator.StringToHash("Close");
    
        [SerializeField]
        private Animator _doorAnimator;
        [SerializeField]
        private float _doorAnimationSpeed;

        private void Awake()
        {
            _doorAnimator.speed = _doorAnimationSpeed;
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.GetComponent<Unit>())
            {
                _doorAnimator.SetTrigger(_open);
            }
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            if (otherCollider.GetComponent<Unit>())
            {
                _doorAnimator.SetTrigger(_close);
            }
        }
    }
}