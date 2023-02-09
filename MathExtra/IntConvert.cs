namespace EM
{
    public static class MathExtra
    {
        /// <summary>
        /// Convert normal int to short term string
        /// For small value only, not suitable for big numer
        /// </summary>
        public static string ShortNumberString(this int thisInt)
        {
            string shortNumberString = thisInt.ToString();

            if (thisInt >= 1000000000)
            {
                shortNumberString = (thisInt / 1000000000).ToString("0.#") + "B";
            }
            else if (thisInt >= 1000000)
            {
                shortNumberString = (thisInt / 1000000).ToString("0.#") + "M";
            }
            else if (thisInt >= 1000)
            {
                shortNumberString = (thisInt / 1000).ToString("0.#") + "k";
            }

            return shortNumberString;
        }
    }
}
