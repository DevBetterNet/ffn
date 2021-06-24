namespace Dev.Web.UFD.Areas.Models
{
    public class GuidGeneratorModel
    {
        public GuidGeneratorModel()
        {
            NumberOfGuids = 1;
            IsUpper = false;
            HasBraces = false;
            HasHypens = true;
            GuidGenerateInString = string.Empty;
        }
        public int NumberOfGuids { get; set; }
        public bool IsUpper { get; set; }
        public bool HasBraces { get; set; }
        public bool HasHypens { get; set; }
        public string GuidGenerateInString { get; set; }


    }
}
