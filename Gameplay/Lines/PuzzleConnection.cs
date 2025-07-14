using UnityEngine;

namespace OWMiniature.Gameplay.Lines
{
    public class PuzzleConnection
    {
        public NomaiMultiPartDoor[] Doors;

        public void SetOpenState(bool isOpen)
        {
            foreach (NomaiMultiPartDoor door in Doors)
            {
                if (isOpen)
                {
                    door.Open(null);
                    continue;
                }

                door.Close(null);
            }
        }

        public void SetOpenState(bool isOpen)
        {
            foreach (NomaiMultiPartDoor door in Doors)
            {
                if (isOpen)
                {
                    door.Open(null);
                    continue;
                }

                door.Close(null);
            }
        }
    }
}
