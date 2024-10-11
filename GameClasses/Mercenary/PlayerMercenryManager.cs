using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PlayerMercenaryManager
    {
        public List<Mercenary> Mercenaries { get; set; } = new List<Mercenary>();


        public bool SetMercenaryProphecyFulfill(int id){
            var mercenary = Mercenaries.FirstOrDefault(m => m.InGameIndex == id);
            if(mercenary == null) return false;

            if(mercenary.TypeCard != 3) return false;

            mercenary.IsAlwaysFulfilled = true;

            return true;
        }

        public List<Mercenary> GetAllUnfilledProphecies(){
            var mercenaries = Mercenaries.FindAll(m => m.IsAlwaysFulfilled == false && m.TypeCard == 3);

            return mercenaries;
        }
    }


}