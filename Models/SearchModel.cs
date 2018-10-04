namespace ImageContest.Models
{
    public class SearchModel {

        public string SearchTerm { get; set; }
        public string ImgUrl1 { get; set; }
        public string ImgUrl2 { get; set; }

        public int ImgUrl1SearchIndex { get; set; }
        public int ImgUrl2SearchIndex { get; set; }

        public int Guess { get; set; }
        public string Message { get; internal set; }

        public bool ShowSearchBox { get {
            return (!string.IsNullOrEmpty(ImgUrl1) && Guess > 0);
        }}
        public bool ShowImageSelection { get {
            return (Guess == 0);
        }}
        public bool ShowResults { get {
            return ShowSearchBox;
        }}
    }
}
