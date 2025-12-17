using UnityEditor;
using UnityEngine;

public class PrefabReplacer : EditorWindow
{
    public GameObject newPrefab;

    [MenuItem("Tools/Prefab Replacer")]
    static void Init()
    {
        GetWindow<PrefabReplacer>("Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Select objects in scene, pick new prefab, and click Replace.", EditorStyles.boldLabel);

        newPrefab = (GameObject)EditorGUILayout.ObjectField("New Prefab", newPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace Selected") && newPrefab != null)
        {
            ReplaceSelected();
        }
    }

    void ReplaceSelected()
    {
        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();

        foreach (GameObject oldObj in Selection.gameObjects)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);

            newObj.transform.position = oldObj.transform.position;
            newObj.transform.rotation = oldObj.transform.rotation;
           // newObj.transform.localScale = oldObj.transform.localScale;
            newObj.transform.parent = oldObj.transform.parent;

            Undo.RegisterCreatedObjectUndo(newObj, "Created New Tree");
            Undo.DestroyObjectImmediate(oldObj);
        }

        Undo.CollapseUndoOperations(group);
    }
}
