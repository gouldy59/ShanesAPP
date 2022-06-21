namespace ShanesAPP.Model
{
    public class GoogleUserResultModel : ResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Given_Name { get; set; }
        public string Family_Name { get; set; }
        public string Picture { get; set; }
        public string Locale { get; set; }
    }
}
