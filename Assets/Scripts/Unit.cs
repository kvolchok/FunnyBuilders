using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDraggable
{
    private static readonly int _idle = Animator.StringToHash("Idle");
    private static readonly int _work = Animator.StringToHash("Work");
    private static readonly int _run = Animator.StringToHash("Run");

    public event Action<Unit> UnitDestroyed;

    public Transform Transform => transform;
    
    public UnitState State { get; private set; }
    public int Level { get; private set; }
    public int Salary { get; private set; }

    [SerializeField]
    private SkinnedMeshRenderer _renderer;
    [SerializeField]
    private Animator _unitAnimator;
    [SerializeField]
    private Animator _waterPuddleAnimator;
    [SerializeField]
    private PopupTextView _popupText;

    public void Initialize(UnitSettings unitSettings)
    {
        Level = unitSettings.Level;
        Salary = unitSettings.Salary;
        _renderer.material.color = unitSettings.Color;
    }

    public void Drag(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ChangeState(UnitState state)
    {
        _popupText.HidePopupText();
        TurnOffPuddleAnimation();
        State = state;

        switch (State)
        {
            case UnitState.Idle:
                ShowAnimation(_idle);
                break;
            case UnitState.Run:
                ShowAnimation(_run);
                break;
            case UnitState.Work:
                ShowAnimation(_work);
                _popupText.ShowPopupText(Salary);
                TurnOnPuddleAnimation();
                break;
        }
    }
    
    public void DestroyUnit()
    {
        UnitDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void TurnOnPuddleAnimation()
    {
       _waterPuddleAnimator.gameObject.SetActive(true);
       _waterPuddleAnimator.SetTrigger(_work);
    }

    private void TurnOffPuddleAnimation()
    {
        if (!_waterPuddleAnimator.gameObject.activeInHierarchy)
        {
            return;
        }
        
        _waterPuddleAnimator.SetTrigger(_idle);
        _waterPuddleAnimator.gameObject.SetActive(false);
    }
    
    private void ShowAnimation(int state)
    {
        _unitAnimator.SetTrigger(state);
    }
}