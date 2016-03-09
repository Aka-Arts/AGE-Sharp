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

    }
}
