// DSZP Unity 11 – Marsbázis: a 2. óra végi közös kiinduló állapot felépítése.
// Használat: tedd az Assets/Editor mappába, majd menü: DSZP → MarsBase_02 felépítése.
// A jelenetet újraépíti és elmenti: Assets/Scenes/MarsBase_02.unity
// Commit előtt TÖRÖLD az Assets/Editor mappából (a diákoknak nem kell).
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DszpMarsBaseBuilder
{
    const string ScenePath = "Assets/Scenes/MarsBase_02.unity";
    static readonly Dictionary<string, Transform> G = new Dictionary<string, Transform>();
    static readonly Dictionary<string, Material> M = new Dictionary<string, Material>();
    static readonly Color jelzoSzin = new Color(0.25f, 0.85f, 1f);
    static readonly Color bazisFenySzin = new Color(1f, 0.85f, 0.6f);

    [MenuItem("DSZP/MarsBase_02 felépítése")]
    public static void Build()
    {
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null &&
            !EditorUtility.DisplayDialog("MarsBase_02 felépítése",
                "Már van MarsBase_02 jelenet. Felülírjam?", "Felülírás", "Mégse"))
            return;
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        foreach (var f in new[] { "Scenes", "Materials", "Scripts", "Prefabs" })
            if (!AssetDatabase.IsValidFolder("Assets/" + f)) AssetDatabase.CreateFolder("Assets", f);

        G.Clear(); M.Clear();
        CreateMaterials();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        SetupCameraAndSun();

        // ---- objektumok (Objektumlista dokumentum, 5. fejezet) ----
        Empty("MarsBazis", null, new Vector3(0f, 0f, 0f));
        Empty("Kornyezet", G["MarsBazis"], new Vector3(0f, 0f, 0f));
        Prim("Talaj", PrimitiveType.Plane, G["Kornyezet"], new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(8f, 1f, 8f), "M_Talaj");
        Empty("Epuletek", G["MarsBazis"], new Vector3(0f, 0f, 0f));
        Prim("KozpontiModul", PrimitiveType.Cube, G["Epuletek"], new Vector3(0f, 2f, 20f), new Vector3(0f, 0f, 0f), new Vector3(8f, 4f, 8f), "M_Epulet");
        Prim("OldalModul_A", PrimitiveType.Cube, G["Epuletek"], new Vector3(-12f, 1.5f, 20f), new Vector3(0f, 0f, 0f), new Vector3(5f, 3f, 5f), "M_Epulet");
        Prim("OldalModul_B", PrimitiveType.Cube, G["Epuletek"], new Vector3(12f, 1.5f, 20f), new Vector3(0f, 0f, 0f), new Vector3(5f, 3f, 5f), "M_Epulet");
        Empty("Utvonalak", G["MarsBazis"], new Vector3(0f, 0f, 0f));
        Prim("Utvonal_Kozponthoz", PrimitiveType.Cube, G["Utvonalak"], new Vector3(0f, 0.05f, 10.25f), new Vector3(0f, 0f, 0f), new Vector3(3f, 0.1f, 11.5f), "M_SotetFem");
        Prim("Utvonal_Modulokhoz", PrimitiveType.Cube, G["Utvonalak"], new Vector3(0f, 0.05f, 20f), new Vector3(0f, 0f, 0f), new Vector3(24f, 0.1f, 3f), "M_SotetFem");
        Empty("Jatekmenet", G["MarsBazis"], new Vector3(0f, 0f, 0f));
        Empty("Leszallohely", G["Jatekmenet"], new Vector3(0f, 0f, 0f));
        Prim("LeszalloPlatform", PrimitiveType.Cylinder, G["Leszallohely"], new Vector3(0f, 0.2f, 0f), new Vector3(0f, 0f, 0f), new Vector3(10f, 0.2f, 10f), "M_Platform");
        Prim("Jelzooszlop_1", PrimitiveType.Cube, G["Leszallohely"], new Vector3(6f, 1f, 6f), new Vector3(0f, 0f, 0f), new Vector3(0.5f, 2f, 0.5f), "M_SotetFem");
        Prim("Jelzooszlop_2", PrimitiveType.Cube, G["Leszallohely"], new Vector3(-6f, 1f, 6f), new Vector3(0f, 0f, 0f), new Vector3(0.5f, 2f, 0.5f), "M_SotetFem");
        Prim("Jelzooszlop_3", PrimitiveType.Cube, G["Leszallohely"], new Vector3(6f, 1f, -6f), new Vector3(0f, 0f, 0f), new Vector3(0.5f, 2f, 0.5f), "M_SotetFem");
        Prim("Jelzooszlop_4", PrimitiveType.Cube, G["Leszallohely"], new Vector3(-6f, 1f, -6f), new Vector3(0f, 0f, 0f), new Vector3(0.5f, 2f, 0.5f), "M_SotetFem");
        Prim("Jelzofeny_1", PrimitiveType.Sphere, G["Leszallohely"], new Vector3(6f, 2.35f, 6f), new Vector3(0f, 0f, 0f), new Vector3(0.7f, 0.7f, 0.7f), "M_Jelzofeny");
        Prim("Jelzofeny_2", PrimitiveType.Sphere, G["Leszallohely"], new Vector3(-6f, 2.35f, 6f), new Vector3(0f, 0f, 0f), new Vector3(0.7f, 0.7f, 0.7f), "M_Jelzofeny");
        Prim("Jelzofeny_3", PrimitiveType.Sphere, G["Leszallohely"], new Vector3(6f, 2.35f, -6f), new Vector3(0f, 0f, 0f), new Vector3(0.7f, 0.7f, 0.7f), "M_Jelzofeny");
        Prim("Jelzofeny_4", PrimitiveType.Sphere, G["Leszallohely"], new Vector3(-6f, 2.35f, -6f), new Vector3(0f, 0f, 0f), new Vector3(0.7f, 0.7f, 0.7f), "M_Jelzofeny");
        PointLight("Pontfeny_1", G["Leszallohely"], new Vector3(6f, 2.35f, 6f), 6.0f, 3.0f, jelzoSzin);
        PointLight("Pontfeny_2", G["Leszallohely"], new Vector3(-6f, 2.35f, 6f), 6.0f, 3.0f, jelzoSzin);
        PointLight("Pontfeny_3", G["Leszallohely"], new Vector3(6f, 2.35f, -6f), 6.0f, 3.0f, jelzoSzin);
        PointLight("Pontfeny_4", G["Leszallohely"], new Vector3(-6f, 2.35f, -6f), 6.0f, 3.0f, jelzoSzin);
        Empty("Iranyitotorony", G["Leszallohely"], new Vector3(0f, 0f, 0f));
        Prim("Torony_alap", PrimitiveType.Cube, G["Iranyitotorony"], new Vector3(10f, 1f, 0f), new Vector3(0f, 0f, 0f), new Vector3(4f, 2f, 4f), "M_Epulet");
        Prim("Torony_felso", PrimitiveType.Cube, G["Iranyitotorony"], new Vector3(10f, 2.5f, 0f), new Vector3(0f, 0f, 0f), new Vector3(3f, 1f, 3f), "M_Epulet");
        Prim("Antenna", PrimitiveType.Cylinder, G["Iranyitotorony"], new Vector3(10f, 4f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0.2f, 1f, 0.2f), "M_SotetFem");
        Prim("Antenna_csucs", PrimitiveType.Sphere, G["Iranyitotorony"], new Vector3(10f, 5.2f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0.4f, 0.4f, 0.4f), "M_Jelzofeny");
        Prim("Rampa", PrimitiveType.Cube, G["Leszallohely"], new Vector3(0f, 0.25f, -6.5f), new Vector3(-8f, 0f, 0f), new Vector3(3f, 0.2f, 4f), "M_SotetFem");
        Empty("Rakomany", G["Leszallohely"], new Vector3(0f, 0f, 0f));
        Prim("Lada_1", PrimitiveType.Cube, G["Rakomany"], new Vector3(9f, 0.75f, 6f), new Vector3(0f, 15f, 0f), new Vector3(1.5f, 1.5f, 1.5f), "M_Epulet");
        Prim("Lada_2", PrimitiveType.Cube, G["Rakomany"], new Vector3(11f, 0.5f, 6f), new Vector3(0f, -10f, 0f), new Vector3(1f, 1f, 1f), "M_Epulet");
        Prim("Lada_3", PrimitiveType.Cube, G["Rakomany"], new Vector3(10f, 0.5f, 7.5f), new Vector3(0f, 30f, 0f), new Vector3(1f, 1f, 1f), "M_Epulet");
        Prim("EnergiaKonzol", PrimitiveType.Cube, G["Jatekmenet"], new Vector3(3.5f, 0.6f, 14f), new Vector3(0f, 0f, 0f), new Vector3(2f, 1.2f, 1f), "M_Jelzofeny");
        Empty("BazisFenyek", G["Jatekmenet"], new Vector3(0f, 0f, 0f));
        PointLight("BazisFeny_1", G["BazisFenyek"], new Vector3(-6f, 5f, 14f), 12.0f, 5.0f, bazisFenySzin);
        PointLight("BazisFeny_2", G["BazisFenyek"], new Vector3(6f, 5f, 14f), 12.0f, 5.0f, bazisFenySzin);
        PointLight("BazisFeny_3", G["BazisFenyek"], new Vector3(-12f, 4.5f, 16f), 8.0f, 4.0f, bazisFenySzin);
        PointLight("BazisFeny_4", G["BazisFenyek"], new Vector3(12f, 4.5f, 16f), 8.0f, 4.0f, bazisFenySzin);
        Empty("VedelmiPontok", G["Jatekmenet"], new Vector3(0f, 0f, 0f));
        Prim("VedelmiPont_1", PrimitiveType.Cylinder, G["VedelmiPontok"], new Vector3(-18f, 0.1f, 10f), new Vector3(0f, 0f, 0f), new Vector3(2f, 0.1f, 2f), "M_Platform");
        Prim("VedelmiPont_2", PrimitiveType.Cylinder, G["VedelmiPontok"], new Vector3(18f, 0.1f, 10f), new Vector3(0f, 0f, 0f), new Vector3(2f, 0.1f, 2f), "M_Platform");
        Prim("VedelmiPont_3", PrimitiveType.Cylinder, G["VedelmiPontok"], new Vector3(0f, 0.1f, 30f), new Vector3(0f, 0f, 0f), new Vector3(2f, 0.1f, 2f), "M_Platform");
        Empty("KristalyHelyek", G["Jatekmenet"], new Vector3(0f, 0f, 0f));
        Empty("KristalyHely_1", G["KristalyHelyek"], new Vector3(-18f, 0.5f, -8f));
        Empty("KristalyHely_2", G["KristalyHelyek"], new Vector3(20f, 0.5f, 2f));
        Empty("KristalyHely_3", G["KristalyHelyek"], new Vector3(-22f, 0.5f, 30f));

        HookUpSwitch();
        MarkCrystalSpots();

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        Debug.Log("[DSZP] MarsBase_02 kész: " + ScenePath);
    }

    // ---------------- segédfüggvények ----------------

    static void CreateMaterials()
    {
        Mat("M_Talaj", new Color(0.62f, 0.33f, 0.20f), false);
        Mat("M_Platform", new Color(0.78f, 0.78f, 0.80f), false);
        Mat("M_SotetFem", new Color(0.22f, 0.23f, 0.26f), false);
        Mat("M_Epulet", new Color(0.55f, 0.62f, 0.70f), false);
        Mat("M_Jelzofeny", jelzoSzin, true);
    }

    static void Mat(string name, Color c, bool emission)
    {
        string path = "Assets/Materials/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null)
        {
            var sh = Shader.Find("Universal Render Pipeline/Lit");
            if (sh == null)
            {
                Debug.LogWarning("[DSZP] Nem találom az URP/Lit shadert – Universal 3D projektben futtasd!");
                sh = Shader.Find("Standard");
            }
            m = new Material(sh);
            AssetDatabase.CreateAsset(m, path);
        }
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
        if (m.HasProperty("_Color")) m.SetColor("_Color", c);
        if (emission)
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", c * 2f);
            m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
        EditorUtility.SetDirty(m);
        M[name] = m;
    }

    static void SetupCameraAndSun()
    {
        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.SetPositionAndRotation(new Vector3(0f, 14f, -18f), Quaternion.Euler(35f, 0f, 0f));
            cam.fieldOfView = 60f;
        }
        foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            if (l.type == LightType.Directional)
            {
                l.intensity = 0.5f;
                l.color = new Color(1f, 0.85f, 0.75f);
                l.transform.rotation = Quaternion.Euler(30f, -40f, 0f);
            }
    }

    static Transform Place(GameObject go, string name, Transform parent, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        go.name = name;
        if (parent != null) go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localEulerAngles = rot;
        go.transform.localScale = scale;
        G[name] = go.transform;
        return go.transform;
    }

    static void Empty(string name, Transform parent, Vector3 pos)
    {
        Place(new GameObject(), name, parent, pos, Vector3.zero, Vector3.one);
    }

    static void Prim(string name, PrimitiveType type, Transform parent, Vector3 pos, Vector3 rot, Vector3 scale, string mat)
    {
        var go = GameObject.CreatePrimitive(type);
        Place(go, name, parent, pos, rot, scale);
        if (mat != null && M.TryGetValue(mat, out var m))
            go.GetComponent<Renderer>().sharedMaterial = m;
    }

    static void PointLight(string name, Transform parent, Vector3 pos, float range, float intensity, Color color)
    {
        var go = new GameObject();
        Place(go, name, parent, pos, Vector3.zero, Vector3.one);
        var l = go.AddComponent<Light>();
        l.type = LightType.Point;
        l.range = range;
        l.intensity = intensity;
        l.color = color;
        l.shadows = LightShadows.None;
    }

    static void HookUpSwitch()
    {
        // Az EnergiaKapcsolo.cs-nek az Assets/Scripts mappában kell lennie (és le kell fordulnia).
        var type = System.Type.GetType("EnergiaKapcsolo, Assembly-CSharp");
        if (type == null)
        {
            Debug.LogWarning("[DSZP] Nem találom az EnergiaKapcsolo scriptet – a konzol script nélkül készült el. " +
                             "Tedd be az Assets/Scripts mappába, várd meg a fordítást, és futtasd újra a menüpontot.");
            return;
        }
        var comp = G["EnergiaKonzol"].gameObject.AddComponent(type);
        var so = new SerializedObject(comp);
        so.FindProperty("bazisFenyek").objectReferenceValue = G["BazisFenyek"].gameObject;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void MarkCrystalSpots()
    {
        var icon = EditorGUIUtility.IconContent("sv_label_3").image as Texture2D;
        if (icon == null) return;
        foreach (var n in new[] { "KristalyHely_1", "KristalyHely_2", "KristalyHely_3" })
            if (G.TryGetValue(n, out var t)) EditorGUIUtility.SetIconForObject(t.gameObject, icon);
    }
}
#endif
