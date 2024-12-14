using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMediator<T> where T : IMediatorPayload
{
    public void RegisterReceiver(IMediatorReceiver<T> receiver);
    public void UnregisterReceiver(IMediatorReceiver<T> receiver);
    public void SendPayload(IMediatorReceiver<T> receiver, T payload);
}
