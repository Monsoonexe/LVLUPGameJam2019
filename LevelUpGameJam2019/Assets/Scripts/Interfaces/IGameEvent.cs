

public interface IGameEvent<T>//Generic
{
    void Raise(T value);
    void AddListener(IGameEventListener<T> listener);
    void RemoveListener(IGameEventListener<T> listener);
    void RemoveAll();
}
public interface IGameEvent//paramaterless
{
    void Raise();
    void AddListener(IGameEventListener listener);
    void RemoveListener(IGameEventListener listener);
    void RemoveAll();
}
