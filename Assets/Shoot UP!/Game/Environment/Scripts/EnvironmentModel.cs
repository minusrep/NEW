using Core;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game_.Environment
{
    [System.Serializable]
    public class EnvironmentModel : Model
    {
        public CameraBehavior camera => this._camera;
        public AudioSource audioSource => this._audioSource;
        public AudioClip menuSound => this._menuSound;
        [SerializeField] private CameraBehavior _camera;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _menuSound;
    }
}