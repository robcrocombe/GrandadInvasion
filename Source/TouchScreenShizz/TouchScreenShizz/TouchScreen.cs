using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone;

namespace touchScreen
{
    public class Screen
    {

        static private int headBox(List<Rectangle> headList, List<Rectangle> touchList)
        {
            for (int i = 0; i < headList.Count; i++)
            {
                foreach (Rectangle t in touchList)
                {
                    if (headList[i].Intersects(t))
                    {
                        return i;
                    }
                }
            }
            return -10;
        }
        static private int bodyBox(List<Rectangle> bodyList, List<Rectangle> touchList)
        {
            for (int i = 0; i < bodyList.Count; i++)
            {
                foreach (Rectangle t in touchList)
                {
                    if (bodyList[i].Intersects(t))
                    {
                        return i;
                    }
                }
            }
            return -10;
        }
        static public int checkTouch(List<Rectangle> headList, List<Rectangle> bodyList, out string type)
        {
            TouchCollection touches = new TouchCollection();
            touches = TouchPanel.GetState();
            List<Rectangle> touchList = new List<Rectangle>();
            if (touches.Count > 0)
            {
                for (int i = 0; i < touches.Count; i++)
                {
                    if (touches[i].State == TouchLocationState.Pressed)
                    {
                        touchList.Add(new Rectangle((int)touches[i].Position.X, (int)touches[i].Position.Y, 5, 5));
                    }
                }
                if (touchList.Count > 0)
                {
                    int head = headBox(headList, touchList);
                    if (head != -10)
                    {
                        type = "head";
                        return head;
                    }
                    else
                    {
                        int body = bodyBox(bodyList, touchList);
                        if (body != -10)
                        {
                            type = "body";
                            return body;
                        }
                    }
                }
            }
            type = "null";
            return -10;
        }

    }
}
