using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using System.IO;
#endif

namespace PhenomTools
{
  public static class EnvironmentController
  {
    private const string ENV_DEV = "ENV_DEV";
    private const string ENV_STAGING = "ENV_STAGING";
    private const string ENV_PROD = "ENV_PROD";
      
    public enum EEnvironmentType
    {
      dev = 0,
      staging = 1,
      production = 2
    }

    public static EEnvironmentType EnvironmentType
    {
      get
      {
#if ENV_PROD
        return EEnvironmentType.production;
#elif ENV_STAGING
        return EEnvironmentType.staging;
#else
        return EEnvironmentType.dev;
#endif
      }
#if UNITY_EDITOR
      set
      {
        ConfigurePluginServicesFiles(EnvironmentType, value);
        
        string define = EnvironmentTypeToDefine(value);
        AddDefineToBuildTarget(define, NamedBuildTarget.Android);
        AddDefineToBuildTarget(define, NamedBuildTarget.iOS);
        AddDefineToBuildTarget(define, NamedBuildTarget.Standalone);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.LogFormat("Environment set as type: {0}", value);
      }
#endif
    }

    public static bool IsDev => EnvironmentType == EEnvironmentType.dev;
    public static bool IsStaging => EnvironmentType == EEnvironmentType.staging;
    public static bool IsProd => EnvironmentType == EEnvironmentType.production;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitEnvironment()
    {
      InitAsync().Forget();

      async UniTask InitAsync()
      {
        EEnvironmentType type = EnvironmentType;
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(type.ToString());
        await UnityServices.InitializeAsync(options);
      }
    }

#if UNITY_EDITOR
    private static void AddDefineToBuildTarget(string define, NamedBuildTarget buildTarget)
    {
      List<string> defines = PlayerSettings.GetScriptingDefineSymbols(buildTarget).Split(";").ToList();
      for (int i = 0; i < defines.Count; i++)
      {
        if (defines[i].Contains("ENV_"))
          defines.RemoveAt(i);
      }

      defines.Add($";{define}");
      PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines.ToArray());
    }
    
    private const string EditorMenuText = "Abyssal Games/Set Environment/";

    [MenuItem(EditorMenuText + "Dev")]
    public static void SetDev() => EnvironmentType = EEnvironmentType.dev;

    // [MenuItem(EditorMenuText + "Staging")]
    public static void SetStaging() => EnvironmentType = EEnvironmentType.staging;

    [MenuItem(EditorMenuText + "Prod")]
    public static void SetProd() => EnvironmentType = EEnvironmentType.production;

    [MenuItem(EditorMenuText + "Dev", true)]
    private static bool IsSetBuildForDev()
    {
      Menu.SetChecked($"{EditorMenuText}Dev", IsDev);
      return !IsDev;
    }

    // [MenuItem(EditorMenuText + "Staging", true)]
    private static bool IsSetBuildForStaging()
    {
      Menu.SetChecked($"{EditorMenuText}Staging", IsStaging);
      return !IsStaging;
    }

    [MenuItem(EditorMenuText + "Prod", true)]
    private static bool IsSetBuildForProd()
    {
      Menu.SetChecked($"{EditorMenuText}Prod", IsProd);
      return !IsProd;
    }
    
    private static void ConfigurePluginServicesFiles(EEnvironmentType prevType, EEnvironmentType newType)
    {
      string subtext = GetFileSubtextForEnv(prevType);
      string pluginsPath = $"{Application.dataPath}/Plugins/";
      string manifestPath = $"{pluginsPath}Android/";

      //Append subtext to the previous environment file names 
      if (!File.Exists($"{manifestPath}AndroidManifest{subtext}.xml"))
      {
        File.Move($"{manifestPath}AndroidManifest.xml", $"{manifestPath}AndroidManifest{subtext}.xml");
        File.Delete($"{manifestPath}AndroidManifest.xml");
      }
      if (!File.Exists($"{pluginsPath}google-services{subtext}.json"))
      {
        File.Move($"{pluginsPath}google-services.json", $"{pluginsPath}google-services{subtext}.json");
        File.Delete($"{pluginsPath}google-services.json");
      }
      if (!File.Exists($"{pluginsPath}GoogleService-Info{subtext}.plist"))
      {
        File.Move($"{pluginsPath}GoogleService-Info.plist", $"{pluginsPath}GoogleService-Info{subtext}.plist");
        File.Delete($"{pluginsPath}GoogleService-Info.plist");
      }
      if (!File.Exists($"{Application.streamingAssetsPath}/google-services-desktop{subtext}.json"))
      {
        File.Move($"{Application.streamingAssetsPath}/google-services-desktop.json", $"{Application.streamingAssetsPath}/google-services-desktop{subtext}.json");
        File.Delete($"{Application.streamingAssetsPath}/google-services-desktop.json");
      }

      subtext = GetFileSubtextForEnv(newType);

      //Remove subtext from the current environment file names 
      if (File.Exists($"{manifestPath}AndroidManifest{subtext}.xml"))
      {
        File.Move($"{manifestPath}AndroidManifest{subtext}.xml", $"{manifestPath}AndroidManifest.xml");
        File.Delete($"{manifestPath}AndroidManifest{subtext}.xml");
      }
      if (File.Exists($"{pluginsPath}google-services{subtext}.json"))
      {
        File.Move($"{pluginsPath}google-services{subtext}.json", $"{pluginsPath}google-services.json");
        File.Delete($"{pluginsPath}google-services{subtext}.json");
      }
      if (File.Exists($"{pluginsPath}GoogleService-Info{subtext}.plist"))
      {
        File.Move($"{pluginsPath}GoogleService-Info{subtext}.plist", $"{pluginsPath}GoogleService-Info.plist");
        File.Delete($"{pluginsPath}GoogleService-Info{subtext}.plist");
      }
      if (File.Exists($"{Application.streamingAssetsPath}/google-services-desktop{subtext}.json"))
      {
        File.Move($"{Application.streamingAssetsPath}/google-services-desktop{subtext}.json", $"{Application.streamingAssetsPath}/google-services-desktop.json");
        File.Delete($"{Application.streamingAssetsPath}/google-services-desktop{subtext}.json");
      }
    }

    private static EEnvironmentType DefineToEnvironmentType(string define)
    {
      return define switch
      {
        ENV_PROD => EEnvironmentType.production,
        ENV_STAGING => EEnvironmentType.staging,
        _ => EEnvironmentType.dev
      };
    }

    private static string EnvironmentTypeToDefine(EEnvironmentType environmentType)
    {
      return environmentType switch
      {
        EEnvironmentType.production => ENV_PROD,
        EEnvironmentType.staging => ENV_STAGING,
        _ => ENV_DEV
      };
    }

    private static string GetFileSubtextForEnv(EEnvironmentType envType)
    {
      return envType switch
      {
        EEnvironmentType.dev => "_dev",
        EEnvironmentType.staging => "_staging",
        EEnvironmentType.production => "_prod",
        _ => throw new ArgumentOutOfRangeException()
      };
    }
#endif
  }
}