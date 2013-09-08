using System;

namespace LxTools.Tlpd
{
    public enum TlpdEntityType
    {
        Invalid, Player, Map
    }
    public struct TlpdEntity
    {
        public static TlpdEntity InvalidEntity
        {
            get { return new TlpdEntity() { Type = TlpdEntityType.Invalid }; }
        }

        public TlpdEntityType Type;
        public string Id;
        public string Race;
        public string Database;
        public string Name;

        public bool IsValid
        {
            get { return (this.Type == TlpdEntityType.Map) || (this.Type == TlpdEntityType.Player); }
        }
        public string Url
        {
            get
            {
                if (this.Type == TlpdEntityType.Invalid)
                    return "http://www.teamliquid.net/tlpd/";
                else
                    return string.Format("http://www.teamliquid.net/tlpd/{0}/{1}/{2}",
                        this.Database, this.Type.ToString().ToLower() + "s", this.Id);
            }
        }
        public string Code
        {
            get
            {
                switch (Type)
                {
                    case TlpdEntityType.Map:
                        return string.Format("[tlpd#maps#{0}#{2}{1}]{3}[/tlpd]", Id, (Race.Length > 0 ? "#" + Race : ""), Database, Name);
                    case TlpdEntityType.Player:
                        return string.Format("[tlpd#players#{0}#{1}#{2}]{3}[/tlpd]", Id, Race, Database, Name);
                    default:
                        return string.Empty;
                }
            }
        }
        public string CodeSimple
        {
            get
            {
                switch (Type)
                {
                    case TlpdEntityType.Map:
                        return string.Format("[tlpd#maps#{0}#{2}]{3}[/tlpd]", Id, Race, (!string.IsNullOrWhiteSpace(Database) ? Database : "a"), Name);
                    case TlpdEntityType.Player:
                        return string.Format("[tlpd#players#{0}#{1}]{3}[/tlpd]", Id, Race, Database, Name);
                    default:
                        return string.Empty;
                }
            }
        }
        public override string ToString()
        {
            return this.Code;
        }
    }
}
