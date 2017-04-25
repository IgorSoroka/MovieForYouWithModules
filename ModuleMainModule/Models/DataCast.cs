namespace MainModule
{
    public class DataCast
    {
        public DataCast() { }

        public DataCast(string creditId, int id, string name, string profile, string character)
        {
            CreditId = creditId;
            Id = id;
            Name = name;
            Path = profile;
            Character = character;
        }

        public string CreditId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Character { get; set; }
    }
}