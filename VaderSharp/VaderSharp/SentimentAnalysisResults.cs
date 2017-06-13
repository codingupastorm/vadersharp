namespace VaderSharp
{
    /// <summary>
    /// A model to represent the result of analysis.
    /// </summary>
    public class SentimentAnalysisResults
    {
        /// <summary>
        /// Negative
        /// </summary>
        public double Negative { get; set; }
        
        /// <summary>
        /// Neutral
        /// </summary>
        public double Neutral { get; set; }
        
        /// <summary>
        /// Positive
        /// </summary>
        public double Positive { get; set; }
        
        /// <summary>
        /// Compound
        /// </summary>
        public double Compound { get; set; }
    }
}
