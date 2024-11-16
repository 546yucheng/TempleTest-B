namespace Aprojectbackend.DTO.actDTO
{

    public class actPagesList
    {
        //public int? currentPage { get; set; }

        public int? pages { get; set; }

        public List<actList> actList { get; set; }
    }

    public class actList
    {
        public int FActId { get; set; }

        public string FActName { get; set; }

        public string FActLocation { get; set; }

        public decimal? FRegFee { get; set; }

        public string FActCategory { get; set; }

        public string? FActImgPath { get; set; }

        public string? FActImgName { get; set; }
    }
    public class actSearch
    {
        public int FActCategoryId { get; set; }

        public string? txtKeyword { get; set; }
        public int targetPage { get; set; }

    }
}
