using UnityEngine;

namespace Sources.UI.Sounds
{
    public class UISoundsSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _buttonClickSource;
        [SerializeField] private AudioClip _buttonClickSound;
        
        public void PlayButtonClickSound() => 
            _buttonClickSource.PlayOneShot(_buttonClickSound);
    }
}