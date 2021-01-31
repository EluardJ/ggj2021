using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComptoirChunk : RoomChunk
{
    public BoxCollider _comptoirTrigger;

    [SerializeField] ParticleSystem successParticles = default;

    private List<IComptoirTriggerListener> _listeners = new List<IComptoirTriggerListener>();

    public bool OnItemEnter(Item item)
    {
        bool hasBeenDropped = false;
        if (_listeners == null)
        {
            return hasBeenDropped;
        }
        if (item != null)
        {
            foreach (IComptoirTriggerListener listener in _listeners)
            {
                if (listener == null)
                {
                    continue;
                }
                hasBeenDropped = hasBeenDropped || listener.OnItemDropped(item);
            }
        }

        if (hasBeenDropped)
            successParticles.Play();

        return hasBeenDropped;
    }

    public void RegisterListener(IComptoirTriggerListener listener)
    {
        if (_listeners == null || listener == null || _listeners.Contains(listener))
        {
            return;
        }
        _listeners.Add(listener);
    }

}
