using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace assignment1
{
    //-----------------------------------------------------------------------------------
    public class Stock
    {
        // Event for stock notification
        public event EventHandler<StockNotificationEventArgs> StockEvent;

        // Name of the stock
        private string _name;

        // Starting value of the stock
        private int _initialValue;

        // Max change in stock value that is possible
        private int _maxChange;

        // Threshold value to notify subscribers to the event
        private int _threshold;

        // Number of changes the stock goes through
        private int _numChanges;

        // Current value of the stock
        private int _currentValue;

        private readonly Thread _thread;

        // Properties
        public string StockName { get => _name; set => _name = value; }
        public int InitialValue { get => _initialValue; set => _initialValue = value; }
        public int CurrentValue { get => _currentValue; set => _currentValue = value; }
        public int MaxChange { get => _maxChange; set => _maxChange = value; }
        public int Threshold { get => _threshold; set => _threshold = value; }
        public int NumChanges { get => _numChanges; set => _numChanges = value; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stock class that contains all the information and handles changes to stock value
        /// </summary>
        /// <param name="name">Stock name</param>
        /// <param name="startingValue">Starting stock value</param>
        /// <param name="maxChange">Maximum value change of the stock</param>
        /// <param name="threshold">The range for stock notifications</param>
        public Stock(string name, int startingValue, int maxChange, int threshold)
        {
            _name = name;
            _initialValue = startingValue;
            _currentValue = startingValue;
            _maxChange = maxChange;
            _threshold = threshold;
            _numChanges = 0;

            // Initialize the thread to simulate stock changes
            _thread = new Thread(new ThreadStart(Activate));
            _thread.Start();
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Activates the thread to change stock values over time
        /// </summary>
        public void Activate()
        {
            for (int i = 0; i < 25; i++)  // Simulate 25 stock changes
            {
                Thread.Sleep(500); // Pause for 1/2 second between changes
                ChangeStockValue();
            }
        }

        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Changes the stock value and raises the stock event if the threshold is exceeded
        /// </summary>
        public void ChangeStockValue()
        {
            Random rand = new Random();
            int change = rand.Next(1, MaxChange);
            CurrentValue += change;
            NumChanges++;

            if ((CurrentValue - InitialValue) > Threshold)
            {
                // Raise the StockEvent if the threshold is exceeded
                StockEvent?.Invoke(this, new StockNotificationEventArgs(StockName, CurrentValue, NumChanges));
            }
        }
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Event arguments for stock notification events
    /// </summary>
    public class StockNotificationEventArgs : EventArgs
    {
        public string StockName { get; }
        public int CurrentValue { get; }
        public int NumChanges { get; }

        public StockNotificationEventArgs(string stockName, int currentValue, int numChanges)
        {
            StockName = stockName;
            CurrentValue = currentValue;
            NumChanges = numChanges;
        }
    }
}
