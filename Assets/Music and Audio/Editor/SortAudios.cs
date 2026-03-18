using UnityEngine;
using UnityEditor;
using System.Linq;

public class SortAudios
{
    [MenuItem("Tools/Sort AudioObjects Alphabetically")]
    static void SortAudioObjects()
    {
        // Find the parent container
        GameObject parent = GameObject.Find("Nodo");
        if (parent == null)
        {
            Debug.LogWarning("No 'AudioObjects' parent found in the scene.");
            return;
        }

        // Get all children
        Transform[] children = parent.GetComponentsInChildren<Transform>()
                                     .Where(t => t != parent.transform)
                                     .OrderBy(t => t.name)
                                     .ToArray();

        // Reorder them alphabetically
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetSiblingIndex(i);
        }

        Debug.Log("AudioObjects sorted alphabetically!");
    }
}