using UnityEngine;

public class ColorCaller : MonoBehaviour
{
    private int id;

    private bool isBlocked = false;
    private Animator animator;
    [SerializeField]
    private Color colorOfLeveler;
    [SerializeField]
    private ChangeColor colorChanger;

    public enum State
    {
        Left, Mid, Right
    };
    private State currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        switch (id)
        {
            case 0: currentState = State.Left; break;
            case 1: currentState = State.Left; break;
            case 2: currentState = State.Right; break;
        }

        switch (currentState)
        {
            case State.Left: animator.SetTrigger("SwitchToLeft"); break;
            case State.Mid: animator.SetTrigger("SwitchToMid"); break;
            case State.Right: animator.SetTrigger("SwitchToRight"); break;
        }
        colorChanger.InitializeColor(id, currentState, colorOfLeveler);
        colorChanger.BlockLevelers += BlockLeveler;
    }

    public void ChangeState()
    {
        if (isBlocked)
            return;

        switch (currentState)
        {
            case State.Left:
                {
                    currentState = State.Mid;
                    animator.SetTrigger("SwitchToMid");
                    break;
                }
            case State.Mid:
                {
                    currentState = State.Right;
                    animator.SetTrigger("SwitchToRight");
                    break;
                }
            case State.Right:
                {
                    currentState = State.Left;
                    animator.SetTrigger("SwitchToLeft");
                    break;
                }
        }
        colorChanger.SwitchColor(id, currentState, colorOfLeveler);
    }

    private void BlockLeveler()
    {
        isBlocked = true;
    }
}
