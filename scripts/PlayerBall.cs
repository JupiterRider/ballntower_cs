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
//  Control the behavior of a pickable object. 

public class PlayerBall : RigidBody
{
    // Indicates if currently carried.
    bool Picked = false;

    Position3D PickPos;
    Camera Camera;

    public override void _Ready()
    {
        PickPos = GetNode<Position3D>("../FirstPersonPlayer/Head_y/Head_x/Camera/PickPos");
        Camera = GetNode<Camera>("../FirstPersonPlayer/Head_y/Head_x/Camera");
    }

    public override void _Process(float delta)
    {   
        // Set the object to sleep and place it in front of the players face.
        if (Picked)
        {
            Sleeping = true;
            GlobalTransform = PickPos.GlobalTransform;
        }
        else
        {
            Sleeping = false;
        }
    }

    // Called by the player when picked up.
    void PickUp()
    {
        Picked = true;
    }

    // Throw the object in the line of sight with the player's strength.
    void Release(float Power)
    {
        Picked = false;
        Vector3 Position = new Vector3();
        Vector3 Energie = new Vector3(Power, Power, Power);
        Vector3 Impulse = ((PickPos.GlobalTransform.origin - Camera.GlobalTransform.origin).Normalized()) * Energie;
        ApplyImpulse(Position, Impulse);
    }
}