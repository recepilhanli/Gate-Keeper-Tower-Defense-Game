
using System.Collections.Generic;
using System.Diagnostics;

public class MediatorInstance<T1, T2> : IMediator<T2> where T2 : IMediatorPayload where T1 : class, new()
{
    protected HashSet<IMediatorReceiver<T2>> _receivers = new HashSet<IMediatorReceiver<T2>>();
    private static T1 _instance;

    public static T1 instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T1();
            }

            return _instance;
        }
    }

    public virtual void RegisterReceiver(IMediatorReceiver<T2> receiver)
    {
        _receivers.Add(receiver);
    }

    public virtual void UnregisterReceiver(IMediatorReceiver<T2> receiver)
    {
        _receivers.Remove(receiver);
    }

    /// <summary>
    /// Send a payload to a receiver
    /// </summary>
    public void SendPayloadToReceiver(IMediatorReceiver<T2> receiver, T2 payload)
    {
        receiver.OnReceiveMessage(payload);
    }

    /// <summary>
    /// Broadcast a payload to all receivers
    /// </summary>
    public void BroadcastPayload(T2 payload)
    {
        foreach (var receiver in _receivers)
        {
            receiver.OnReceiveMessage(payload);
        }
    }

    /// <summary>
    /// Send a payload to the mediator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="payload"></param>
    public void SendPayload(IMediatorReceiver<T2> sender, T2 payload) { OnReceivePayload(sender, payload); }

    protected virtual void OnReceivePayload(IMediatorReceiver<T2> sender, T2 payload)
    {
   
    }

}
