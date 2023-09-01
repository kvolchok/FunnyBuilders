using DefaultNamespace;
using UnityEngine;

public class Unit : MonoBehaviour, IDraggable
{
    private static readonly int _idle = Animator.StringToHash("Idle");
    private static readonly int _work = Animator.StringToHash("Work");
    private static readonly int _run = Animator.StringToHash("Run");

    public UnitState State { get; private set; }
    public int Level { get; private set; }
    public int Salary { get; private set; }

    [SerializeField]
    private SkinnedMeshRenderer _renderer;
    [SerializeField]
    private Animator _waterPuddle;
    
    [SerializeField] private PopupTextOverWorker _popupText;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
        _popupText.HideFloatingText();
        TurnOffPuddle();
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
                _popupText.ShowFloatingText(Salary);
                TurnOnPuddle();
                break;
        }
    }

    private void TurnOnPuddle()
    {
       _waterPuddle.gameObject.SetActive(true);
       _waterPuddle.SetTrigger(_work);
    }

    private void TurnOffPuddle()
    {
        if (!_waterPuddle.gameObject.activeInHierarchy)
        {
            return;
        }
        
        _waterPuddle.SetTrigger(_idle);
        _waterPuddle.gameObject.SetActive(false);
    }
    
    private void ShowAnimation(int state)
    {
        _animator.SetTrigger(state);
    }
}