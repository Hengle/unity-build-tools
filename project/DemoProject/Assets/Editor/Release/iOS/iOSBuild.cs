﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using System.IO;

namespace CC.Release.iOS
{
    internal class iOSBuild: GeneralBuild
    {
        public override BuildTarget Target
        {
            get
            {
                return BuildTarget.iOS;
            }
        }

        public override BuildTargetGroup TargetGroup
        {
            get
            {
                return BuildTargetGroup.iOS;
            }
        }

        public override bool Setup()
        {
            base.Setup();

            PlayerSettings.bundleIdentifier = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.BundleID);
            PlayerSettings.iPhoneBundleIdentifier = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.BundleID);
            PlayerSettings.iOS.buildNumber = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BundleVersionCode];

            return true;
        }

        public override bool Build()
        {
            base.Build();

            var levels = ReleaseConfig.BuildLevels;
            var buildPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BuildPath];
            var buildProjectPath = Path.Combine(buildPath, ReleaseConfig.Setting[ReleaseConfig.SettingDefine.ProjectCodeName]);

            BuildPipeline.BuildPlayer(levels, buildProjectPath, BuildTarget.iOS, BuildOptions.Il2CPP | BuildOptions.ShowBuiltPlayer);

            return true;
        }

        public override bool PostBuild(BuildTarget target, string pathToBuiltProject)
        {
            base.PostBuild(target, pathToBuiltProject);

            if(target != BuildTarget.iOS)
            {
                UnityEngine.Debug.LogError("iOSBuild.PostBuild target error");
                return false;
            }

            XcodeSetting.Setup(pathToBuiltProject);

            XcodeBuild.Init();
            XcodeBuild.Build(pathToBuiltProject);
            return true;
        }
    }
}
