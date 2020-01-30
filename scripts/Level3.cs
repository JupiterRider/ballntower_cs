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
//  Information about the current Level.
//  Modify children in the game scene hierarchy.
//  Control the level goal.

public class Level3 : Spatial
{
    // Information about the Level.
    string LevelName = "Level 3";
    string LevelDescription = "> Get the cans off the table again. <";

    DirectionalLight Sun;
    WorldEnvironment WorldEnvironment;

    // Value for counted cans.
    int RemainingCans;

    public override void _Ready()
    {
        Sun = GetNode<DirectionalLight>("Sun");
        WorldEnvironment = GetNode<WorldEnvironment>("WorldEnvironment");

        // Updates values in All.cs.
        All.CurrentLevel = "res://scenes/Level3.tscn";
        All.NextLevel = "res://scenes/Level1.tscn";
        All.CancelScene = false;

        Graphics();
    }

    // Set Ambient occlusion and shadows depending on the values.
    void Graphics()
    {
        Sun.ShadowEnabled = All.Shadows;
        WorldEnvironment.Environment.SsaoEnabled = All.Shadows;
    }

    // Called when a can left the area.
    void _on_Area_body_exited(Node body)
    {
        if (body.Name.Contains("Can"))
        {   
            // Count down.
            RemainingCans--;

            // Change scene if no can remains and 
            // the game isn't already about to quit or switch the scene.
            if (RemainingCans == 0 && !All.CancelScene && !All.QuittingGame)
            {
                GetTree().ChangeScene("res://scenes/EndMenu.tscn");
            }
        }
    }

    // Called when a can entered the area.
    void _on_Area_body_entered(Node body)
    {
        if (body.Name.Contains("Can"))
        {
            // Count up.
            RemainingCans++;
        }
    }
}