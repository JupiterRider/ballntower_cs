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
//  User interface to control some settings.

public class SettingsMenu : Control
{   
    LineEdit EditMouseSensitivity;
    Button ButtonFullscreen;
    Button ButtonShadows;
    Button ButtonVSync;
    LineEdit EditLockFPS; 
    Button ButtonShowFPS;
    Button ButtonAA;

    // Temporary values:
    //  Discarded when the player doesn't save.
    float MouseSensitivityTemp;
    bool FullscreenTemp;
    bool ShadowsTemp;
    bool VSyncTemp;
    int LockFpsValueTemp;
    bool ShowFpsTemp;
    bool AA;

    public override void _Ready()
    {
        EditMouseSensitivity = GetNode<LineEdit>("Control/EditMouseSensitivity");
        ButtonFullscreen = GetNode<Button>("Control/EditMouseSensitivity/ButtonFullscreen");
        ButtonShadows = GetNode<Button>("Control/EditMouseSensitivity/ButtonFullscreen/ButtonVSync/EditLockFPS/ButtonShowFPS/ButtonAA/ButtonShadows");
        ButtonVSync = GetNode<Button>("Control/EditMouseSensitivity/ButtonFullscreen/ButtonVSync");
        EditLockFPS = GetNode<LineEdit>("Control/EditMouseSensitivity/ButtonFullscreen/ButtonVSync/EditLockFPS");
        ButtonShowFPS = GetNode<Button>("Control/EditMouseSensitivity/ButtonFullscreen/ButtonVSync/EditLockFPS/ButtonShowFPS");
        ButtonAA = GetNode<Button>("Control/EditMouseSensitivity/ButtonFullscreen/ButtonVSync/EditLockFPS/ButtonShowFPS/ButtonAA");

        GetValues();
        DisplayValues();
    }

    // Toggle fullscreen mode.
    void _on_ButtonFullscreen_pressed()
    {
        FullscreenTemp = !FullscreenTemp;
        ButtonFullscreen.Text = Convert.ToString(FullscreenTemp);
    }

    // Toggle Ambient occlusion and shadows.
    void _on_ButtonShadows_pressed()
    {
        ShadowsTemp = !ShadowsTemp;
	    ButtonShadows.Text = Convert.ToString(ShadowsTemp);
    }

    // Toggle VSync.
    void _on_ButtonVSync_pressed()
    {
        VSyncTemp = !VSyncTemp;
	    ButtonVSync.Text = Convert.ToString(VSyncTemp);
    }

    void GetFpsLock()
    {        
        if (LockFpsValueTemp == 0)
        {   
            // Display "False" if the value is 0.
            EditLockFPS.Text = "False";
        }
        else
        {   
            // Display the value.
            EditLockFPS.Text = Convert.ToString(LockFpsValueTemp);
        }
    }

    // Toggle the fps indicator.
    void _on_ButtonShowFPS_pressed()
    {
        ShowFpsTemp = !ShowFpsTemp;
        ButtonShowFPS.Text = Convert.ToString(ShowFpsTemp);
    }

    // Toggle anti aliasing.
    void _on_ButtonAA_pressed()
    {
        AA = !AA;
        ButtonAA.Text = Convert.ToString(AA);
    }

    // Get and display the anti aliasing value. 
    void GetAA()
    {
        if (All.AAValue == 0)
        {
            AA = false;
        }
        else
        {
            AA = true;
        }

        ButtonAA.Text = Convert.ToString(AA);
    }

    // Save the anti aliasing value.
    void SetAA()
    {
        if (AA)
        {
            All.AAValue = 2;
        }
        else
        {
            All.AAValue = 0;
        }
    }

    // Adjust the mouse sensitivity.
    void _on_EditMouseSensitivity_text_changed(float Value)
    {   
        // Values between 0.001 and 1 are allowed. 
        if (Value >= 0.001f && Value <= 1f)
        {
            MouseSensitivityTemp = Value;
        }
    }

    // Display the temp value when the player hits enter in the LineEdit.
    void _on_EditMouseSensitivity_text_entered(float Value)
    {
        EditMouseSensitivity.Text = Convert.ToString(MouseSensitivityTemp);
    }

    // Adjust the force fps value.
    void _on_EditLockFPS_text_changed(int Value)
    {   
        // Values between 30 and 240 are allowed. 
        if (Value >= 30 && Value <= 240)
        {
            LockFpsValueTemp = Value;
        }
        // 0 means Unrestricted.
        else if(Value == 0)
        {
            LockFpsValueTemp = Value;
        }
    }

    // Display the temp value when the player hits enter in the LineEdit.
    void _on_EditLockFPS_text_entered(int Value)
    {
        if (Value == 0)
        {   
            // 0 is shown as "False".
            EditLockFPS.Text = "False";
        }
        else
        {
            EditLockFPS.Text = Convert.ToString(LockFpsValueTemp);
        }
    }

    // Commit the settings.
    void _on_ExitSave_pressed()
    {
        // Set global values equals temp values.
        All.MouseSensitivity = MouseSensitivityTemp;
        All.Fullscreen = FullscreenTemp;
	    All.Shadows = ShadowsTemp;
	    All.VSync = VSyncTemp;
	    All.ShowFps = ShowFpsTemp;
        All.LockFpsValue = LockFpsValueTemp;
	    SetAA();

        // Synchronize the text properties.
        DisplayValues();

        // Make the global values persistent.
        All.SaveSettings();

        // Apply the settings.
        All.UseSettings();

        // Close the settings menu.
        All.SettingsOpen = false;
        GetParent().GetNode<Control>("Control").Show();
        Hide();
        GetParent().GetNode<Button>("Control/SettingsButton").GrabFocus();

        // Set Ambient occlusion and shadows at runtime.
        if(GetParent().GetParent().HasMethod("Graphics"))
        {
            GetParent().GetParent().Call("Graphics");
        }

        // Make sure the text properties are correct.
        GetFpsLock();
	    GetAA();
    }

    void _on_ExitDiscarding_pressed()
    {   
        // Reset temp values.
        GetValues();
        // Reset text properties.
        DisplayValues();

        // Close the settings menu.
        All.SettingsOpen = false;
        GetParent().GetNode<Control>("Control").Show();
        Hide();
        GetParent().GetNode<Button>("Control/SettingsButton").GrabFocus();
    }

    // Synchronize the temp values.
    void GetValues()
    {
        MouseSensitivityTemp = All.MouseSensitivity;
	    FullscreenTemp = All.Fullscreen;
	    ShadowsTemp = All.Shadows;
	    VSyncTemp = All.VSync;
	    ShowFpsTemp = All.ShowFps;
        LockFpsValueTemp = All.LockFpsValue;
    	GetFpsLock();
	    GetAA();
    }

    //Display the temp values in the text property.
    void DisplayValues()
    {
        EditMouseSensitivity.Text = Convert.ToString(MouseSensitivityTemp);
        ButtonFullscreen.Text = Convert.ToString(FullscreenTemp);
        ButtonShadows.Text = Convert.ToString(ShadowsTemp);
        ButtonVSync.Text = Convert.ToString(VSyncTemp);
        ButtonShowFPS.Text = Convert.ToString(ShowFpsTemp);
    }
}