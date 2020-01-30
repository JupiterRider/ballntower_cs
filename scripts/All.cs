// MIT License

// Copyright (c) 2019-2020 JupiterRider

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Godot;
using System;

// Purpose:
//	This script gets autoloaded via project settings.
//	It stores global values (variables, functions, etc.) which must
//	be accessed by other nodes/scripts in different scene hierarchies.

public class All : Node
{
	// Times since the level started.
	public static float ElapsedTime = 0;
	public static float Minutes = 0;
	public static float Seconds = 0;

	// Current and next level to start.
	public static string CurrentLevel = "res://scenes/Level1.tscn";
	public static string NextLevel = "res://scenes/Level2.tscn";

	// Set true before scene changes.
	public static bool CancelScene = false;
	// Set true before game quits.
	public static bool QuittingGame = false;
	// Set true if settings menu is visible.
	public static bool SettingsOpen = false;

	// File to store settings.
	public static string ConfigPath = "user://settings.cfg";
	public static ConfigFile Config;

	// RID required for VisualServer changes.
	static RID GameRID;

	// Values for various graphics options:
	//	Mouse speed multiplier.
	public static float MouseSensitivity = 0.05f;
	//	Fullscreen mode.
	public static bool Fullscreen = false;
	//	Ambient occlusion and shadows.
	public static bool Shadows = false; 
	//	VSync.
	public static bool VSync = true;
	//	FPS indicator.
	public static bool ShowFps = true;
	//	Multisample anti aliasing.
	public static int AAValue = 0;
	//	Force FPS value.
	public static int LockFpsValue = 0;

	public override void _Ready()
	{	
		// Same decimal separator for every country to avoid issues with input and output of float values. 
		System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
		
		Config = new ConfigFile();

		// Read config file or generate if none exists.
		if (Config.Load(ConfigPath) == 0)
		{
			ReadSettings();
		}
		else
		{
			SaveSettings();
		}

		GameRID = GetTree().Root.GetViewportRid();

		UseSettings();
	}

	public static void SaveSettings()
	{
		// Save values.
		Config.SetValue("Input", "MouseSensitivity", MouseSensitivity);
		Config.SetValue("Output", "Fullscreen", Fullscreen);
		Config.SetValue("Graphics", "Shadows", Shadows);
		Config.SetValue("Output", "VSync", VSync);
		Config.SetValue("Output", "ShowFps", ShowFps);
		Config.SetValue("Graphics", "MSAA", AAValue);
		Config.SetValue("Output", "ForceFPS", LockFpsValue);

		// Store in file.
		Config.Save(ConfigPath);
	}

	// Read the file.
	public static void ReadSettings()
	{ 
		MouseSensitivity = Convert.ToSingle(Config.GetValue("Input", "MouseSensitivity"));
		Fullscreen = Convert.ToBoolean(Config.GetValue("Output", "Fullscreen"));
		Shadows = Convert.ToBoolean(Config.GetValue("Graphics", "Shadows"));
		VSync = Convert.ToBoolean(Config.GetValue("Output", "VSync"));
		ShowFps = Convert.ToBoolean(Config.GetValue("Output", "ShowFps"));
		AAValue = Convert.ToInt32(Config.GetValue("Graphics", "MSAA"));
		LockFpsValue = Convert.ToInt32(Config.GetValue("Output", "ForceFPS"));
	}

	// Apply the defined values.
	public static void UseSettings()
	{	
		OS.WindowFullscreen = Fullscreen;
		OS.VsyncEnabled = VSync;

		var AA = (VisualServer.ViewportMSAA)AAValue;
		VisualServer.ViewportSetMsaa(GameRID, AA);

		Engine.TargetFps = LockFpsValue;
	}
}