namespace MainModule
{
    public class DataCast
    {
        public DataCast() { }

        public DataCast(string CreditId, int Id, string Name, string Profile, string Character)
        {
            this.CreditId = CreditId;
            this.Id = Id;
            this.Name = Name;
            this.Path = Profile;
            this.Character = Character;
        }

        public string CreditId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Character { get; set; }
    }
}
