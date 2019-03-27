关于该资源目录的几点说明：

1、该文件夹只存放 Editor Scripts所引用的相关资源

2、该文件夹的资源不会被包含在build中

3、访问该文件夹下的资源需要使用 EditorGUIUtility.Load 接口。

4、访问时需要指定扩展名，比如"myPlugin/mySkin.guiskin" 而不是 "myPlugin/mySkin"

5、调用Object.Destroy和EditorUtility.UnloadUnusedAssets来释放EditorGUIUtility.Load占用的内存

6、只能有一个“Editor Default Resources”文件夹存在，而且必须直接放在 Assets目录下。