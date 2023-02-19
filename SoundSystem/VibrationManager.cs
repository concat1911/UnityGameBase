using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    public bool status { get; private set; }
    public static bool Status => instance.status;

    private void Awake()
    {
        if( instance )
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static void Vibrate(int value = 200)
    {
        if (!instance.status) return;

        Vibration.Vibrate(value);
    }

    public static bool ToggleStatus()
    {
        return instance.status = !instance.status;
    }

    public static void SetStatus(bool status)
    {
        instance.status = status;
    }
}
