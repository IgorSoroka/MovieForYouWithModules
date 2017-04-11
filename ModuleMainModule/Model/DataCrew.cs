namespace MainModule
{
    public class DataCrew
    {
        public DataCrew() { }

        public DataCrew(string creditId, int id, string name, string profile, string department, string job)
        {
            CreditId = creditId;
            Id = id;
            Name = name;
            Path = profile;
            Department = department;
            Job = job;
        }

        public string CreditId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
    }
}