namespace GalileoDriver.Interfaces
{
    internal interface ITransmission
    {
        /// <summary>
        /// Move with linear spear in direction
        /// V(r/l) = V +/- (widht / 2) * direction / t
        /// </summary>
        /// <param name="speed">Linear speed</param>
        /// <param name="direction">Rotation angle. Right > 0, Left < 0</param>
        void Move(double speed, double direction);

    }
}
