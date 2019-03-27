using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.XCodeEditor
{
    public partial class XCPlist : System.IDisposable
    {

		private string filePath;
		List<string> contents = new List<string>();
        public XCPlist(string sPath)
		{
            //filePath = Path.Combine( fPath, "info.plist" );
            filePath = sPath;
            if( !System.IO.File.Exists( filePath ) ) {
                Debug.LogError( filePath +"路径下文件不存在" );
			    return;
			}

            FileInfo projectFileInfo = new FileInfo( filePath );
			StreamReader sr = projectFileInfo.OpenText();
			while (sr.Peek() >= 0) 
			{
				contents.Add(sr.ReadLine());
			}
			sr.Close();

		}
		public void AddKey(string key)
		{
				if(contents.Count < 2)
						return;
				contents.Insert(contents.Count - 2,key);

		}

		public bool ReplaceKey(string key,string replace){
			for(int i = 0;i < contents.Count;i++){
					if(contents[i].IndexOf(key) != -1){
							contents[i] = contents[i].Replace(key,replace);
                            return true;
					}
			}
            return false;
		}

		public void Save()
		{
            File.Delete(filePath);
            StreamWriter saveFile = File.CreateText(filePath);
			foreach(string line in contents)
					saveFile.WriteLine(line);
			saveFile.Close();   
    	}

        public void Process(Hashtable configs)
        {
            if (configs == null)
                return;
            foreach (DictionaryEntry line in configs)
            {
                string key = line.Key.ToString();
                string sValue = line.Value.ToString();
                if (ReplaceKey(key, sValue))
                {
                    continue;
                }
                else
                {
                    AddKey(sValue);
                }
            }
            

        }
		public void Dispose()
		{

		}
    }
}