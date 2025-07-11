using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;

namespace Kriptok.Games.BlastemUp.Enemies
{
    /// <summary>
    /// Estructura que almacena las distintas trayectorias que pueden adoptar los enemigos.
    /// </summary>
    public struct EnemyTrajectory
    {
        public readonly int InitX;
        public readonly int InitY;
        public readonly EnemyTrajectoryItem[] Items;

        public EnemyTrajectory(int x, int y, params EnemyTrajectoryItem[] trayecs)
        {            
            InitX = x;
            InitY = y - Consts.PlayRegionMinY;
            Items = trayecs;
        }

        public int Length => Items.Length;

        public struct EnemyTrajectoryItem
        {
            public int imagener_por_seccion;            

            /// <summary>
            /// Velocidad.
            /// </summary>
            public readonly Vector2F Speed;

            /// <summary>
            /// Aceleración.
            /// </summary>
            public readonly Vector2F Acceleration;

            public EnemyTrajectoryItem(int a, int speedX, int speedY, int accelerationX, int accelerationY)
            {
                imagener_por_seccion = a;

                Speed = new Vector2F(
                    speedX * Consts.SpeedModifier, 
                    speedY * Consts.SpeedModifier);

                Acceleration = new Vector2F(
                   accelerationX * Consts.SpeedModifier2,
                   accelerationY * Consts.SpeedModifier2);
            }
        }
    }
}
