using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{

    // Queue that stores actions waiting to be executed on the main Unity thread
    private static readonly Queue<Action> _executionQueue = new();

    public void Update()
    {
        // Unity runs Update() on the main thread.
        // So we execute all queued actions here safely
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }
    public void Enqueue(IEnumerator action)
    {
        // Enqueue a coroutine to run on the main thread
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
            {
                StartCoroutine(action);
            });
        }
    }
    public void Enqueue(Action action)
    {
        Enqueue(ActionWrapper(action));
    }
    // Enqueue an Action but return a Task so you can "await" it
    public Task EnqueueAsync(Action action)
    {
        var tcs = new TaskCompletionSource<bool>();
        void WrappedAction()
        {
            try
            {
                action();
                tcs.TrySetResult(true);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }
        Enqueue(ActionWrapper(WrappedAction));
        return tcs.Task;
    }
    // Wrap an Action into a coroutine (required by Enqueue)
    IEnumerator ActionWrapper(Action a)
    {
        a();
        yield return null;
    }
    // Singleton setup
    private static UnityMainThreadDispatcher _instance = null;

    public static bool Exists()
    {
        return _instance != null;
    }
    public static UnityMainThreadDispatcher Instance()
    {
        if (!Exists())
        {
            throw new Exception("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        }
        return _instance;
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void OnDestroy()
    {
        _instance = null;
    }
}