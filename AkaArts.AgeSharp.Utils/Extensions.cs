using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils
{
    public static class Extensions
    {

        #region SafeAudioPlay

        public static bool TryPlay(this SoundEffect sfx)
        {

            var result = false;

            try
            {
                result = sfx.Play();
            }
            catch (Exception)
            {
                // black-hole
            }

            return result;

        }

        public static bool TryPlay(this SoundEffect sfx, float volume, float pitch, float pan)
        {

            var result = false;

            try
            {
                result = sfx.Play(volume, pitch, pan);
            }
            catch (Exception)
            {
                // black-hole
            }

            return result;

        }

        #endregion

        #region ArrayShuffle

        public static void Shuffle<T>(this Random rand, T[] array)
        {

            int length = array.Length;

            while (length > 1)
            {
                int index = rand.Next(length--);
                T temp = array[length];
                array[length] = array[index];
                array[index] = temp;
            }

        }

        #endregion

        #region VECTOR2

        public static bool IsOrthogonalTo(this Vector2 v1, Vector2 v2)
        {
            var v1ToX = Math.Atan2(v1.Y, v1.X);
            var v2ToX = Math.Atan2(v2.Y, v2.X);

            if (v1ToX < 0)
            {
                v1ToX += Math.PI; // take the positive angle
            }

            if (v2ToX < 0)
            {
                v2ToX += Math.PI; // dito
            }

            var v1ToV2 = Math.Abs(v1ToX - v2ToX); // smallest between the 2 vectors

            var epsilon = 1e-5;

            if (v1ToV2 >= MathHelper.PiOver2 - epsilon && v1ToV2 <= MathHelper.PiOver2 + epsilon)
            {
                return true;
            }
            else{
                return false;
            }
        }

        #endregion
    }
}
