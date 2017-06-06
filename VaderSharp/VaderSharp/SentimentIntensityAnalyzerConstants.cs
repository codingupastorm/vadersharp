using System;
using System.Collections.Generic;
using System.Text;

namespace VaderSharp
{
    public partial class SentimentIntensityAnalyzer
    {
        private const double BIncr = 0.293;
        private const double BDecr = -0.293;
        private const double CIncr = 0.733;
        private const double NScalar = -0.74;

        private static readonly string[] PuncList = new string[]
        {
            ".", "!", "?", ",", ";", ":", "-", "'", "\"","!!", "!!!",
            "??", "???", "?!?", "!?!", "?!?!", "!?!?"
        };

        private static readonly string[] Negate = new string[]
        {
            "aint", "arent", "cannot", "cant", "couldnt", "darent", "didnt", "doesnt",
            "ain't", "aren't", "can't", "couldn't", "daren't", "didn't", "doesn't",
            "dont", "hadnt", "hasnt", "havent", "isnt", "mightnt", "mustnt", "neither",
            "don't", "hadn't", "hasn't", "haven't", "isn't", "mightn't", "mustn't",
            "neednt", "needn't", "never", "none", "nope", "nor", "not", "nothing", "nowhere",
            "oughtnt", "shant", "shouldnt", "uhuh", "wasnt", "werent",
            "oughtn't", "shan't", "shouldn't", "uh-uh", "wasn't", "weren't",
            "without", "wont", "wouldnt", "won't", "wouldn't", "rarely", "seldom", "despite"
        };

        private static readonly Dictionary<string, double> BoosterDict = new Dictionary<string, double>
        {
            {"absolutely", BIncr},
            {"amazingly", BIncr},
            {"awfully", BIncr},
            {"completely", BIncr},
            {"considerably", BIncr},
            {"decidedly", BIncr},
            {"deeply", BIncr},
            {"effing", BIncr},
            {"enormously", BIncr},
            {"entirely", BIncr},
            {"especially", BIncr},
            {"exceptionally", BIncr},
            {"extremely", BIncr},
            {"fabulously", BIncr},
            {"flipping", BIncr },
            {"flippin", BIncr},
            {"fricking", BIncr},
            {"frickin", BIncr},
            {"frigging", BIncr},
            {"friggin", BIncr},
            {"fully", BIncr},
            {"fucking", BIncr},
            {"greatly", BIncr},
            {"hella", BIncr},
            {"highly", BIncr},
            {"hugely", BIncr},
            {"incredibly", BIncr},
            {"intensely", BIncr},
            {"majorly", BIncr},
            {"more", BIncr},
            {"most", BIncr},
            {"particularly", BIncr},
            {"purely", BIncr},
            {"quite", BIncr},
            {"really", BIncr},
            {"remarkably", BIncr},
            {"so", BIncr},
            {"substantially", BIncr},
            {"thoroughly", BIncr},
            {"totally", BIncr},
            {"tremendously", BIncr},
            {"uber", BIncr},
            {"unbelievably", BIncr},
            {"unusually", BIncr},
            {"utterly", BIncr},
            {"very", BIncr},
            { "almost", BDecr},
            { "barely", BDecr},
            { "hardly", BDecr},
            { "just enough", BDecr},
            { "kind of", BDecr},
            { "kinda", BDecr},
            { "kindof", BDecr},
            { "kind-of", BDecr},
            { "less", BDecr},
            { "little", BDecr},
            { "marginally", BDecr},
            { "occasionally", BDecr},
            { "partly", BDecr},
            { "scarcely", BDecr},
            { "slightly", BDecr},
            { "somewhat", BDecr},
            {"sort of", BDecr},
            { "sorta", BDecr},
            { "sortof", BDecr},
            { "sort-of", BDecr}
        };

        private static readonly Dictionary<string,double> SpecialCaseIdioms = new Dictionary<string, double>
        {
            {"the shit", 3},
            { "the bomb", 3},
            { "bad ass", 1.5},
            { "yeah right", -2},
            { "cut the mustard", 2},
            { "kiss of death", -1.5},
            { "hand to mouth", -2}
        };
    }
}
