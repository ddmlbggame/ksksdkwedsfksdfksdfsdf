using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEditor.Callbacks;

public class XCodeProjectMod {
	public static string CODE_SIGN_DEVELOPER { get; private set; }
	public static string CODE_SIGN_DISTRIBUTION { get; private set; }
	public static string PROVISIONING_DEVELOPER { get; private set; }
	public static string PROVISIONING_DISTRIBUTION { get; private set; }

	[PostProcessBuild(999)]
	public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
	{
		if (BuildTarget == BuildTarget.iOS)
		{
			Debug.Log(path);
			string projPath = PBXProject.GetPBXProjectPath(path);
			PBXProject proj = new PBXProject();

			proj.ReadFromString(File.ReadAllText(projPath));

			// 友盟报错，添加-lz编译flag
			string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
			string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
			proj.AddFileToBuildWithFlags(target, fileGuid, "-lz");
			proj.SetCompileFlagsForFile(target, fileGuid, null);

			//友盟报错 对所有的编译配置设置选项  
			proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
			proj.AddFrameworkToProject(target, "libsqlite3.tbd", false);
			File.Copy(Application.dataPath + "/Editor/AutoBuild/Xcode/UnityAppController.mm", path + "/Classes/UnityAppController.mm", true);
			////Handle xcodeproj
			//File.Copy(Application.dataPath + "/Editor/xcodeapi/Res/KeychainAccessGroups.plist", path + "/KeychainAccessGroups.plist", true);

			//proj.AddFile("KeychainAccessGroups.plist", "KeychainAccessGroups.plist");

			//var codesign = Debug.isDebugBuild ? CODE_SIGN_DEVELOPER : CODE_SIGN_DISTRIBUTION;
			//var provision = Debug.isDebugBuild ? PROVISIONING_DEVELOPER : PROVISIONING_DISTRIBUTION;

			//proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", codesign);
			//proj.SetBuildProperty(target, "PROVISIONING_PROFILE", provision);
			//proj.SetBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", "KeychainAccessGroups.plist");
			//proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
			////proj.SetGeneralTeam(target, "xxxxx");//fix source code

			//File.WriteAllText(projPath, proj.WriteToString());

			////Handle plist
			//string plistPath = path + "/Info.plist";
			//PlistDocument plist = new PlistDocument();
			//plist.ReadFromString(File.ReadAllText(plistPath));
			//PlistElementDict rootDict = plist.root;

			//rootDict.SetString("CFBundleVersion", GetVer());//GetVer() 返回自定义自增值

			//File.WriteAllText(plistPath, plist.WriteToString());
		}
	}
}

