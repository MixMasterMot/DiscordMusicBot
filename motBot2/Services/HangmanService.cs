using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace motBot2.Services
{
    class HangmanService
    {
        private static string hangManWord = null;
        private static string hdnHangManWord = null;
        private static List<string> LtrList = new List<string>();
        private static int strikes = 0;
        private static int hints = 3;

        private static bool hangManGame = false;

        public static Embed NewGame()
        {
            hangManGame = true;
            strikes = 0;
            hints = 3;
            LtrList.Clear();
            hdnHangManWord = "";

            string json = File.ReadAllText("Configs/hngman.json");
            List<string> words = JsonConvert.DeserializeObject<List<string>>(json);

            Random r = new Random();
            int rInt = r.Next(0, words.Count - 1);
            hangManWord = words[rInt];
            hdnHangManWord = new string('-', hangManWord.Length);
            string takenLtrs = "";
            string hangman = BuildHangMan(strikes);

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(hdnHangManWord)
                .WithDescription(hangman)
                .WithColor(Color.Blue)
                .WithFooter(takenLtrs);
            Embed cmp = builder.Build();

            return cmp;
        }

        public static Tuple<string, Embed> HangManWordContains(string ltr)
        {
            if (hangManGame == false)
            {
                return new Tuple<string, Embed>("First start a game", null);
            }
            if (LtrList.Contains(ltr))
            {
                return new Tuple<string, Embed>("Letter already used", null);
            }
            if (!ltr.All(Char.IsLetter))
            {
                return new Tuple<string, Embed>("Only letters you idiot", null);
            }
            LtrList.Add(ltr);
            if (hangManWord.Contains(ltr))
            {
                List<int> eachOcr = new List<int>();
                for (int i = 0; i < hangManWord.Length; i++)
                {
                    if (Convert.ToString(hangManWord[i]) == ltr)
                    {
                        eachOcr.Add(i);
                    }
                }
                char[] chars = hdnHangManWord.ToCharArray();
                foreach (int i in eachOcr)
                {
                    chars[i] = Convert.ToChar(ltr);
                }
                hdnHangManWord = new string(chars);

            }
            else
            {
                strikes = strikes + 1;
            }

            string hangman = BuildHangMan(strikes);
            string takenLtrs = null;
            foreach (string str in LtrList)
            {
                takenLtrs = takenLtrs + str + ", ";
            }

            EmbedBuilder builder = new EmbedBuilder();
            Embed cmp = null;
            if (strikes < 6)
            {
                if (!hdnHangManWord.Contains("-"))
                {
                    takenLtrs = "Blyt you saved him";
                    hangManGame = false;
                }

                builder.WithTitle(hdnHangManWord)
                .WithDescription(hangman)
                .WithColor(Color.Blue)
                .WithFooter(takenLtrs);
                cmp = builder.Build();
            }
            else
            {
                builder.WithTitle(hangManWord)
                .WithDescription(hangman)
                .WithColor(Color.Red)
                .WithFooter("YOU KILLLED HIM!!!!");
                cmp = builder.Build();
                hangManGame = false;
            }



            return new Tuple<string, Embed>("", cmp);
        }

        public static Tuple<string, Embed> Hint()
        {
            if (hangManGame == false)
            {
                return new Tuple<string, Embed>("Please start a game", null);
            }
            if (hints <= 0)
            {
                return new Tuple<string, Embed>("No hints left", null);
            }
            List<int> avaid = new List<int>();
            for (int i = 0; i < hdnHangManWord.Length; i++)
            {
                if (hdnHangManWord[i] == Convert.ToChar("-"))
                {
                    avaid.Add(i);
                }
            }

            Random r = new Random();
            int rInt = r.Next(0, avaid.Count - 1);
            int revIndex = avaid[rInt];
            char ltr = hangManWord[revIndex];

            List<int> eachOcr = new List<int>();
            for (int i = 0; i < hangManWord.Length; i++)
            {
                if (hangManWord[i] == ltr)
                {
                    eachOcr.Add(i);
                }
            }
            char[] chars = hdnHangManWord.ToCharArray();
            foreach (int i in eachOcr)
            {
                chars[i] = ltr;
            }
            hdnHangManWord = new string(chars);

            LtrList.Add(Convert.ToString(ltr));
            hints = hints - 1;

            string takenLtrs = null;
            foreach (string str in LtrList)
            {
                takenLtrs = takenLtrs + str + ", ";
            }
            string hangman = BuildHangMan(strikes);

            EmbedBuilder builder = new EmbedBuilder();
            Embed cmp = null;
            builder.WithTitle(hdnHangManWord)
                .WithDescription(hangman)
                .WithColor(Color.Blue)
                .WithFooter(takenLtrs);
            cmp = builder.Build();

            return new Tuple<string, Embed>("", cmp);
        }

        private static string BuildHangMan(int i)
        {
            string body = null;
            List<string> hangBase = new List<string>();
            if (i == 0)
            {
                //base
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            else if (i == 1)
            {
                //head
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------0------ \n");
                hangBase.Add("00-------------------0------ \n");
                hangBase.Add("00----------------0----0---- \n");
                hangBase.Add("00-------------------0------ \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            else if (i == 2)
            {
                //back
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00----------------0----0---- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00------------------0------- \n");
                hangBase.Add("00------------------0------- \n");
                hangBase.Add("00------------------0------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            else if (i == 3)
            {
                //arm R
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00----------------0----0---- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00------------------00------ \n");
                hangBase.Add("00------------------0--0---- \n");
                hangBase.Add("00------------------0----0-- \n");
                hangBase.Add("00------------------------0 \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            else if (i == 4)
            {
                //arm L
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00----------------0----0---- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-----------------000------ \n");
                hangBase.Add("00---------------0--0--0---- \n");
                hangBase.Add("00-------------0----0----0-- \n");
                hangBase.Add("00-----------0------------0 \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            else if (i == 5)
            {
                //Leg R
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00----------------0----0---- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-----------------000------ \n");
                hangBase.Add("00---------------0--0--0---- \n");
                hangBase.Add("00-------------0----0----0-- \n");
                hangBase.Add("00-----------0-----00-----0 \n");
                hangBase.Add("00---------------------0---- \n");
                hangBase.Add("00-----------------------0-- \n");
                hangBase.Add("00-------------------------0 \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            else if (i == 6)
            {
                //Leg L
                hangBase.Add("0000000000000000000000000000 \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00----------------0----0---- \n");
                hangBase.Add("00-------------------0------- \n");
                hangBase.Add("00-----------------000------ \n");
                hangBase.Add("00---------------0--0--0---- \n");
                hangBase.Add("00-------------0----0----0-- \n");
                hangBase.Add("00-----------0----000-----0 \n");
                hangBase.Add("00---------------0-----0---- \n");
                hangBase.Add("00-------------0---------0-- \n");
                hangBase.Add("00-----------0-------------0 \n");
                hangBase.Add("00-------------------------- \n");
                hangBase.Add("0000000000000000000000000000 \n");
            }
            foreach (string str in hangBase)
            {
                body = body + str;
            }
            return body;
        }
    }
}
