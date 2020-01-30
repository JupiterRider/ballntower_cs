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
//  User interface displayed during the game phase.

public class PauseMenu : Control
{
    Label LabelTitle;
    Label DescriptionText;
    Control SettingsMenu;
    Button ContinueButton;
    Control Control;
    LineEdit EditMouseSensitivity;

    public override void _Ready()
    {
        LabelTitle = GetNode<Label>("Control/LabelTitle");
        DescriptionText = GetNode<Label>("Control/DescriptionText");
        SettingsMenu = GetNode<Control>("SettingsMenu");
        ContinueButton = GetNode<Button>("Control/ContinueButton");
        Control = GetNode<Control>("Control");
        EditMouseSensitivity = GetNode<LineEdit>("SettingsMenu/Control/EditMouseSensitivity");

        // Print the level name and description on lables.
        LabelTitle.Text = Convert.ToString(GetParent().Get("LevelName"));
        DescriptionText.Text = Convert.ToString(GetParent().Get("LevelDescription"));
    }

    public override void _Input(InputEvent @event)
    {   
        // Show or hide the pause menu.
        if (Input.IsActionJustPressed("esc") && !All.SettingsOpen)
        {   
            if (!GetTree().Paused)
            {
                // Pause the game.
                GetTree().Paused = true;
                // Set the pause menu visible.
                Show();
                // Show the cursor. 
                Input.SetMouseMode(Input.MouseMode.Visible);
                // Focus on a button. Required for keyboard navigation.
                ContinueButton.GrabFocus();
            }
            else 
            {   
                _on_ContinueButton_pressed();
            }
        }
        // Hide the settings menu if visible.
        else if (Input.IsActionJustPressed("esc") && All.SettingsOpen)
        {
            SettingsMenu.Call("_on_ExitDiscarding_pressed");
        }
    }

    // Continue playing.
    void _on_ContinueButton_pressed()
    {
        // End the pause.
        GetTree().Paused = false;
        // Set the pause menu invisible.
        Hide();
        // Hide the cursor and capture the mouse.
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    // Show the settings menu.
    void _on_SettingsButton_pressed()
    {
        All.SettingsOpen = true;
        SettingsMenu.Show();
        Control.Hide();
        EditMouseSensitivity.GrabFocus();
    }

    // Restart the current level.
    void _on_RestartLevelButton_pressed()
    {
        All.CancelScene = true;
        GetTree().Paused = false;
        GetTree().ChangeScene(All.CurrentLevel);
    }

    // Quit the game.
    void _on_ExitButton_pressed()
    {
        All.QuittingGame = true;
        GetTree().Quit();
    }
}