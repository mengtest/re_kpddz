using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using UnityEngine;

// 资源依赖
public class DependenciesData 
{
	private static DependenciesData _instance;

	private Dictionary<string, string[]> _dependencies;
	private Dictionary<string, string[]> _fullDependencies;
	
	public  DependenciesData()
	{
		if (_instance != null)
			throw new Exception("单件实例错误");
		_instance = this;

		_dependencies = new Dictionary<string, string[]>();
		_fullDependencies = new Dictionary<string, string[]>();
	}
	
	public static DependenciesData GetInstance()
	{
		if (_instance != null)
		{
			return _instance;
		}
		return new DependenciesData();
	}

	public void Init(string text)
	{
		XmlDocument _xmlDoc = new XmlDocument();
		_xmlDoc.LoadXml(text);
		XmlNodeList nodeList = _xmlDoc.SelectSingleNode("root").ChildNodes;
		List<string> dependencies = new List<string>();
		string path;
		foreach (XmlElement node in nodeList)
		{
			path = node.GetAttribute("path");
			dependencies.Clear();
			foreach(XmlElement child in node)
			{
				dependencies.Add(child.GetAttribute("path"));
			}
			_dependencies.Add(path, dependencies.ToArray());
        }

		foreach(var dep in _dependencies)
		{
			_fullDependencies.Add(dep.Key, GetAllDependencies(dep.Value));
		}
    }
    
    public string[] GetDependencies(string path)
    {
        if(_dependencies.ContainsKey(path))
        {
            return _dependencies[path];
		}
		return null;
	}

	public string[] GetFullDependencies(string path)
	{
        if(_fullDependencies.ContainsKey(path))
		{
			return _fullDependencies[path];
		}
		return null;
    }

	public string[] CalculateRealityPaths(string[] paths)
	{
		List<string> result = new List<string>();
		foreach(var path in paths)
		{
			result.Add(path);
		}
		
		int index = 1;
		string current;
		string[] dependencies;
		while(index < result.Count + 1)
		{
			current = result[result.Count - index];
			dependencies = DependenciesData.GetInstance().GetDependencies(current);
			if(dependencies != null)
			{
				foreach(var dep in dependencies)
				{
					if(result.Contains(dep))
					{
						int depIndex = result.IndexOf(dep);
						if(depIndex > result.Count - index)
						{
							result.Remove(dep);
							result.Insert(0, dep);
							index --;
						}
					}
					else
					{
						result.Insert(0, dep);
					}
				}
			}
			index ++;
		}
		
		return result.ToArray();
	}

	public bool IsInit()
	{
		return _dependencies.Count > 0 && _fullDependencies.Count > 0;
	}

	private string[] GetAllDependencies(string[] paths)
	{
		List<string> dependencies = new List<string>();
		dependencies.AddRange(paths);
		foreach(var path in paths)
		{
			string[] dep = GetDependencies(path);
			if(dep !=null && dep.Length > 0)
			{
				dependencies.AddRange(dep);
				string[] childDep = GetAllDependencies(dep);
				if(childDep.Length > 0)
				{
					dependencies.AddRange(childDep);
				}
			}
		}
		return dependencies.ToArray();
	}
}