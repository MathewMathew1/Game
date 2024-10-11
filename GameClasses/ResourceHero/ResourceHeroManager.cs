namespace BoardGameBackend.Models
{
    public class ResourceHeroManager
    {
        private readonly Dictionary<ResourceHeroType, int> _ResourceHeros;

        public ResourceHeroManager()
        {
            _ResourceHeros = new Dictionary<ResourceHeroType, int>
            {
                { ResourceHeroType.Army, 0 },
                { ResourceHeroType.Siege, 0 },
                { ResourceHeroType.Magic, 0 },
                { ResourceHeroType.Signet, 0 },
            };
        }

        public int GetResourceHeroAmount(ResourceHeroType ResourceHeroType)
        {
            return _ResourceHeros.TryGetValue(ResourceHeroType, out var amount) ? amount : 0;
        }

        public void AddResource(ResourceHeroType ResourceHeroType, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount to add cannot be negative.");

            _ResourceHeros[ResourceHeroType] += amount;
        }

        public bool SubtractResource(ResourceHeroType ResourceHeroType, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount to subtract cannot be negative.");

            if (_ResourceHeros[ResourceHeroType] < amount)
                return false;

            _ResourceHeros[ResourceHeroType] -= amount;
            return true;
        }

        public void ResetResources()
        {
            foreach (var key in _ResourceHeros.Keys.ToList())
            {
                _ResourceHeros[key] = 0;
            }
        }

        public Dictionary<ResourceHeroType, int> GetResources(){
            return _ResourceHeros;
        }
    }
}