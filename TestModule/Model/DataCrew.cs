namespace TestModule.Model
{
    public class DataCrew
    {
        public DataCrew() { }

        public DataCrew(string CreditId, int Id, string Name, string Profile, string Department, string Job)
        {
            this.CreditId = CreditId;
            this.Id = Id;
            this.Name = Name;
            this.Path = Profile;
            this.Department = Department;
            this.Job = Job;
        }

        public string CreditId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
    }
}
