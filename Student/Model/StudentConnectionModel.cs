namespace Student.Model
{
    public class StudentConnectionModel
    {
        public string FullName { get; set; } = "Nom Prenom";
        public string ServerIP { get; set; } = "localhost";
        public int Port { get; set; } = 8000;
    }
}