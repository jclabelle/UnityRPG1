using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipList : ScriptableObject
{
    [field: SerializeField] public List<AudioClip> AudioClips { get; set; }
}
