using UnityEngine;

public enum GameState
{
    None,
    AppAwake,
    Loading,
    MenuLoading,
    Menu,
    GameplayLoading,
    Gameplay,
    ResultWin,
    ResultLose,
    Pause
}

public abstract class GameManagerBase : MonoBehaviour
{
    [SerializeField] protected GameState currentState = GameState.None;
    [SerializeField] protected GameState prevState    = GameState.None;

    public GameState CurrentState => currentState;
    public GameState PrevState => prevState;

    public System.Action<GameState> OnStateEnter       = delegate { };
    public System.Action<GameState> OnStateExit        = delegate { };
    public System.Action<GameState> OnStateUpdate      = delegate { };
    public System.Action<GameState> OnStateFixedUpdate = delegate { };
    public System.Action<GameState> OnStateLateUpdate  = delegate { };

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        ChangeState(GameState.AppAwake);
    }

    protected virtual void Update()
    {
        StateUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateFixedUpdate();
    }

    protected virtual void LateUpdate()
    {
        StateLateUpdate();
    }

    protected virtual void StateEnter()
    {
        OnStateEnter?.Invoke(currentState);
    }

    protected virtual void StateUpdate()
    {
        OnStateUpdate?.Invoke(currentState);
    }
    protected virtual void StateFixedUpdate()
    {
        OnStateFixedUpdate?.Invoke(currentState);
    }

    protected virtual void StateLateUpdate()
    {

    }

    protected virtual void StateExit()
    {
        OnStateExit?.Invoke(currentState);
    }

    protected void ChangeState(GameState newState, bool forceReload = false)
    {
        if (currentState == newState && !forceReload) return;
        StateExit();
        prevState = currentState;
        currentState = newState;
        StateEnter();
    }
}
