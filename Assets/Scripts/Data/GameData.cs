using System;
using Infrastructure;

namespace Data
{
    public class GameData
    {
        public int MoneyCount;
        public int HealthCount;
        public int TempHealthCount;

        public event Action TempHealthCountChangedUp;
        public event Action TempHealthCountChangedDown;

        public GameData(SceneId sceneId)
        {
            if (sceneId == SceneId.Main)
            {
                MoneyCount = 0;
                HealthCount = 5;
            }
        }

        public void IncreaseLifes()
        {
            TempHealthCount++;
            TempHealthCountChangedUp?.Invoke();
        }

        public void ReduceLifes()
        {
            TempHealthCount--;
            TempHealthCountChangedDown?.Invoke();
        }

        public void SaveTempLifesCount() =>
            HealthCount = TempHealthCount;
    }
}