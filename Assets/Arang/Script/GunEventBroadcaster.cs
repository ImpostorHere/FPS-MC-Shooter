using System;
using UnityEngine;

public class GunEventBroadcaster : MonoBehaviour
{
    public event Action OnReloadMagazineAttached;
    public event Action OnReloadDone;

    void Broadcast_ReloadMagazineAttached() // dipanggil dari Animation Event
    {
        OnReloadMagazineAttached?.Invoke();
        Debug.Log("OnReloadMagazineAttached Invoked");
    }

    void Broadcast_ReloadDone()
    {
        OnReloadDone?.Invoke();
        Debug.Log("OnReloadDone Invoked");
    }
}
