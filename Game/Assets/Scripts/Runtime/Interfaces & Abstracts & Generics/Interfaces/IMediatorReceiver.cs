using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMediatorReceiver<T> where T : IMediatorPayload
{
    public static IMediator<T> mediator { get; }
    public void SendMessageToMeadiator(T payload);
    public void OnReceiveMessage(T payload);


}
