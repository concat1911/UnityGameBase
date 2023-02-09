using UnityEngine;

public static class AnimationHelper
{
    public static readonly int ShowHashed = Animator.StringToHash("Show");
    public static readonly int HideHashed = Animator.StringToHash("Hide");
    public static readonly int InitHashed = Animator.StringToHash("Init");
    public static readonly int OffHashed  = Animator.StringToHash("Off");
}
