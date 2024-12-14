

using UnityEngine;

public static class ShaderPorperties
{
    public readonly static int baseColor = Shader.PropertyToID("_BaseColor");
    public readonly static int alpha = Shader.PropertyToID("_Alpha");
    public readonly static int emission = Shader.PropertyToID("_Emission");
    public readonly static int speed = Shader.PropertyToID("_Speed");

}