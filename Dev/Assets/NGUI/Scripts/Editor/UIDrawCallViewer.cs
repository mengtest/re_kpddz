//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Draw Call Viewer shows a list of draw calls created by NGUI and lets you hide them selectively.
/// </summary>

public class UIDrawCallViewer : EditorWindow
{
	static public UIDrawCallViewer instance;
    private bool _bShowActiveList = true;
    private bool _bShowInActiveList = false;

	enum Visibility
	{
		Visible,
		Hidden,
	}

	enum ShowFilter
	{
		AllPanels,
		SelectedPanel,
	}

	Vector2 mScroll = Vector2.zero;

	void OnEnable () { instance = this; }
	void OnDisable () { instance = null; }
	void OnSelectionChange () { Repaint(); }

	/// <summary>
	/// Draw the custom wizard.
	/// </summary>

	void OnGUI ()
	{

		BetterList<UIDrawCall> dcs_active = UIDrawCall.activeList;
        BetterList<UIDrawCall> dcs_inactive = UIDrawCall.inactiveList;

        BetterList<UIDrawCall> dcs = new BetterList<UIDrawCall>();
        if (_bShowActiveList)
        {
            for (int i = 0; i < dcs_active.size; i++)
            {
                dcs.Add(dcs_active[i]);
            }
        }
        if (_bShowInActiveList)
        {
            for (int i = 0; i < dcs_inactive.size; i++ )
            {
                dcs.Add(dcs_inactive[i]);
            }
        }
		dcs.Sort(delegate(UIDrawCall a, UIDrawCall b)
		{
			return a.finalRenderQueue.CompareTo(b.finalRenderQueue);
		});

        GUILayout.BeginHorizontal();
        _bShowActiveList = EditorGUILayout.ToggleLeft("统计可见部分", _bShowActiveList, GUILayout.Width(90f));
        if (!_bShowInActiveList) _bShowActiveList = true;//不能2者都不选
        _bShowInActiveList = EditorGUILayout.ToggleLeft("统计隐藏部分", _bShowInActiveList, GUILayout.Width(90f));
        if (!_bShowActiveList) _bShowInActiveList = true;//不能2者都不选
        GUILayout.EndHorizontal();
		if (dcs.size == 0)
		{
			EditorGUILayout.HelpBox("No NGUI draw calls present in the scene", MessageType.Info);
			return;
		}

		UIPanel selectedPanel = NGUITools.FindInParents<UIPanel>(Selection.activeGameObject);

		GUILayout.Space(12f);

		NGUIEditorTools.SetLabelWidth(100f);
		ShowFilter show = (NGUISettings.showAllDCs ? ShowFilter.AllPanels : ShowFilter.SelectedPanel);

		if ((ShowFilter)EditorGUILayout.EnumPopup("Draw Call Filter", show) != show)
			NGUISettings.showAllDCs = !NGUISettings.showAllDCs;

		GUILayout.Space(6f);

		if (selectedPanel == null && !NGUISettings.showAllDCs)
		{
			EditorGUILayout.HelpBox("No panel selected", MessageType.Info);
			return;
		}

		NGUIEditorTools.SetLabelWidth(80f);
		mScroll = GUILayout.BeginScrollView(mScroll);

		int dcCount = 0;

		for (int i = 0; i < dcs.size; ++i)
		{
			UIDrawCall dc = dcs[i];
			string key = "Draw Call " + (i + 1);
			bool highlight = (selectedPanel == null || selectedPanel == dc.manager);

			if (!highlight)
			{
				if (!NGUISettings.showAllDCs) continue;
				
				if (UnityEditor.EditorPrefs.GetBool(key, true))
				{
					GUI.color = new Color(0.85f, 0.85f, 0.85f);
				}
				else
				{
					GUI.contentColor = new Color(0.85f, 0.85f, 0.85f);
				}
			}
			else GUI.contentColor = Color.white;

			++dcCount;
			string name = key + " of " + dcs.size;
			if (!dc.isActive) name = name + " (HIDDEN)";
			else if (!highlight) name = name + " (" + dc.manager.name + ")";

			if (NGUIEditorTools.DrawHeader(name, key))
			{
				GUI.color = highlight ? Color.white : new Color(0.8f, 0.8f, 0.8f);

				NGUIEditorTools.BeginContents();
				EditorGUILayout.ObjectField("Material", dc.dynamicMaterial, typeof(Material), false);

				int count = 0;

				for (int a = 0; a < UIPanel.list.Count; ++a)
				{
					UIPanel p = UIPanel.list[a];

					for (int b = 0; b < p.widgets.Count; ++b)
					{
						UIWidget w = p.widgets[b];
						if (w.drawCall == dc) ++count;
					}
				}

				string myPath = NGUITools.GetHierarchy(dc.manager.cachedGameObject);
				string remove = myPath + "\\";
				string[] list = new string[count + 1];
				list[0] = count.ToString();
				count = 0;

				for (int a = 0; a < UIPanel.list.Count; ++a)
				{
					UIPanel p = UIPanel.list[a];

					for (int b = 0; b < p.widgets.Count; ++b)
					{
						UIWidget w = p.widgets[b];

						if (w.drawCall == dc)
						{
							string path = NGUITools.GetHierarchy(w.cachedGameObject);
							list[++count] = count + ". " + (string.Equals(path, myPath) ? w.name : path.Replace(remove, ""));
						}
					}
				}

				GUILayout.BeginHorizontal();
				int sel = EditorGUILayout.Popup("Widgets", 0, list);
				NGUIEditorTools.DrawPadding();
				GUILayout.EndHorizontal();

				if (sel != 0)
				{
					count = 0;

					for (int a = 0; a < UIPanel.list.Count; ++a)
					{
						UIPanel p = UIPanel.list[a];

						for (int b = 0; b < p.widgets.Count; ++b)
						{
							UIWidget w = p.widgets[b];

							if (w.drawCall == dc && ++count == sel)
							{
								Selection.activeGameObject = w.gameObject;
								break;
							}
						}
					}
				}

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Render Q", dc.finalRenderQueue.ToString(), GUILayout.Width(120f));
				bool draw = (Visibility)EditorGUILayout.EnumPopup(dc.isActive ? Visibility.Visible : Visibility.Hidden) == Visibility.Visible;
				NGUIEditorTools.DrawPadding();
				GUILayout.EndHorizontal();

				if (dc.isActive != draw)
				{
					dc.isActive = draw;
					NGUITools.SetDirty(dc.manager);
				}

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Triangles", dc.triangles.ToString(), GUILayout.Width(120f));

				if (dc.manager != selectedPanel)
				{
					if (GUILayout.Button("Select the Panel"))
					{
						Selection.activeGameObject = dc.manager.gameObject;
					}
					NGUIEditorTools.DrawPadding();
				}
				GUILayout.EndHorizontal();

				if (dc.manager.clipping != UIDrawCall.Clipping.None && !dc.isClipped)
				{
					EditorGUILayout.HelpBox("You must switch this material's shader to Unlit/Transparent Colored or Unlit/Premultiplied Colored in order for clipping to work.",
						MessageType.Warning);
				}

				NGUIEditorTools.EndContents();
				GUI.color = Color.white;
			}
		}

		if (dcCount == 0)
		{
			EditorGUILayout.HelpBox("No draw calls found", MessageType.Info);
		}
		GUILayout.EndScrollView();
	}
}
