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
//  User interface displayed after finishing a level.

public class EndMenu : Control
{
    Button NextLevelButton;
    Label LabelTitle;
    Control SettingsMenu;
    Control Control;
    LineEdit EditMouseSensitivity;

    public override void _Ready()
    {
        NextLevelButton = GetNode<Button>("Control/NextLevelButton");
        LabelTitle = GetNode<Label>("Control/LabelTitle");
        SettingsMenu = GetNode<Control>("SettingsMenu");
        Control = GetNode<Control>("Control");
        EditMouseSensitivity = GetNode<LineEdit>("SettingsMenu/Control/EditMouseSensitivity");

        // Set cursor visible.
        Input.MouseMode = Input.MouseModeEnum.Captured;
        // Focus on a button. Required for keyboard navigation.
        NextLevelButton.GrabFocus();

        // Print the required time in minutes:seconds.
        if (All.Seconds < 10)
        {
            LabelTitle.Text = ($"You won in {Convert.ToString(All.Minutes)}:0{Convert.ToString(All.Seconds)}");
        }
        else
        {
            LabelTitle.Text = ($"You won in {Convert.ToString(All.Minutes)}:{Convert.ToString(All.Seconds)}");
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Hide the settings menu if visible.
        if (Input.IsActionJustPressed("esc") && All.SettingsOpen)
        {
            SettingsMenu.Call("_on_ExitDiscarding_pressed");
        }
    }

    // Switch to the next level.
    void _on_NextLevelButton_pressed()
    {
        GetTree().ChangeScene(All.NextLevel);
    }

    // Play the current level again.
    void _on_PlayAgainButton_pressed()
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
