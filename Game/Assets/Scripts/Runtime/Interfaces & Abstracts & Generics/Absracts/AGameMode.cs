using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameMode : ScriptableObject
{
    public abstract void GameModeUpdate();
    public abstract void InitGameMode();
    public abstract void Success();
    public abstract void Fail();
}

