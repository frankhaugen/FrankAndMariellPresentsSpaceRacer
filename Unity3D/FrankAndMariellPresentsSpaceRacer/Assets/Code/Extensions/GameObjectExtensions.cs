using UnityEngine;

namespace Code.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsPlayer(this GameObject gameObject) => gameObject.CompareTag("Player");
    }
}