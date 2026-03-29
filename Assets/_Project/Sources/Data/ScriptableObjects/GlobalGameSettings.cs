using UnityEngine;

namespace Sources.Data.ScriptableObjects
{
    [CreateAssetMenu (menuName = "Scriptable Objects/Global Project Settings", fileName = "GlobalProjectSettings", order = 51)]
    public class GlobalGameSettings : ScriptableObject
    {
        [SerializeField] private float _delayedTickTime = 0.1f;

        public float DelayedTickTime => _delayedTickTime;
    }
}