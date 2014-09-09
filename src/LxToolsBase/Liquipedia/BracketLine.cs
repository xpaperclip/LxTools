using System;

namespace LxTools.Liquipedia
{
    public struct BracketLine
    {
        public static BracketLine Bye
        {
            get
            {
                var bye = new BracketLine();
                bye.ID = "BYE";
                bye.Score = "-";
                bye.Race = "bye";
                return bye;
            }
        }
        public static BracketLine From(string ID, string Race, string Flag)
        {
            var line = new BracketLine();
            line.ID = ID;
            line.Race = Race;
            line.Flag = Flag;
            return line;
        }

        public string ID { get; set; }
        public string Score { get; set; }
        public bool Win { get; set; }
        private string _flag;
        public string Flag
        {
            get { return _flag; }
            set { _flag = (value ?? "").ToLower(); }
        }
        private string _race;
        public string Race
        {
            get { return _race; }
            set
            {
                if (value != "bye")
                    _race = (value ?? "").ToUpper();
                else
                    _race = (value ?? "");
            }
        }
        public string IDPlain
        {
            get
            {
                if (this.ID == null)
                    return "";
                else
                    return StringUtils.StripHtmlComments(this.ID).Trim();
            }
        }

        public bool IsBye
        {
            get { return Race == "bye"; }
        }

        public void FillBag(Bag bag, string identifier)
        {
            bag[string.Format("{0}", identifier)] = this.ID;
            bag[string.Format("{0}flag", identifier)] = this.Flag;
            bag[string.Format("{0}race", identifier)] = this.Race;
            bag[string.Format("{0}score", identifier)] = this.Score;
            bag[string.Format("{0}win", identifier)] = this.Win ? "1" : " ";
        }
        public BracketLine Advance()
        {
            BracketLine next = this;
            next.ID = this.IDPlain;
            next.Score = "";
            next.Win = false;
            return next;
        }

        public override string ToString()
        {
            return this.ID;
        }
    }
}
