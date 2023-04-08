using OWML.Common;
using OWML.ModHelper;
using System.IO;
using UnityEngine;

namespace ColorCameras { 

public class ColorCameras : ModBehaviour
{
	private void Awake()
	{
		// You won't be able to access OWML's mod helper in Awake.
		// So you probably don't want to do anything here.
		// Use Start() instead.
	}

	private void Start()
	{
		LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
		{
			ModHelper.Events.Unity.FireOnNextUpdate(() => {
				var cameras = Resources.FindObjectsOfTypeAll<OWCamera>();
				for (int i = 0; i < cameras.Length; i++)
                {
					if (cameras[i].postProcessingSettings != null) cameras[i].postProcessingSettings.userLutEnabled = false;
				}

				if (loadScene == OWScene.SolarSystem || loadScene == OWScene.EyeOfTheUniverse)
                {
					if (loadScene != OWScene.EyeOfTheUniverse)
                    {
						var landingCamera = Locator.GetShipTransform().Find("Module_Cockpit/Systems_Cockpit/LandingCamera");
						var lut = GetTexture("landingcamlut.png");
						lut.anisoLevel = 0;
						landingCamera.GetComponent<LandingCamera>()._landingCameraLUT = lut;
						landingCamera.GetComponent<LandingCamera>().Awake();
					}

					ModHelper.Events.Unity.RunWhen(PlayerState.IsWearingSuit, () => 
					{
						var hud = Locator.GetPlayerTransform().Find("PlayerCamera/Helmet/HUDController").GetComponent<HUDCanvas>();
						var scoutMat = hud._hudProbeLauncherUI._material;
						scoutMat.SetColor("_Color", new Color(1f, 0.75f, 0.75f, 1f));
					});
				}
			});
		};
	}

	public Texture2D GetTexture(string filename)
    {
		string path = ModHelper.Manifest.ModFolderPath + filename;
		ModHelper.Console.WriteLine("Loading texture from " + path);
		byte[] data = File.ReadAllBytes(path);
		Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false, true);
		tex.LoadImage(data);
		return tex;
    }
}
}

