#!/bin/sh

#参数判断  
if [ $# != 1 ];then  
    echo "需要一个参数。 参数是游戏包的名子"  
    exit     
fi  


#UNITY程序的路径#
UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity

#当前路径#
PWD = 'pwd'
#游戏程序路径#
PROJECT_PATH=${PWD}/..
# echo $PROJECT_PATH
# exit;

#IOS打包脚本路径#
BUILD_IOS_PATH=${PWD}/buildios.sh

#生成的Xcode工程路径#
XCODE_PATH=${PROJECT_PATH}/TargetIOS/$1
# echo $XCODE_PATH
#将unity导出成xcode工程#
$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildForIPhone project-$1 -quit

echo "XCODE工程生成完毕"

#更改下一个sh权限
chmod +x $BUILD_IOS_PATH

#开始生成ipa#
$BUILD_IOS_PATH $XCODE_PATH $1

echo "ipa生成完毕"