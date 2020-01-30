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
//  Script to control a first person character and some UI features.

public class FirstPersonPlayer : KinematicBody
{
    Spatial HeadY;
    Spatial HeadX;
    RayCast Tail;
    RayCast InteractionRay;
    Position3D PickPos;
    Label InteractionText;
    ProgressBar ProgressBar;
    Label DescriptionText;
    Label FPSCounter;

    Vector3 Velocity = new Vector3();

    int Speed;

    float ViewAngle = 0f;

    float GravitySpeed = 0f;

    readonly float Gravity = 78.4f;

    readonly int JumpHeight = 20;

    bool Picked = false;

    float Power = 0f;

    bool InRange = false;

    public override void _Ready()
    {   
        // Hide the cursor and capture the mouse.
        Input.SetMouseMode(Input.MouseMode.Captured);

        HeadY = GetNode<Spatial>("Head_y");
        HeadX = GetNode<Spatial>("Head_y/Head_x");
        Tail = GetNode<RayCast>("RayCast");
        InteractionRay = GetNode<RayCast>("Head_y/Head_x/Camera/InteractionRay");
        PickPos = GetNode<Position3D>("Head_y/Head_x/Camera/PickPos");
        InteractionText = GetNode<Label>("Hud/InteractionText");
        ProgressBar = GetNode<ProgressBar>("Hud/PowerProgressBar");
        DescriptionText = GetNode<Label>("Hud/DescriptionText");
        FPSCounter = GetNode<Label>("Hud/FPSCounter");

        // Display the name of the current level.
        DescriptionText.Text = Convert.ToString(GetParent().Get("LevelName"));
    }

    public override void _Process(float delta)
    {
        Walk(delta);

        PickableInRange();

        Text();

        PowerUp(delta);

        // Show the fps label if the value is true.
        if (All.ShowFps)
        {
            CountFps();
        }
        else
        {
            FPSCounter.Hide();
        }
    }

    void Walk(float delta)
    {
        // Move the player relative to the line of sight.
        Vector3 Direction = new Vector3();

        var Aim = HeadY.GlobalTransform.basis;

        if (Input.IsActionPressed("up"))
        {
            Direction -= Aim.z;
        }
        else if (Input.IsActionPressed("down"))
        {
            Direction += Aim.z;
        }

        if (Input.IsActionPressed("left"))
        {
            Direction -= Aim.x;
        }
        else if (Input.IsActionPressed("right"))
        {
            Direction += Aim.x;
        }

        // Move faster or slower.
        if (Input.IsActionPressed("shift"))
        {
            Speed = 14;
        }
        else if (Input.IsActionPressed("ctrl"))
        {
            Speed = 3;
        }
        else
        {
            Speed = 8;
        }

        // Calculate the movement on all three axis.
        GravitySpeed -= Gravity * delta;
        Velocity = Direction.Normalized() * Speed;
        Velocity.y = GravitySpeed;

        // Jump if the player is on the floor.
        if (Input.IsActionJustPressed("space") && Tail.IsColliding())
        {
            Velocity.y = JumpHeight;
        }

        // Moves the player to the specified directions.
        GravitySpeed = MoveAndSlide(Velocity).y;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion EventMouseMotion)
        {   
            // Turn the head horizontally.
            HeadY.RotateY(Mathf.Deg2Rad(-EventMouseMotion.Relative.x * All.MouseSensitivity));

            // Turn the head vertically, but not more than 90 degrees each direction. 
            var ViewChange = -EventMouseMotion.Relative.y * All.MouseSensitivity;
            if (ViewChange + ViewAngle < 90 && ViewChange + ViewAngle > -90)
            {
                HeadX.RotateX(Mathf.Deg2Rad(ViewChange));
                ViewAngle += ViewChange;
            }
        }
        
        // Pick up an object if it is nearby and has not already been picked up.
        if (Input.IsActionJustPressed("f") && InRange && !Picked)
        {
            Pick();
        }

        // Throw the picked object. 
        if (Input.IsActionJustReleased("lmb") && InRange && Picked)
        {
            Throw();
        }
    }

    // Call the objects function and reset the power value.
    void Pick()
    {
        InteractionRay.GetCollider().Call("PickUp");
        Picked = true;
        Power = 0;
        ProgressBar.Value = Power;
    }

    // Call the objects function including the power value.
    void Throw()
    {
        InteractionRay.GetCollider().Call("Release", Power);
        Picked = false;
    }

    // Display text if a pickable object is spotted.
    void Text()
    {
        if (InRange && !Picked)
        {
            InteractionText.Text = "[F] to pick up";
        }
        else
        {
            InteractionText.Text = "";
        }
    }

    // Indicats if a pickable object is spotted.
    void PickableInRange()
    {
        if (InteractionRay.IsColliding())
        {
            if (InteractionRay.GetCollider().HasMethod("PickUp"))
            {
                InRange = true;
            }
            else
            {
                InRange = false;
            }
        }
        else
        {
            InRange  = false;
        }
    }

    // Increase the power value when an object has been picked up.
    void PowerUp(float delta)
    {
        if (Input.IsActionPressed("lmb") && InRange && Picked)
        {
            if (Power < 300)
            {
                Power += 200 * delta;
                ProgressBar.Value = Power;
            }
        }
    }

    // Display the current frames per second on the label.
    void CountFps()
    {
        FPSCounter.Show();
        FPSCounter.Text = Convert.ToString(Engine.GetFramesPerSecond());
    }
}
