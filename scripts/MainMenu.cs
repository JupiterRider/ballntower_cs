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
//  User interface displayed at the beginning.

public class MainMenu : Control
{
    Button PlayButton;
    Control SettingsMenu;
    Control Control;
    LineEdit EditMouseSensitivity;

    public override void _Ready()
    {
        PlayButton = GetNode<Button>("Control/PlayButton");
        SettingsMenu = GetNode<Control>("SettingsMenu");
        Control = GetNode<Control>("Control");
        EditMouseSensitivity = GetNode<LineEdit>("SettingsMenu/Control/EditMouseSensitivity");

        // Focus on a button. Required for keyboard navigation.
        PlayButton.GrabFocus();
    }

    public override void _Input(InputEvent @event)
    {
        // Hide the settings menu if visible.
        if (Input.IsActionJustPressed("esc") && All.SettingsOpen)
        {
            SettingsMenu.Call("_on_ExitDiscarding_pressed");
        }
    }

    // Switch to the first level.
    private void _on_PlayButton_pressed()
    {
        GetTree().ChangeScene(All.CurrentLevel);
    }

    // Show the settins menu.
    void _on_SettingsButton_pressed()
    {
        All.SettingsOpen = true;
        SettingsMenu.Show();
        Control.Hide();
        EditMouseSensitivity.GrabFocus();
    }

    // Quit the game.
    void _on_ExitButton_pressed()
    {
        All.QuittingGame = true;
        GetTree().Quit();
    }
}