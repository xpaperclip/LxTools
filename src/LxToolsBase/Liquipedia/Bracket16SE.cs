using System;

namespace LxTools.Liquipedia
{
    public class Bracket16SE
    {
        public BracketLine[] RoundOf16 = new BracketLine[16];
        public BracketLine[] RoundOf8 = new BracketLine[8];
        public BracketLine[] RoundOf4 = new BracketLine[4];
        public BracketLine[] RoundOf2 = new BracketLine[2];

        public void Fill()
        {
            // fill first round empty slots with byes
            for (int i = 0; i < 16; i++)
            {
                if (string.IsNullOrEmpty(RoundOf16[i].ID))
                    RoundOf16[i] = BracketLine.Bye;
            }

            // advance winners
            FillRoundWinners(RoundOf16, RoundOf8);
            FillRoundWinners(RoundOf8, RoundOf4);
            FillRoundWinners(RoundOf4, RoundOf2);
            FillRoundWinners(RoundOf2, null);
        }
        private void FillRoundWinners(BracketLine[] curr, BracketLine[] next)
        {
            for (int i = 0; i < curr.Length; i += 2)
            {
                // set byes adjacent to winners
                if (curr[i].IsBye && !curr[i + 1].IsBye)
                {
                    curr[i + 1].Win = true;
                    curr[i + 1].Score = "W";
                }
                else if (!curr[i].IsBye && curr[i + 1].IsBye)
                {
                    curr[i].Win = true;
                    curr[i].Score = "W";
                }
                else
                {
                    // fill missing scores with 0s
                    if (curr[i].Win && string.IsNullOrEmpty(curr[i + 1].Score))
                        curr[i + 1].Score = "0";
                    if (curr[i + 1].Win && string.IsNullOrEmpty(curr[i].Score))
                        curr[i].Score = "0";
                }

                if (next == null)
                    continue;

                if (string.IsNullOrEmpty(next[i / 2].ID))
                {
                    // advance forward if the next slot is empty
                    if (curr[i].IsBye && curr[i + 1].IsBye)
                    {
                        next[i / 2] = BracketLine.Bye;
                    }

                    // advance known parameters
                    if (curr[i].Race == curr[i + 1].Race) next[i / 2].Race = curr[i].Race;
                    if (curr[i].Flag == curr[i + 1].Flag) next[i / 2].Flag = curr[i].Flag;

                    // advance winners
                    if (curr[i].Win) next[i / 2] = curr[i].Advance();
                    if (curr[i + 1].Win) next[i / 2] = curr[i + 1].Advance();
                }
                else
                {
                    // advance walkover wins backward
                    if (next[i / 2].IDPlain == curr[i].IDPlain && !curr[i].Win)
                    {
                        curr[i].Win = true;
                        if (string.IsNullOrEmpty(curr[i].Score))
                        {
                            curr[i].Score = "W";
                            curr[i + 1].Score = "-";
                        }
                    }
                    if (next[i / 2].IDPlain == curr[i + 1].IDPlain && !curr[i + 1].Win)
                    {
                        curr[i + 1].Win = true;
                        if (string.IsNullOrEmpty(curr[i + 1].Score))
                        {
                            curr[i + 1].Score = "W";
                            curr[i].Score = "-";
                        }
                    }
                }
            }
        }

        public string GetMarkup(LpTemplateWriter writer)
        {
            Bag bag = new Bag();
            for (int i = 1; i <= 16; i++) RoundOf16[i - 1].FillBag(bag, "R1D" + i.ToString());
            for (int i = 1; i <= 8; i++) RoundOf8[i - 1].FillBag(bag, "R2W" + i.ToString());
            for (int i = 1; i <= 4; i++) RoundOf4[i - 1].FillBag(bag, "R3W" + i.ToString());
            for (int i = 1; i <= 2; i++) RoundOf2[i - 1].FillBag(bag, "R4W" + i.ToString());
            return writer.Write(bag);
        }
    }
}
