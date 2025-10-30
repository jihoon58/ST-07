using UnityEngine;

namespace ST07.World
{
    public interface IInteractable
    {
        bool CanInteract(GameObject actor);
        void Interact(GameObject actor);
    }
}



