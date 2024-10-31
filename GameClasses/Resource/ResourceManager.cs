using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{
    public class ResourceManager
    {
        private readonly Dictionary<ResourceType, int> _resources;
        private readonly PlayerInGame _player;
        public int GoldIncome { get; set; } = 0;
        public List<ResourceType> allowedSubstituteResources = new List<ResourceType>
        {
            ResourceType.Wood,
            ResourceType.Iron,
            ResourceType.Gems,
            ResourceType.Niter
        };

        public ResourceManager(PlayerInGame player)
        {
            _player = player;
            _resources = new Dictionary<ResourceType, int>
            {
                { ResourceType.Gold, 2 },
                { ResourceType.Wood, 0 },
                { ResourceType.Iron, 0 },
                { ResourceType.Gems, 0 },
                { ResourceType.Niter, 0 },
                { ResourceType.MysticFog, 0 }
            };
        }

        public void EndOfTurnIncome()
        {
            foreach (var resource in _resources)
            {
                if (resource.Key != ResourceType.Gold)
                {
                    _resources[resource.Key] = 0;
                }

            }
        }

        public void EndOfRoundIncome()
        {
            foreach (var resource in _resources)
            {
                if (resource.Key == ResourceType.Gold)
                {
                    _resources[resource.Key] += GoldIncome;
                }

            }
        }

        public void AddGoldIncome(int addedGoldIncome)
        {
            GoldIncome += addedGoldIncome;
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            return _resources.TryGetValue(resourceType, out var amount) ? amount : 0;
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount to add cannot be negative.");

            _resources[resourceType] += amount;
        }

        public List<Resource> AddResources(List<Resource> resourcesToAdd)
        {
            var addedResources = new List<Resource>();

            foreach (var resource in resourcesToAdd)
            {
                int bonusAmount = GetBonus(resource.Type, resource.Amount);
                int totalAmount = resource.Amount + bonusAmount;

                AddResource(resource.Type, totalAmount);

                addedResources.Add(new Resource(resource.Type, totalAmount));
            }

            return addedResources;
        }

        private int GetBonus(ResourceType resourceType, int amount)
        {
            int bonus = 0;

            switch (resourceType)
            {
                case ResourceType.Gold:
                    bonus = 0;
                    break;
                case ResourceType.Wood:
                    bonus = 0;
                    break;
            }

            return bonus;
        }

        public bool SubtractResource(ResourceType resourceType, int amount, EventManager eventManager)
        {
            if (amount < 0)
                throw new ArgumentException("Amount to subtract cannot be negative.");

            if (_resources[resourceType] < amount)
                return false;

            _resources[resourceType] -= amount;
            
            var eventArgs = new ResourceSpendEventData
            {
                ResourceLeft = _resources[resourceType],
                ResourceSpend = amount,
                PlayerId = _player.Id,
                ResourceType = resourceType
            };

            eventManager.Broadcast("ResourceSpendEvent", ref eventArgs);
            
            return true;
        }

        public void ResetResources()
        {
            foreach (var key in _resources.Keys.ToList())
            {
                _resources[key] = 0;
            }
        }

        public bool CheckForResourceAndRemoveThem(List<ResourceInfo> resources, EventManager eventManager)
        {
            foreach (var resource in resources)
            {
                int availableAmount = GetResourceAmount(resource.Name);

                if (availableAmount < resource.Amount)
                {
                    return false;
                }
            }

            foreach (var resource in resources)
            {
                SubtractResource(resource.Name, resource.Amount, eventManager);
            }

            return true;
        }

        public bool CheckForResourceAndRemoveThemWithSubstitue(List<ResourceInfo> resources, EventManager eventManager)
        {
            List<ResourceType> resourcesWithAmountGreaterThanZero = _resources
            .Where(resource => allowedSubstituteResources.Contains(resource.Key) && resource.Value > 0)
            .Select(resource => resource.Key)
            .ToList();

            var substituteResourcesUsed = 0;
            foreach (var resource in resources)
            {
                if (allowedSubstituteResources.Contains(resource.Name))
                {
                    substituteResourcesUsed += 1;
                    if (substituteResourcesUsed > resourcesWithAmountGreaterThanZero.Count)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                int availableAmount = GetResourceAmount(resource.Name);

                if (availableAmount < resource.Amount)
                {
                    return false;
                }
            }

            foreach (var resource in resources)
            {
                if (allowedSubstituteResources.Contains(resource.Name))
                {
                    SubtractResource(resourcesWithAmountGreaterThanZero[0], resource.Amount, eventManager);
                    resourcesWithAmountGreaterThanZero.RemoveAt(0);
                }
                else
                {
                    SubtractResource(resource.Name, resource.Amount, eventManager);
                }          
            };
            return true;
        }

        public Dictionary<ResourceType, int> GetResources()
        {
            return _resources;
        }
    }
}