namespace Dev.Web.UFD.Areas.Models
{
    public class GuidGeneratorModel
    {
        public GuidGeneratorModel()
        {
            NumberOfGuids = 1;
            GuidGenerateInString = string.Empty;
        }
        public int NumberOfGuids { get; set; }

        public string GuidGenerateInString { get; set; }
    }
}
