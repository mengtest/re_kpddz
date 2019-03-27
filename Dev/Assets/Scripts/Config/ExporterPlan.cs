using System.Collections.Generic;
using System.Linq;

public class ExporterPlan
{
    public static readonly string[] EffectExportableFileTypes = { ".prefab"};
    public static readonly string[] RoleExportableFileTypes = { ".prefab", ".fbx"};
    public static readonly string[] SoundExportableFileTypes = { ".wav", ".mp3" };
    public static readonly string[] WindowsExportableFileTypes = { ".prefab", ".ttf"};
    public static readonly string[] LevelExportableFileTypes = { ".unity" };
    public static readonly string[] GameObjectExportableFileTypes = { ".prefab" };

	public static readonly string WindowPath = "Main.Bundles";
    public static readonly string BattlePath = "Battle.Bundles";
    public static readonly string BattleRAMPath = "battle-";
    public static readonly string MainRole = "MyRole-";
    public static readonly string FirstBattle = "FirstBattle-";
    public static readonly string OrderPath = "Order.Bundles";
    public static readonly string DisposePath = "Dispose.Bundles";
	public static readonly string WindowControllerPath = "WindowController";

	
}
