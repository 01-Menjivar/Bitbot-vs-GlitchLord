using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using System.IO;

public class SetupLevel3Final
{
    [MenuItem("Tools/Setup Level 3 Final")]
    public static void RunSetup()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            Debug.LogWarning("Se desactivó el Play Mode para realizar la configuración. Por favor ejecute el comando nuevamente.");
            return;
        }

        CreateVirusAnimatorControllers();
        ConfigureVirusPrefabs();

        Debug.Log("[Setup Lvl3] Configuración de virus completada con éxito.");
    }

    private static void CreateVirusAnimatorControllers()
    {
        string dir = "Assets/Animations/Virus";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            AssetDatabase.Refresh();
        }

        string[] virusPaths = new string[]
        {
            "Assets/Art/Enemies/Virus/Idle_Virus.png",
            "Assets/Art/Enemies/Virus/Moving_Virus.png",
            "Assets/Art/Enemies/Virus/Spin_Virus.png",
            "Assets/Art/Enemies/Virus/Defeated_Virus.png",
            "Assets/Art/Enemies/Virus/Damaged_Virus.png",
            "Assets/Art/Enemies/Virus/Tank_Virus.png",
            "Assets/Art/Enemies/Virus/Brace_Virus.png",
            "Assets/Art/Enemies/Virus/Charged_Virus.png"
        };

        foreach (string path in virusPaths)
        {
            EnsureSpriteType(path);
        }

        Sprite idleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Idle_Virus.png");
        Sprite movingSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Moving_Virus.png");
        Sprite spinSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Spin_Virus.png");
        Sprite defeatedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Defeated_Virus.png");
        Sprite tankSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Tank_Virus.png");
        Sprite braceSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Brace_Virus.png");
        Sprite chargedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Charged_Virus.png");

        CreateSingleController("NormalVirus", idleSprite, movingSprite, spinSprite, defeatedSprite);
        CreateSingleController("FastVirus", idleSprite, spinSprite, chargedSprite, defeatedSprite);
        CreateSingleController("TankVirus", braceSprite, tankSprite, chargedSprite, defeatedSprite);
    }

    private static void CreateSingleController(string prefix, Sprite idleS, Sprite moveS, Sprite spinS, Sprite smashS)
    {
        string path = $"Assets/Animations/Virus/{prefix}Controller.controller";
        
        if (File.Exists(path))
        {
            AssetDatabase.DeleteAsset(path);
        }

        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
        var rootStateMachine = controller.layers[0].stateMachine;

        AnimationClip spawnClip = CreateSpawnClip(prefix, idleS);
        AnimationClip moveClip = CreateMoveClip(prefix, moveS);
        AnimationClip spinClip = CreateSpinClip(prefix, spinS);
        AnimationClip smashClip = CreateSmashClip(prefix, smashS);

        AssetDatabase.CreateAsset(spawnClip, $"Assets/Animations/Virus/{prefix}_Spawning.anim");
        AssetDatabase.CreateAsset(moveClip, $"Assets/Animations/Virus/{prefix}_Moving.anim");
        AssetDatabase.CreateAsset(spinClip, $"Assets/Animations/Virus/{prefix}_Spin.anim");
        AssetDatabase.CreateAsset(smashClip, $"Assets/Animations/Virus/{prefix}_Smashed.anim");

        controller.AddParameter("State", AnimatorControllerParameterType.Int);

        var spawnState = rootStateMachine.AddState("Spawning");
        spawnState.motion = spawnClip;

        var moveState = rootStateMachine.AddState("Moving");
        moveState.motion = moveClip;

        var spinState = rootStateMachine.AddState("Spin");
        spinState.motion = spinClip;

        var smashState = rootStateMachine.AddState("Smashed");
        smashState.motion = smashClip;

        rootStateMachine.defaultState = spawnState;

        var t1 = spawnState.AddTransition(moveState);
        t1.hasExitTime = true;
        t1.exitTime = 1.0f;
        t1.duration = 0.1f;
        t1.AddCondition(AnimatorConditionMode.Equals, 1, "State");

        var t2 = moveState.AddTransition(spinState);
        t2.AddCondition(AnimatorConditionMode.Equals, 2, "State");
        t2.hasExitTime = false;
        t2.duration = 0.2f;

        var t3 = rootStateMachine.AddAnyStateTransition(smashState);
        t3.AddCondition(AnimatorConditionMode.Equals, 3, "State");
        t3.duration = 0.05f;
    }

    private static AnimationClip CreateSpawnClip(string prefix, Sprite s)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = $"{prefix}_Spawning";
        clip.frameRate = 12;

        EditorCurveBinding spriteBinding = EditorCurveBinding.PPtrCurve("Visual", typeof(SpriteRenderer), "m_Sprite");
        ObjectReferenceKeyframe[] spriteKeys = { new ObjectReferenceKeyframe { time = 0f, value = s } };
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeys);

        var curveX = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1.15f), new Keyframe(0.7f, 1.0f));
        var curveY = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1.15f), new Keyframe(0.7f, 1.0f));
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.x", curveX);
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.y", curveY);

        var curveAlpha = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1.0f));
        clip.SetCurve("Visual", typeof(SpriteRenderer), "m_Color.a", curveAlpha);

        return clip;
    }

    private static AnimationClip CreateMoveClip(string prefix, Sprite s)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = $"{prefix}_Moving";
        clip.frameRate = 12;

        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        EditorCurveBinding spriteBinding = EditorCurveBinding.PPtrCurve("Visual", typeof(SpriteRenderer), "m_Sprite");
        ObjectReferenceKeyframe[] spriteKeys = { new ObjectReferenceKeyframe { time = 0f, value = s } };
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeys);

        var curveX = new AnimationCurve(new Keyframe(0f, 1.0f), new Keyframe(0.5f, 1.06f), new Keyframe(1.0f, 1.0f));
        var curveY = new AnimationCurve(new Keyframe(0f, 1.06f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 1.06f));
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.x", curveX);
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.y", curveY);

        var curveRot = new AnimationCurve(new Keyframe(0f, -4f), new Keyframe(0.5f, 4f), new Keyframe(1.0f, -4f));
        clip.SetCurve("Visual", typeof(Transform), "localEulerAnglesRaw.z", curveRot);
        clip.SetCurve("Visual", typeof(SpriteRenderer), "m_Color.a", AnimationCurve.Constant(0f, 1f, 1f));

        return clip;
    }

    private static AnimationClip CreateSpinClip(string prefix, Sprite s)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = $"{prefix}_Spin";
        clip.frameRate = 12;

        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        EditorCurveBinding spriteBinding = EditorCurveBinding.PPtrCurve("Visual", typeof(SpriteRenderer), "m_Sprite");
        ObjectReferenceKeyframe[] spriteKeys = { new ObjectReferenceKeyframe { time = 0f, value = s } };
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeys);

        var curveRot = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1.0f, -360f));
        clip.SetCurve("Visual", typeof(Transform), "localEulerAnglesRaw.z", curveRot);

        var curveX = new AnimationCurve(new Keyframe(0f, 0.95f), new Keyframe(0.5f, 1.15f), new Keyframe(1.0f, 0.95f));
        var curveY = new AnimationCurve(new Keyframe(0f, 1.15f), new Keyframe(0.5f, 0.95f), new Keyframe(1.0f, 1.15f));
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.x", curveX);
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.y", curveY);
        clip.SetCurve("Visual", typeof(SpriteRenderer), "m_Color.a", AnimationCurve.Constant(0f, 1f, 1f));

        return clip;
    }

    private static AnimationClip CreateSmashClip(string prefix, Sprite s)
    {
        AnimationClip clip = new AnimationClip();
        clip.name = $"{prefix}_Smashed";
        clip.frameRate = 12;

        EditorCurveBinding spriteBinding = EditorCurveBinding.PPtrCurve("Visual", typeof(SpriteRenderer), "m_Sprite");
        ObjectReferenceKeyframe[] spriteKeys = { new ObjectReferenceKeyframe { time = 0f, value = s } };
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeys);

        var curveX = new AnimationCurve(new Keyframe(0f, 1.0f), new Keyframe(0.2f, 1.5f));
        var curveY = new AnimationCurve(new Keyframe(0f, 1.0f), new Keyframe(0.2f, 0.1f));
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.x", curveX);
        clip.SetCurve("Visual", typeof(Transform), "m_LocalScale.y", curveY);

        var curveAlpha = new AnimationCurve(new Keyframe(0f, 1.0f), new Keyframe(0.2f, 0f));
        clip.SetCurve("Visual", typeof(SpriteRenderer), "m_Color.a", curveAlpha);

        return clip;
    }

    private static void ConfigureVirusPrefabs()
    {
        string[] prefabs = { "bug_1", "bug_2", "bug_3" };
        string[] controllers = { "NormalVirus", "FastVirus", "TankVirus" };
        
        Sprite idleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Idle_Virus.png");
        Sprite tankSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Brace_Virus.png");
        
        for (int i = 0; i < prefabs.Length; i++)
        {
            string prefabPath = $"Assets/Prefabs/{prefabs[i]}.prefab";
            GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);

            Transform visualTrans = prefab.transform.Find("Visual");
            GameObject visualObj;
            if (visualTrans == null)
            {
                visualObj = new GameObject("Visual");
                visualObj.transform.SetParent(prefab.transform, false);
            }
            else
            {
                visualObj = visualTrans.gameObject;
            }

            SpriteRenderer rootSR = prefab.GetComponent<SpriteRenderer>();
            SpriteRenderer childSR = visualObj.GetComponent<SpriteRenderer>();
            if (childSR == null)
            {
                childSR = visualObj.AddComponent<SpriteRenderer>();
            }

            if (rootSR != null)
            {
                childSR.color = rootSR.color;
                childSR.sortingLayerID = rootSR.sortingLayerID;
                childSR.sortingOrder = rootSR.sortingOrder;
                childSR.sharedMaterial = rootSR.sharedMaterial;
                Object.DestroyImmediate(rootSR);
            }

            childSR.sprite = (i == 2) ? tankSprite : idleSprite;
            prefab.tag = "Bug";

            Animator animator = prefab.GetComponent<Animator>();
            if (animator == null)
            {
                animator = prefab.AddComponent<Animator>();
            }

            var controllerAsset = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>($"Assets/Animations/Virus/{controllers[i]}Controller.controller");
            animator.runtimeAnimatorController = controllerAsset;

            PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefab);
            Debug.Log($"[Setup] Configurado prefab {prefabs[i]} con el nuevo sprite y Animator.");
        }
    }

    private static void EnsureSpriteType(string path)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
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
            if (needsReimport)
            {
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
