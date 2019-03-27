using UnityEngine;

[DisallowMultipleComponent]
[SLua.CustomLuaClass]
public class ComponentData:MonoBehaviour
{
    public int Tag = 0;
    public int Id = 0;
    public string Text = "";
    public int Value = 0;
    public Color color = Color.white;
    public Color eColor = Color.white;
    public UILabel.Effect effColor = UILabel.Effect.None;
    public string[] Params;
    public string Name = "";

    public static ComponentData Get(GameObject go) {
        return go.GetComponent<ComponentData>() ?? go.AddComponent<ComponentData>();
    }
}