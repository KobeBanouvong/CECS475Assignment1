using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace assignment1
{
    public class StockBroker
    {
        public string BrokerName { get; set; }
        public List<Stock> stocks = new List<Stock>();

        private static readonly string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lab1_output.txt");
        private static readonly object fileLock = new object();
        private static bool headerWritten = false;

        // Titles for the columns in the output file
        private static readonly string titles = "Broker".PadRight(16) + "Stock".PadRight(15) + "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time";

        // Constructor for StockBroker
        public StockBroker(string brokerName)
        {
            BrokerName = brokerName;
            // Write the header only once
            WriteHeaderIfNeeded();
        }

        // Static method to ensure the header is written only once
        private static void WriteHeaderIfNeeded()
        {
            lock (fileLock)
            {
                if (!headerWritten)
                {
                    try
                    {
                        // Create or overwrite the file with the header
                        using (StreamWriter outputFile = new StreamWriter(destPath, false)) // 'false' to overwrite
                        {
                            outputFile.WriteLine(titles);
                        }
                        headerWritten = true; // Set the flag to indicate header has been written
                        Console.WriteLine(titles);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Adds a stock to the broker's list of stocks and registers the Notify listener.
        /// </summary>
        /// <param name="stock">Stock object to be added</param>
        public void AddStock(Stock stock)
        {
            stocks.Add(stock);
            stock.StockEvent += EventHandler;  // Register the Notify listener (EventHandler)
        }

        /// <summary>
        /// The event handler that raises the event of a stock change
        /// </summary>
        /// <param name="sender">The stock that triggered the event</param>
        /// <param name="e">Event arguments containing stock information</param>
        private void EventHandler(object sender, EventArgs e)
        {
            StockNotificationEventArgs stockEventArgs = e as StockNotificationEventArgs;
            if (stockEventArgs != null)
            {
                Helper(sender, stockEventArgs);
            }
        }

        /// <summary>
        /// Helper method that writes stock information to the file and console when the stock's threshold is reached.
        /// </summary>
        /// <param name="sender">The stock that triggered the event</param>
        /// <param name="e">Event arguments containing stock details</param>
        private void Helper(object sender, StockNotificationEventArgs e)
        {
            Stock stock = (Stock)sender;
            string message = $"{BrokerName.PadRight(16)}{e.StockName.PadRight(15)}" +
                             $"{e.CurrentValue.ToString().PadRight(10)}" +
                             $"{e.NumChanges.ToString().PadRight(10)}" +
                             $"{DateTime.Now.ToString()}";

            try
            {
                lock (fileLock)
                {
                    using (StreamWriter outputFile = new StreamWriter(destPath, true)) // 'true' to append
                    {
                        outputFile.WriteLine(message);
                    }
                }

                // Also write the stock information to the console
                Console.WriteLine(message);
                Console.Out.Flush();  // Flush the console output buffer
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
    }
}
