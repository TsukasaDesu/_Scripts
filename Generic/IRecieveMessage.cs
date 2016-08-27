using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public interface IRecieveMessage : IEventSystemHandler
{
    void OnRecieve(float power);
}