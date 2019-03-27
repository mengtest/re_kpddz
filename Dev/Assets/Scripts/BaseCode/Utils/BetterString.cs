/***************************************************************

 *
 *
 * Filename:  	BetterString.cs	
 * Summary: 	事件管理
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2017/06/29 19:22
 ***************************************************************/


public class BetterString {

    /// <summary>
    /// intern string: only one copy in memory, but can't release by GC;
    /// </summary>
    /// <param name="s1"></param>
    /// <returns></returns>
    public static string ConstString(string s1)
    {
        return string.Intern(s1);
    }

    /// <summary>
    /// cancat strings, can release by GC
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string Builder(params string[] values)
    {
        if (values.Length == 0)
            return null;
        else if (values.Length == 1)
            return values[0];
        else
        {
            int length = 1;
            for (int i=0; i < values.Length; i++)
            {
                if (values[i] != null)
                    length += values[i].Length;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                    sb.Append(values[i]);
            }
            return sb.ToString();
        }
    }
    /// <summary>
    /// cancat strings, and save as const string
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string ConstBuilder(params string[] values)
    {
        if (values.Length == 0)
            return null;
        else if (values.Length == 1)
            return values[0];
        else
        {
            int length = 1;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                    length += values[i].Length;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                    sb.Append(values[i]);
            }
            return string.Intern(sb.ToString());
        }
    }

    public static void GC()
    {
        System.GC.Collect();
    }
    
    
}
