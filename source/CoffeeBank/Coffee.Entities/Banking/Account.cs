using System;

namespace Coffee.Entities
{
    public class Account
    {
        public readonly string Number; //as primary id

        private decimal _currentSum;

        public Account()
        {
            Number = Guid.NewGuid().ToString();
            _currentSum = 0;
        }

        public Account(string number, decimal sum)
        {
            Number = number;
            _currentSum = sum;
        }


        public bool PutMoney(decimal value)
        {
            try
            {
                //there will be some DB work. So, if transaction was completed successfully, we'll return true.
                _currentSum += value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WithdrawMoney(decimal value)
        {
            try
            {
                if (_currentSum >= value)
                {
                    //there will be some DB work. So, if transaction was completed successfully, we'll return true.
                    _currentSum -= value;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
