using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabBuilder
{
    [MenuItem("Tools/Stop Play Mode")]
    public static void StopPlayMode()
    {
        EditorApplication.isPlaying = false;
        Debug.Log("Play mode stopped.");
    }

    [MenuItem("Tools/Create Bug Prefabs")]
    public static void CreateBugPrefabs()
    {
        string folderPath = "Assets/Prefabs";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        // Create bug_1
        CreateSinglePrefab(
            "Assets/Art/Enemies/enemy-bug-1.png",
            "bug_1",
            Path.Combine(folderPath, "bug_1.prefab")
        );

        // Create bug_2
        CreateSinglePrefab(
            "Assets/Art/Enemies/enemy-bug-2.png",
            "bug_2",
            Path.Combine(folderPath, "bug_2.prefab")
        );

        // Create bug_3
        CreateSinglePrefab(
            "Assets/Art/Enemies/enemy-bug-3.png",
            "bug_3",
            Path.Combine(folderPath, "bug_3.prefab")
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateSinglePrefab(string texturePath, string goName, string prefabPath)
    {
        // Ensure texture is imported as Sprite
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (importer != null)
        {
            bool needsReimport = false;
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                needsReimport = true;
            }
            if (importer.spriteImportMode != SpriteImportMode.Single)
            {
                importer.spriteImportMode = SpriteImportMode.Single;
                needsReimport = true;
            }
            if (importer.filterMode != FilterMode.Point)
            {
                importer.filterMode = FilterMode.Point;
                needsReimport = true;
            }
            if (importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                needsReimport = true;
            }
            if (needsReimport)
            {
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceSynchronousImport);
            }
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        if (sprite == null)
        {
            Debug.LogError("Failed to load sprite at " + texturePath);
            return;
        }

        // Create temporary GameObject
        GameObject tempGo = new GameObject(goName);
        SpriteRenderer sr = tempGo.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // Save as Prefab
        GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(tempGo, prefabPath);

        // Clean up
        Object.DestroyImmediate(tempGo);

        if (prefabAsset != null)
        {
            Debug.Log("Successfully created prefab '" + goName + "' at: " + prefabPath);
        }
        else
        {
            Debug.LogError("Failed to create prefab '" + goName + "'");
        }
    }
}
