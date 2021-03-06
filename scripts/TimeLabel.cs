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
//  Record the play times.

public class TimeLabel : Label
{
    public override void _Ready()
    {   
        // Reset the timer.
        All.ElapsedTime = 0;
    }

    public override void _PhysicsProcess(float delta)
    {
        // Record the time.
        All.ElapsedTime += delta;

        // Convert the time in minutes and seconds.
        All.Minutes = Convert.ToInt32(All.ElapsedTime)/60;
        All.Seconds = Convert.ToInt32(All.ElapsedTime)%60;
        
        // Print the time on a label.
        if (All.Seconds < 10)
        {   
            // Put a zero in front of the seconds if below 10.
            Text = ($"{ Convert.ToString(All.Minutes) }:0{ Convert.ToString(All.Seconds) }");
        }
        else
        {
            Text = ($"{ Convert.ToString(All.Minutes) }:{ Convert.ToString(All.Seconds) }");
        }
    }
}