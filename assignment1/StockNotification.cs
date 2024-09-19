using System;

namespace assignment1
{
    public class StockNotification : EventArgs
    {
        // Properties for stock notification
        public string StockName { get; set; }
        public int CurrentValue { get; set; }
        public int NumChanges { get; set; }

        /// <summary>
        /// Stock notification attributes that are set and changed
        /// </summary>
        /// <param name="stockName">Name of the stock</param>
        /// <param name="currentValue">Current value of the stock</param>
        /// <param name="numChanges">Number of changes the stock has undergone</param>
        public StockNotification(string stockName, int currentValue, int numChanges)
        {
            // Initialize the notification fields with the provided values
            this.StockName = stockName;         // Set the stock name
            this.CurrentValue = currentValue;   // Set the current stock value
            this.NumChanges = numChanges;       // Set the number of changes the stock has undergone
        }
    }
}