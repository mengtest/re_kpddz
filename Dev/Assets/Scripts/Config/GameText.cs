
#region Using
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using asset;
using FoundationHelper;
#endregion


[SLua.CustomLuaClass]
public class GameText
{
    private const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
    private const string SpecialSymbols = @"[0-9\s-—`~@#\$%\^\&\*\(\)_\+<>\""\{\}\\\/'\[\]]";

    private static string _value;
    private static string _badwords;

    private static Regex _base;
    private static readonly Regex Special = new Regex(SpecialSymbols, Options);
    private static readonly Regex Chinese = new Regex("[\u4e00-\u9fa5]+", Options);

    private readonly Dictionary<string, string> _textData = new Dictionary<string, string>();
    private static readonly GameText Inst = new GameText();

    FilterWord _filterWord = null;


    /// <summary>
    /// 单例
    /// </summary>
    public static GameText Instance
    {
        get { return Inst; }
    }

    /// <summary>
    ///     通过索引获取对应文字
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string Get(string index)
    {
        return _textData.TryGetValue(index, out _value) ? _value : index;
    }

    /// <summary>
    ///     通过索引获取对应文字
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetStr(string index)
    {
        return Instance.Get(index);
    }

    /// <summary>
    ///     添加一条记录，如果 index 已经存在，则覆盖
    /// </summary>
    /// <param name="index"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private void AddOrOverlay(string index, string text)
    {
        _textData[index] = text;
    }

    /// <summary>
    ///     初始化导入数据
    /// </summary>
    /// <returns></returns>
    public void InitData()
    {
        var gameText = AssetManager.getInstance().loadAsset("Config/GameText.xml"); //同步加载XML
        if (gameText == null) return;
        var doc = XDocument.Parse(gameText.ToString());
        if (doc.Root == null) return;
        //TODO：得到每一个text节点，获取text的节点的下 t, value 节点的值，加入 textData 中
        foreach (XElement item in doc.Root.Descendants("text")) {
            AddOrOverlay(item.Attribute("t").Value, item.Attribute("value").Value);
        }

        var replaceFile = AssetManager.getInstance().loadAsset("Config/sensitivewords.txt");//同步加载XML
        if (replaceFile == null) return;
        _badwords = replaceFile.ToString();

        string[] listBadWords = _badwords.Split(new char[] { ',', '\n', '\r' });
        //foreach(var str in listBadWords)
        //{
        //    if (!str.Equals(""))
        //        UnityEngine.Debug.Log(str);
        //}

        _filterWord = new FilterWord(ref listBadWords);

        //_base = new Regex(_badwords, Options);
    }

    /// <summary>
    ///     检测是否含有敏感字符
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool StrCheck(string source)
    {
        if (_filterWord != null)
            return _filterWord.Filter(source, '*') != source;

        return false;
    }

    /// <summary>
    ///     过滤敏感字符
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public string StrFilter(string source, char replaceChar = '*')
    {
        if (ClientDefine.GAME_IN_DEVELOPING == 1)
        {
            if (source.StartsWith("ai") || source.StartsWith("ahe"))
                return source;
        }


        if (_filterWord != null)
            return _filterWord.Filter(source, replaceChar);

        return source;
    }

#region format

    public static string Format(string index)
    {
        var format = Instance.Get(index);
        return format;
    }

    public static string Format(string index, object arg0)
    {
        var format = Instance.Get(index);
        return string.Format(format, arg0);
    }

    public static string Format(string index, params object[] args)
    {
        var format = Instance.Get(index);
        return string.Format(format, args);
    }

    public static string Format(string index, object arg0, object arg1)
    {
        var format = Instance.Get(index);
        return string.Format(format, arg0, arg1);
    }

    public static string Format(string index, object arg0, object arg1, object arg2)
    {
        var format = Instance.Get(index);
        return string.Format(format, arg0, arg1, arg2);
    }

#endregion
}