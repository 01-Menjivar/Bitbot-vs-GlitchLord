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
            Path.Combine(folderPath, "bug_1.prefab"),
            1,
            new Vector3(0.35f, 0.35f, 1f),
            2.0f
        );

        // Create bug_2
        CreateSinglePrefab(
            "Assets/Art/Enemies/enemy-bug-2.png",
            "bug_2",
            Path.Combine(folderPath, "bug_2.prefab"),
            1,
            new Vector3(0.24f, 0.24f, 1f),
            3.5f
        );

        // Create bug_3
        CreateSinglePrefab(
            "Assets/Art/Enemies/enemy-bug-3.png",
            "bug_3",
            Path.Combine(folderPath, "bug_3.prefab"),
            2,
            new Vector3(0.37f, 0.37f, 1f),
            1.0f
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateSinglePrefab(string texturePath, string goName, string prefabPath, int initialHealth, Vector3 scale, float movementSpeed)
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

        // Create temporary GameObject and set scale
        GameObject tempGo = new GameObject(goName);
        tempGo.transform.localScale = scale;

        SpriteRenderer sr = tempGo.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // Add BoxCollider2D for click/raycast detection
        tempGo.AddComponent<BoxCollider2D>();

        // Add BugHealth component and set initial health
        BugHealth bh = tempGo.AddComponent<BugHealth>();
        bh.SetMaxHealth(initialHealth);

        // Add BugBehavior component and set speed
        BugBehavior bb = tempGo.AddComponent<BugBehavior>();
        bb.SetSpeed(movementSpeed);

        // Save as Prefab
        GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(tempGo, prefabPath);

        // Clean up
        Object.DestroyImmediate(tempGo);

        if (prefabAsset != null)
        {
            Debug.Log("Successfully created prefab '" + goName + "' with scale " + scale + " and " + initialHealth + " HP at: " + prefabPath);
        }
        else
        {
            Debug.LogError("Failed to create prefab '" + goName + "'");
        }
    }
}
