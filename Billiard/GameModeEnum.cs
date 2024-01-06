using Kriptok.Exceptions;

namespace Billiard
{
    public enum GameModeEnum
    {
        None = 0,

        /// <summary>
        /// Se está apuntando con el taco.
        /// </summary>
        PointAt = 1,

        /// <summary>
        /// Se está indicando el efecto.
        /// </summary>
        Effect = 2,

        /// <summary>
        /// Se está aplicando la fuerza al tiro.
        /// </summary>
        Shoot = 3
    }

    public static class GameModeEnumExtensions
    {
        public static string ToText(this GameModeEnum mode)
        {
            switch (mode)
            {
                case GameModeEnum.None: return string.Empty;
                case GameModeEnum.PointAt: return "Apuntar";
                case GameModeEnum.Effect: return "Efecto";
                case GameModeEnum.Shoot: return "Tiro";
            };

            throw new InvalidEnumValueException<GameModeEnum>(mode);
        }
    }
}
