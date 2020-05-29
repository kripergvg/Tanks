namespace Tanks
{
    public class UnityTimeProvider : ITimeProvider
    {
        public float Time => UnityEngine.Time.time;
    }
}