using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State State;

    // Update is called once per frame
    public void SetState(State _state)
    {
        State = _state;
        StartCoroutine(State.Start());
    }
}
