using UnityEngine;
using UnityEditor;

public class AudioCreator
{
    [MenuItem("Tools/Create Audio Objects With Sources")]
    static void CreateAudioObjects()
    {
        // Get all selected AudioClips in the Project window
        AudioClip[] clips = Selection.GetFiltered<AudioClip>(SelectionMode.Assets);

        // Create a parent container for organization
        GameObject parent = new GameObject("Nodo");

        foreach (AudioClip clip in clips)
        {
            // Create a new GameObject named after the clip
            GameObject obj = new GameObject(clip.name);

            // Add an AudioSource and assign the clip
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = clip;

            // Parent under the container
            obj.transform.parent = parent.transform;
        }
    }
}
