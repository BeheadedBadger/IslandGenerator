namespace IslandGenerator.Models
{
    public class Cell
    {
        public String icon = "🌊";
        public Type type = Type.SaltWater;
        public String id = "A0";
        public int row = 0;
        public int column = 0;
        public List<Cell> neighbours = new List<Cell>();

        public enum Type { 
            SaltWater,
            FreshWater,
            Loam,
            Sand,
            Silt,
            Clay
        }

        public Cell(string id)
        {
            this.id = id;
        }

        public Cell(string icon, Type type, string id)
        {
            this.icon = icon;
            this.type = type;
            this.id = id;
        }
    }
}
