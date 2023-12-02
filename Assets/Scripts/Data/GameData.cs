using System;
using Infrastructure;

namespace Data
{
    public class GameData
    {
        public int MoneyCount;
        public float HealthCount;
        public float TempHealthCount;
        public float Armor;

        public event Action TempHealthCountChangedUp;
        public event Action TempHealthCountChangedDown;

        public GameData(SceneId sceneId)
        {
            if (sceneId == SceneId.Level_1)
            {
                MoneyCount = 0;
                HealthCount = 10;
                TempHealthCount = 10;
                Armor = 0.5f;
            }
        }

        public void RefreshHealth(float damage)
        {
            TempHealthCount -= damage;
            TempHealthCountChangedDown?.Invoke();
        }

        public void SaveTempLifesCount() =>
            HealthCount = TempHealthCount;
    }
}